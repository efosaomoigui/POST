using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Node;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation.Utility.CellulantEncryptionService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CellulantPaymentService : ICellulantPaymentService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private IServiceCentreService _serviceCenterService;
        private readonly IWalletService _walletService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly INodeService _nodeService;
        private readonly IMessageSenderService _messageSenderService;
        public CellulantPaymentService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService,
            IWalletService walletService, IPaymentTransactionService paymentTransactionService, INodeService nodeService, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _nodeService = nodeService;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }

        #region Cellulant Transfer Management
        public Task<TransferDetailsDTO> GetAllTransferDetails(string reference)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetCellulantKey()
        {
            var apiKey = ConfigurationManager.AppSettings["CellulantKey"];
            return apiKey;
        }

        public async Task<string> DecryptKey(string encrytedKey)
        {
            return await Decrypt(encrytedKey);
        }

        public async Task<bool> AddCellulantTransferDetails(TransferDetailsDTO transferDetailsDTO)
        {
            try
            {
                if (transferDetailsDTO is null)
                {
                    throw new GenericException("invalid payload", $"{(int)HttpStatusCode.BadRequest}");
                }

                var entity = await _uow.TransferDetails.ExistAsync(x => x.SessionId == transferDetailsDTO.SessionId);
                if (entity)
                {
                    throw new GenericException($"This transfer details with SessionId {transferDetailsDTO.SessionId} already exist.", $"{(int)HttpStatusCode.Forbidden}");
                }

                if (transferDetailsDTO.ResponseCode == "00")
                {
                    transferDetailsDTO.TransactionStatus = "success";
                }
                else if (transferDetailsDTO.ResponseCode == "25")
                {
                    transferDetailsDTO.TransactionStatus = "failed";
                }
                else
                {
                    transferDetailsDTO.TransactionStatus = "pending";
                }

                var transferDetails = Mapper.Map<TransferDetails>(transferDetailsDTO);
                _uow.TransferDetails.Add(transferDetails);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria baseFilter)
        {
            var isAdmin = await CheckUserRoleIsAdmin();
            var isRegion = await CheckUserPrivilegeIsRegion();
            var isAccount = await CheckUserRoleIsAccount();


            List<TransferDetailsDTO> transferDetailsDto = new List<TransferDetailsDTO>();

            if (!isAdmin && !isRegion && !isAccount)
            {
                var crAccount = await GetServiceCentreCrAccount();

                if (string.IsNullOrWhiteSpace(crAccount))
                {
                    throw new GenericException($"Service centre does not have a CRAccount.");
                }

                transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter, crAccount);
            }
            else
            {
                if (isRegion == true)
                {
                    var crAccounts = await GetRegionServiceCentresCrAccount();
                    if (crAccounts.Count > 0)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter, crAccounts);
                    }
                }
                else
                {
                    if (isAdmin == true || isAccount == true)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter);
                    }
                }
            }

            return transferDetailsDto;
        }

        public async Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber)
        {
            var isAdmin = await CheckUserRoleIsAdmin();
            var isRegion = await CheckUserPrivilegeIsRegion();
            var isAccount = await CheckUserRoleIsAccount();

            List<TransferDetailsDTO> transferDetailsDto = new List<TransferDetailsDTO>();

            if (!isAdmin && !isRegion && !isAccount)
            {
                var crAccount = await GetServiceCentreCrAccount();

                if (string.IsNullOrWhiteSpace(crAccount))
                {
                    throw new GenericException($"Service centre does not have a CRAccount.");
                }

                transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber, crAccount);
            }
            else
            {
                if (isRegion == true)
                {
                    var crAccounts = await GetRegionServiceCentresCrAccount();
                    if (crAccounts.Count > 0)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber, crAccounts);
                    }
                }
                else
                {
                    if (isAdmin == true || isAccount == true)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber);
                    }
                }
            }

            return transferDetailsDto;
        }



        private async Task<string> GetServiceCentreCrAccount()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var userClaims = await _userService.GetClaimsAsync(currentUserId);

            string[] claimValue = null;
            string crAccount = "";
            foreach (var claim in userClaims)
            {
                if (claim.Type == "Privilege")
                {
                    claimValue = claim.Value.Split(':');   // format stringName:stringValue
                }
            }

            if (claimValue == null)
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            if (claimValue[0] == "ServiceCentre")
            {
                crAccount = await _uow.ServiceCentre.GetServiceCentresCrAccount(int.Parse(claimValue[1]));
            }
            else
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            return crAccount;
        }

        private async Task<List<string>> GetRegionServiceCentresCrAccount()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                List<string> crAccounts = new List<string>();
                List<int> serviceCenterIds = new List<int>();
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }

                if (claimValue == null)
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }

                if (claimValue[0] == "Region")
                {
                    var regionId = int.Parse(claimValue[1]);
                    serviceCenterIds = await _uow.RegionServiceCentreMapping.GetAllAsQueryable().Where(x => x.RegionId == regionId).Select(x => x.ServiceCentreId).ToListAsync();
                }
                else
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge region claim.");
                }

                if (serviceCenterIds.Count > 0)
                {
                    crAccounts = await _uow.ServiceCentre.GetAllAsQueryable().Where(x => serviceCenterIds.Contains(x.ServiceCentreId)).Select(x => x.CrAccount).ToListAsync();
                }
                return crAccounts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CheckUserRoleIsAdmin()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var userRoles = await _userService.GetUserRoles(currentUserId);

                bool isAdmin = false;
                foreach (var role in userRoles)
                {
                    if (role == "Admin")
                    {
                        isAdmin = true;   // set to true
                    }
                }

                return isAdmin;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckUserRoleIsAccount()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var userRoles = await _userService.GetUserRoles(currentUserId);

                bool isAccount = false;
                foreach (var role in userRoles)
                {
                    if (role == "Account")
                    {
                        isAccount = true;   // set to true
                    }
                }

                return isAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckUserPrivilegeIsRegion()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                bool isRegion = false;

                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }

                if (claimValue == null)
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }

                if (claimValue[0] == "Region")
                {
                    isRegion = true;
                }

                return isRegion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion


        #region Cellulant Payment Gateway

        public async Task<CellulantPaymentResponse> VerifyAndValidatePayment(CellulantWebhookDTO payload)
        {
            CellulantPaymentResponse response = new CellulantPaymentResponse();

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(payload.MerchantTransactionID);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                response = await ProcessPaymentForWaybill(payload);
            }
            else
            {
                response = await ProcessPaymentForWallet(payload);
            }

            return response;
        }

        private async Task<CellulantPaymentResponse> ProcessPaymentForWaybill(CellulantWebhookDTO payload)
        {
            //1. verify the payment 
            var verifyResult = payload;
            var result = new CellulantPaymentResponse
            {
                StatusCode = "183",
                StatusDescription = "Payment processed successfully",
                CheckoutRequestID = payload.CheckoutRequestID,
                MerchantTransactionID = payload.MerchantTransactionID
            };

            if (verifyResult.RequestStatusCode.Equals(178))
            {
                if (verifyResult.Payments != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == verifyResult.MerchantTransactionID);

                    if (paymentLog == null)
                        return result;

                    //2. if the payment successful
                    if (verifyResult.RequestStatusDescription.Equals("Request fully paid") && !paymentLog.IsWaybillSettled)
                    {
                        var checkAmount = ValidatePaymentValue(paymentLog.Amount, verifyResult.AmountPaid);

                        if (checkAmount)
                        {
                            //3. Process payment for the waybill if successful
                            PaymentTransactionDTO paymentTransaction = new PaymentTransactionDTO
                            {
                                Waybill = paymentLog.Waybill,
                                PaymentType = PaymentType.Online,
                                TransactionCode = paymentLog.Reference,
                                UserId = paymentLog.UserId
                            };

                            var processWaybillPayment = await _paymentTransactionService.ProcessPaymentTransaction(paymentTransaction);

                            if (processWaybillPayment)
                            {
                                //2. Update waybill Payment log
                                paymentLog.IsPaymentSuccessful = true;
                                paymentLog.IsWaybillSettled = true;
                            }
                        }
                    }

                    paymentLog.TransactionStatus = ProcessStatusCode(verifyResult.RequestStatusCode);
                    paymentLog.TransactionResponse = verifyResult.RequestStatusDescription;
                    await _uow.CompleteAsync();
                }
            }

            return result;
        }

        private async Task<CellulantPaymentResponse> ProcessPaymentForWallet(CellulantWebhookDTO payload)
        {
            //1. verify the payment 
            var verifyResult = payload;
            var result = new CellulantPaymentResponse
            {
                StatusCode = "183",
                StatusDescription = "Payment processed successfully",
                CheckoutRequestID = payload.CheckoutRequestID,
                MerchantTransactionID = payload.MerchantTransactionID
            };
            if (verifyResult.RequestStatusCode.Equals(178))
            {
                if (verifyResult.Payments != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == verifyResult.MerchantTransactionID);

                    if (paymentLog == null)
                        return result;

                    if (verifyResult.RequestStatusDescription != null)
                    {
                        verifyResult.RequestStatusDescription = verifyResult.RequestStatusDescription.ToLower();
                    }

                    bool sendPaymentNotification = false;
                    var walletDto = new WalletDTO();
                    var userPayload = new UserPayload();
                    var bonusAddon = new BonusAddOn();
                    bool checkAmount = false;

                    //2. if the payment successful
                    if (verifyResult.RequestStatusDescription.Equals("request fully paid") && !paymentLog.IsWalletCredited)
                    {
                        checkAmount = ValidatePaymentValue(paymentLog.Amount, verifyResult.AmountPaid);

                        if (checkAmount)
                        {
                            //a. update the wallet for the customer
                            string customerId = null;  //set customer id to null

                            //get wallet detail to get customer code
                            walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                            if (walletDto != null)
                            {
                                //use customer code to get customer id
                                var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                                if (user != null)
                                {
                                    customerId = user.Id;
                                    userPayload.Email = user.Email;
                                    userPayload.UserId = user.Id;
                                }
                            }

                            //if pay was done using Master VIsa card, give some discount
                            //bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                            //update the wallet
                            await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                            {
                                WalletId = paymentLog.WalletId,
                                Amount = paymentLog.Amount,
                                CreditDebitType = CreditDebitType.Credit,
                                Description = verifyResult.RequestStatusDescription,
                                PaymentType = PaymentType.Online,
                                PaymentTypeReference = paymentLog.Reference,
                                UserId = customerId
                            }, false);

                            sendPaymentNotification = true;

                            //3. update the wallet payment log
                            paymentLog.IsWalletCredited = true;
                        }
                    }

                    paymentLog.TransactionStatus = ProcessStatusCode(verifyResult.RequestStatusCode);
                    paymentLog.TransactionResponse = verifyResult.RequestStatusDescription;
                    await _uow.CompleteAsync();

                    if (sendPaymentNotification)
                    {
                        await SendPaymentNotificationAsync(walletDto, paymentLog);
                    }

                    //Call Node API for subscription process
                    if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && checkAmount)
                    {
                        if (userPayload != null)
                        {
                            await _nodeService.WalletNotification(userPayload);
                        }
                    }
                }
            }
            return result;
        }

        private WaybillWalletPaymentType GetPackagePaymentType(string refCode)
        {
            if (!string.IsNullOrWhiteSpace(refCode))
            {
                refCode = refCode.ToLower();
            }

            if (refCode.StartsWith("wb"))
            {
                return WaybillWalletPaymentType.Waybill;
            }

            return WaybillWalletPaymentType.Wallet;
        }

        private async Task SendPaymentNotificationAsync(WalletDTO walletDto, WalletPaymentLog paymentLog)
        {
            if (walletDto != null)
            {
                walletDto.Balance = walletDto.Balance + paymentLog.Amount;

                var message = new MessageDTO()
                {
                    CustomerCode = walletDto.CustomerCode,
                    CustomerName = walletDto.CustomerName,
                    ToEmail = walletDto.CustomerEmail,
                    To = walletDto.CustomerEmail,
                    Currency = walletDto.Country.CurrencySymbol,
                    Body = walletDto.Balance.ToString("N"),
                    Amount = paymentLog.Amount.ToString("N"),
                    Date = paymentLog.DateCreated.ToString("dd-MM-yyyy")
                };

                //send mail to customer
                await _messageSenderService.SendPaymentNotificationAsync(message);

                //send a copy to chairman
                var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);

                if (chairmanEmail != null)
                {
                    //seperate email by comma and send message to those email
                    string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();

                    foreach (string email in chairmanEmails)
                    {
                        message.ToEmail = email;
                        await _messageSenderService.SendPaymentNotificationAsync(message);
                    }
                }
            }
        }

        private bool ValidatePaymentValue(decimal shipmentAmount, decimal paymentAmount)
        {
            var factor = Convert.ToDecimal(Math.Pow(10, 0));
            paymentAmount = Math.Round(paymentAmount * factor) / factor;
            shipmentAmount = Math.Round(shipmentAmount * factor) / factor;

            decimal increaseShipmentPrice = shipmentAmount + 1;
            decimal decreaseShipmentPrice = shipmentAmount - 1;

            if (shipmentAmount == paymentAmount
                || increaseShipmentPrice == paymentAmount
                || decreaseShipmentPrice == paymentAmount)
            {
                return true;
            }

            return false;
        }

        private string ProcessStatusCode(int code)
        {
            switch (code)
            {
                case 129:
                    return "Request has expired with no partial payment received";
                case 176:
                    return "Partial payment made for the request and has been marked as closed.";
                case 178:
                    return "Full payment made for the request";
                case 179:
                    return "Request has expired with one or more partial payments made.";
                case 99:
                    return "A failed payment has been received for this request. Request is still open to receive payments";
                default:
                    break;
            }
            return string.Empty;
        }

        public async Task<CellulantResponseDTO> CheckoutEncryption(CellulantPayloadDTO payload)
        {
            string accessKey = ConfigurationManager.AppSettings["CellulantAccessKey"];
            string ivKey = ConfigurationManager.AppSettings["CellulantIVKey"];
            string secretKey = ConfigurationManager.AppSettings["CellulantSecretKey"];
            string serviceCode = ConfigurationManager.AppSettings["CellulantServiceCode"];

            ICellulantDataEncryption encryption = new CellulantDataEncryption(ivKey, secretKey);
            payload.serviceCode = serviceCode;

            string json = JsonConvert.SerializeObject(payload).Replace("/", "\\/");

            string encParams = encryption.EncryptData(json);
            var result = new CellulantResponseDTO { param = encParams, accessKey = accessKey, countryCode = payload.countryCode };
            return result;
        }

        #endregion
    }
}
