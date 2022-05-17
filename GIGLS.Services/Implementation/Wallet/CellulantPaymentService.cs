

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
using GIGLS.Infrastructure.Persistence;
using GIGLS.Services.Implementation.Utility.CellulantEncryptionService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        private readonly IStellasService _stellasService;
        public CellulantPaymentService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService,
            IWalletService walletService, IPaymentTransactionService paymentTransactionService, INodeService nodeService, IMessageSenderService messageSenderService, IStellasService stellasService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _nodeService = nodeService;
            _messageSenderService = messageSenderService;
            _stellasService = stellasService;
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
                    _uow.BeginTransaction(IsolationLevel.Serializable);
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

                            //Convert amount base on country rate if isConverted 
                            //1. CHeck if is converted equals true
                            if (paymentLog.isConverted)
                            {
                                //2. Get user country id
                                var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);
                                if (user == null)
                                {
                                    return result;
                                }

                                if (user.UserActiveCountryId <= 0)
                                {
                                    return result;
                                }

                                var userdestCountry = new CountryRouteZoneMap();

                                // Get conversion rate base of card type use
                                if (paymentLog.CardType == CardType.Naira)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 1 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Pound)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 62 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Dollar)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 207 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 76 && c.CompanyMap == CompanyMap.GIG);
                                }

                                if (userdestCountry == null)
                                {
                                    return result;
                                }

                                if (userdestCountry.Rate <= 0)
                                {
                                    return result;
                                }
                                //3. Convert base on country rate
                                var convertedAmount = Math.Round((userdestCountry.Rate * (double)paymentLog.Amount), 2);
                                paymentLog.Amount = (decimal)convertedAmount;
                            }

                            //update the wallet
                            await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                            {
                                WalletId = paymentLog.WalletId,
                                Amount = paymentLog.Amount,
                                CreditDebitType = CreditDebitType.Credit,
                                // Description = verifyResult.RequestStatusDescription,
                                Description = "Funding made through debit card",
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
                    _uow.Commit();
                    //await _uow.CompleteAsync();

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
            //string serviceCode = ConfigurationManager.AppSettings["CellulantServiceCode"];

            ICellulantDataEncryption encryption = new CellulantDataEncryption(ivKey, secretKey);
            //payload.serviceCode = serviceCode;

            string json = JsonConvert.SerializeObject(payload).Replace("/", "\\/");

            string encParams = encryption.EncryptData(json);
            var result = new CellulantResponseDTO { param = encParams, accessKey = accessKey, countryCode = payload.countryCode };
            return result;
        }

        public async Task<bool> CODCallBack(CODCallBackDTO cod)
        {
            var result = false;
            var customerCode = string.Empty;
            //0. validate payload
            if (cod == null)
            {
                throw new GenericException($"invalid payload", $"{(int)HttpStatusCode.Forbidden}");
            }
            var amount = Convert.ToDecimal(cod.CODAmount);
            if (String.IsNullOrEmpty(cod.Waybill) || amount <= 0)
            {
                throw new GenericException($"missing parameter", $"{(int)HttpStatusCode.Forbidden}");
            }

            //1. get sender details
            var shipmentInfo = await _uow.Shipment.GetAsync(x => x.Waybill == cod.Waybill);
            if (shipmentInfo == null)
            {
                var mobileShipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == cod.Waybill);
                if (mobileShipment == null)
                {
                    throw new GenericException($"waybill not found", $"{(int)HttpStatusCode.NotFound}");
                }
                else
                {

                    mobileShipment.CODStatusDate = DateTime.Now;
                    mobileShipment.CODStatus = CODMobileStatus.Collected;
                    mobileShipment.CODDescription = $"COD {CODMobileStatus.Collected.ToString()}({PaymentType.Transfer.ToString()})";

                    customerCode = mobileShipment.CustomerCode;
                }
            }
            if (shipmentInfo != null)
            {
                customerCode = shipmentInfo.CustomerCode;
                shipmentInfo.CODStatusDate = DateTime.Now;
                shipmentInfo.CODStatus = CODMobileStatus.Collected;
                shipmentInfo.CODDescription = $"COD {CODMobileStatus.Collected.ToString()}({PaymentType.Transfer.ToString()})";
            }
            var senderInfo = await _uow.Company.GetAsync(x => x.CustomerCode == customerCode);
            if (senderInfo == null)
            {
                throw new GenericException($"sender information not found", $"{(int)HttpStatusCode.NotFound}");
            }

            var senderWallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == customerCode);
            var currentUserId = await _userService.GetCurrentUserId();

            //2.update the cod shipment
            var codAccShipment = await _uow.CashOnDeliveryRegisterAccount.GetAsync(x => x.Waybill == cod.Waybill);
            if (codAccShipment != null)
            {
                codAccShipment.PaymentType = PaymentType.Transfer;
                codAccShipment.CODStatusHistory = CODStatushistory.RecievedAtServiceCenter;
                codAccShipment.PaymentTypeReference = cod.TransactionReference;
                codAccShipment.DestinationCountryId = senderInfo.UserActiveCountryId;
                codAccShipment.TransferAccount = cod.TransferAccount;
            }
            await _uow.CompleteAsync();


            var accInfo = await _uow.CODWallet.GetAsync(x => x.CustomerCode == customerCode);
            if (accInfo is null)
            {
                // throw new GenericException("user does not have a cod wallet");
                var err = new LogEntry()
                {
                    CallSite = "",
                    ErrorMessage = "user does not have a cod wallet",
                    ErrorMethod = "Stellas Tranfer",
                    ErrorSource = "Stellas API",
                    Username = cod.Waybill,
                    InnerErrorMessage = "user does not have a cod wallet",
                    DateTime = DateTime.Now.ToString()
                };
                using (GIGLSContext _context = new GIGLSContext())
                {
                    _context.LogEntry.Add(err);
                    await _context.SaveChangesAsync();
                }

                return true;
            }

            var no = Guid.NewGuid().ToString();
            no = no.Substring(0, 5);
            var refNo = $"TRX-{cod.Waybill}-CLNT-{no}";

            var koboValue = amount * 100;
            if (amount <= 0)
            {
                throw new GenericException("invalid amount");
            }
            var transferDTOStellas = new StellasTransferDTO()
            {
                Amount = Convert.ToString(koboValue),
                RetrievalReference = refNo,
                ReceiverAccountNumber = accInfo.AccountNo,
                Narration = $"COD Bank Transfer Payout for {cod.Waybill}",
                ReceiverBankCode = "200002",
            };

            var alreadyPaid = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == cod.Waybill && x.PaymentStatus == PaymentStatus.Paid);
            if (alreadyPaid != null)
            {
                return true;
            }

            var codTransferReg = new CODTransferRegister()
            {
                Amount = amount,
                RefNo = refNo,
                CustomerCode = customerCode,
                AccountNo = accInfo.AccountNo,
                PaymentStatus = PaymentStatus.Pending,
                Waybill = cod.Waybill,
                ClientRefNo = cod.TransactionReference
            };
            _uow.CODTransferRegister.Add(codTransferReg);
            await _uow.CompleteAsync();

            var stellasWithdraw = await _stellasService.StellasTransfer(transferDTOStellas);
            if (stellasWithdraw.status)
            {
                //update codtransferlog table to paid
                var codtransferlog = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == cod.Waybill);
                if (codtransferlog != null)
                {
                    codtransferlog.StatusCode = stellasWithdraw.status.ToString();
                    codtransferlog.StatusDescription = stellasWithdraw.message;
                }
                var res = await UpdateCODShipmentOnCallBackStellas(cod);
                result = res;
            }
            else if (!stellasWithdraw.status)
            {
                //update codtransferlog table to paid
                var codtransferlog = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == cod.Waybill);
                if (codtransferlog != null)
                {
                    codtransferlog.StatusCode = stellasWithdraw.status.ToString();
                    codtransferlog.StatusDescription = stellasWithdraw.message;
                }
            }

            //log stellas response
            string json = JsonConvert.SerializeObject(stellasWithdraw);
            var logEnt = new LogEntry()
            {
                CallSite = "",
                ErrorMessage = stellasWithdraw.message,
                ErrorMethod = "Stellas Tranfer",
                ErrorSource = "Stellas API",
                Username = cod.Waybill,
                InnerErrorMessage = json,
                DateTime = DateTime.Now.ToString()
            };
            using (GIGLSContext _context = new GIGLSContext())
            {
                _context.LogEntry.Add(logEnt);
                await _context.SaveChangesAsync();
            }
            await _uow.CompleteAsync();
            //3. TODO: deduct charges
            //4. TODO: send email to merchant
            result = true;
            return result;
        }
        #endregion

        #region Cellulant Webhook


        public async Task<CellulantPaymentResponse> VerifyAndValidatePaymentForWebhook(CellulantWebhookDTO webhook)
        {
            CellulantPaymentResponse result = new CellulantPaymentResponse();
            result.StatusCode = "183";
            result.StatusDescription = "Payment processed successfully";
            result.CheckoutRequestID = webhook.CheckoutRequestID;
            result.MerchantTransactionID = webhook.MerchantTransactionID;

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(webhook.MerchantTransactionID);

            var referenceCode = webhook.MerchantTransactionID;

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                //1. Get PaymentLog
                var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == referenceCode);

                if (paymentLog != null)
                {

                    if (paymentLog.OnlinePaymentType == OnlinePaymentType.Cellulant)
                    {
                        result = await VerifyAndValidateCellulantPayment(webhook);
                    }
                }
            }
            else
            {
                //1. Get PaymentLog
                var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == referenceCode);

                if (paymentLog != null)
                {
                    if (paymentLog.OnlinePaymentType == OnlinePaymentType.Cellulant)
                    {
                        result = await VerifyAndValidateCellulantPayment(webhook);
                    }
                }
            }

            return result;
        }

        private async Task<CellulantPaymentResponse> VerifyAndValidateCellulantPayment(CellulantWebhookDTO webhook)
        {
            var result = await VerifyAndValidatePayment(webhook);

            return result;
        }
        #endregion

        #region Cellulant TRANSFER PROCESS

        public async Task<CellulantTransferResponsePayload> Transfer(CellulantTransferPayload payload)
        {
            string celullantUrl = ConfigurationManager.AppSettings["CellulantTransferBeepUrl"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(payload);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(celullantUrl, data);
                string responseResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CellulantTransferResponsePayload>(responseResult);
                return result;
            }
        }

        public async Task<CellulantTransferResponsePayload> CelullantTransfer(CellulantTransferDTO transferDTO)
        {
            if (transferDTO is null)
            {
                throw new GenericException("invalid payload");
            }
            var user = await _uow.Company.GetCompanyByCode(transferDTO.CustomerCode);
            if (user is null)
            {
                throw new GenericException("ecommerce user does not exist");
            }
            var accInfo = await _uow.CODWallet.GetAsync(x => x.CustomerCode == transferDTO.CustomerCode);
            if (accInfo is null)
            {
                throw new GenericException("user does not have a cod wallet");
            }

            //log transaction into CODTransferRegister
            var shipmentInfo = await _uow.Shipment.GetAsync(x => x.Waybill == transferDTO.Waybill);
            if (shipmentInfo == null)
            {
                throw new GenericException($"waybill not found", $"{(int)HttpStatusCode.NotFound}");
            }
            var codTransferReg = new CODTransferRegister()
            {
                Amount = Convert.ToDecimal(transferDTO.Amount),
                RefNo = transferDTO.RefNo,
                CustomerCode = shipmentInfo.CustomerCode,
                AccountNo = accInfo.AccountNo,
                PaymentStatus = PaymentStatus.Pending,
                Waybill = transferDTO.Waybill,
                ClientRefNo = transferDTO.ClientRefNo
            };
            _uow.CODTransferRegister.Add(codTransferReg);
            await _uow.CompleteAsync();
            var phoneNo = user.PhoneNumber;
            var phoneNoIndex = user.PhoneNumber.IndexOf('+');
            if (phoneNoIndex >= 0)
            {
                phoneNo = user.PhoneNumber.Remove(phoneNoIndex, 1);
            }
            //test
            //var callback = "https://agilitysystemapidevm.azurewebsites.net/api/thirdparty/updateshipmentcallback";
            //live
            var callback = "https://thirdparty.gigl-go.com/api/thirdparty/updateshipmentcallback";


            var extraData = new ExtraData();
            extraData.callbackUrl = callback;
            extraData.DestinationAccountName = $"{accInfo.FirstName} {accInfo.LastName}";
            extraData.DestinationBankCode = $"200002";
            extraData.DestinationBank = $"Stellas";
            extraData.DestinationAccountNo = accInfo.AccountNo;

            string extraDataStringified = JsonConvert.SerializeObject(extraData);
            var today = DateTime.Now;
            string paymentDate = $"{today.Year}-{today.Month}-{today.Day} {today.Hour}:{today.Minute}:{today.Second}";
            string username = ConfigurationManager.AppSettings["CellulantUsername"];
            string pwd = ConfigurationManager.AppSettings["CellulantPwd"];
            string serviceCode = ConfigurationManager.AppSettings["CellulantBeepServiceCode"];
            var pak = new Packet();
            pak.ServiceCode = serviceCode;
            pak.MSISDN = phoneNo;
            pak.InvoiceNumber = transferDTO.RefNo;
            pak.AccountNumber = accInfo.AccountNo;
            pak.PayerTransactionID = transferDTO.RefNo;
            pak.Amount = transferDTO.Amount;
            pak.HubID = "";
            pak.Narration = "COD bank payout";
            pak.DatePaymentReceived = paymentDate;
            pak.ExtraData = extraDataStringified;
            pak.CurrencyCode = "NGN";
            pak.CustomerNames = $"{accInfo.FirstName} {accInfo.LastName}";
            pak.PaymentMode = "Bank";

            var payload = new CellulantTransferPayload();
            payload.CountryCode = "NG";
            payload.Function = "BEEP.postPayment";
            payload.Payload.Credentials.Password = pwd;
            payload.Payload.Credentials.Username = username;
            payload.Payload.Packet.Add(pak);
            var result = await Transfer(payload);

            string t = JsonConvert.SerializeObject(payload);
            return result;
        }

        public async Task<CellulantPushPaymentStatusResponse> UpdateCODShipmentOnCallBack(PushPaymentStatusRequstPayload payload)
        {
            var response = new CellulantPushPaymentStatusResponse();
            if (payload != null)
            {
                var payerTransacID = payload.Payload.Packet.PayerTransactionID;
                var getWaybill = payerTransacID.Split('-');
                var waybill = getWaybill[1];
                if (payload.Payload != null && payload.Payload.Packet != null && payload.Payload.Packet.StatusCode == "183")
                {
                    //1. get shipment details
                    var shipmentInfo = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                    if (shipmentInfo != null)
                    {
                        shipmentInfo.CODStatusDate = DateTime.Now;
                        shipmentInfo.CODStatus = CODMobileStatus.Paid;
                        shipmentInfo.CODDescription = $"COD {CODMobileStatus.Paid.ToString()}";
                    }
                    var mobileShipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == waybill);
                    if (mobileShipment != null)
                    {
                        mobileShipment.CODStatusDate = DateTime.Now;
                        mobileShipment.CODStatus = CODMobileStatus.Paid;
                        mobileShipment.CODDescription = $"COD {CODMobileStatus.Paid.ToString()}";
                    }

                    //update codtransferlog table to paid
                    var codtransferlog = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == waybill);
                    if (codtransferlog != null)
                    {
                        codtransferlog.PaymentStatus = PaymentStatus.Paid;
                        codtransferlog.StatusCode = payload.Payload.Packet.StatusCode;
                        codtransferlog.StatusDescription = payload.Payload.Packet.StatusDescription;
                        codtransferlog.ReceiverNarration = payload.Payload.Packet.ReceiverNarration;
                    }

                    await _uow.CompleteAsync();

                    response.AuthStatus.AuthStatusCode = 131;
                    response.AuthStatus.AuthStatusDescription = "API call doesn't need authentication";
                    response.Results.BeepTransactionID = payload.Payload.Packet.BeepTransactionID;
                    response.Results.PayerTransactionID = payload.Payload.Packet.PayerTransactionID;
                    response.Results.StatusCode = 188;
                    response.Results.StatusDescription = "Response was received";
                }

                else
                {
                    response.AuthStatus.AuthStatusCode = 131;
                    response.AuthStatus.AuthStatusDescription = "API call doesn't need authentication";
                    response.Results.BeepTransactionID = payload.Payload.Packet.BeepTransactionID;
                    response.Results.PayerTransactionID = payload.Payload.Packet.PayerTransactionID;
                    response.Results.StatusCode = Convert.ToInt32(payload.Payload.Packet.StatusCode);
                    response.Results.StatusDescription = payload.Payload.Packet.StatusDescription;

                    //update codtransferlog table to paid
                    var codtransferlog = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == waybill);
                    if (codtransferlog != null)
                    {
                        codtransferlog.StatusCode = payload.Payload.Packet.StatusCode;
                        codtransferlog.StatusDescription = payload.Payload.Packet.StatusDescription;
                        codtransferlog.ReceiverNarration = payload.Payload.Packet.ReceiverNarration;
                    }
                }
            }
            await _uow.CompleteAsync();
            return response;
        }

        public async Task<bool> UpdateCODShipmentOnCallBackStellas(CODCallBackDTO cod)
        {
            bool result = false;
            //1. get shipment details
            var shipmentInfo = await _uow.Shipment.GetAsync(x => x.Waybill == cod.Waybill);
            if (shipmentInfo != null)
            {
                shipmentInfo.CODStatusDate = DateTime.Now;
                shipmentInfo.CODStatus = CODMobileStatus.Paid;
                shipmentInfo.CODDescription = $"COD {CODMobileStatus.Paid.ToString()}";
            }
            var mobileShipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == cod.Waybill);
            if (mobileShipment != null)
            {
                mobileShipment.CODStatusDate = DateTime.Now;
                mobileShipment.CODStatus = CODMobileStatus.Paid;
                mobileShipment.CODDescription = $"COD {CODMobileStatus.Paid.ToString()}";
            }

            //update codtransferlog table to paid
            var codtransferlog = await _uow.CODTransferRegister.GetAsync(x => x.Waybill == cod.Waybill);
            if (codtransferlog != null)
            {
                codtransferlog.PaymentStatus = PaymentStatus.Paid;
            }
            await _uow.CompleteAsync();
            result = true;
            return result;
        }
        #endregion

        #region Cellulant Confirm Transfer
        public async Task<bool> GetTransferStatus(string craccount)
        {
            bool result = false;
            if (string.IsNullOrEmpty(craccount))
            {
                throw new GenericException("CR Account cannot be null or empty", $"{(int)HttpStatusCode.BadRequest}");
            }
            craccount = craccount.Trim();

            var sessionId = await GetSessionIdFromTransferDetails(craccount);
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new GenericException("SessionId not found", $"{(int)HttpStatusCode.NotFound}");
            }

            result = await ConfirmTransferStatus(sessionId);
            return result;
        }

        private async Task<bool> ConfirmTransferStatus(string sessionId)
        {
            bool result = false;
            string content = string.Empty;
            string url = ConfigurationManager.AppSettings["CellulantTransferUrl"];
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                httpClient.Timeout = new TimeSpan(0, 0, 30);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var request = new HttpRequestMessage(HttpMethod.Post, $"/GenericWave/Proxy/Query?sessionid={sessionId}&type=1") { };
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
                var status = JObject.Parse(content)["status"].ToString();

                if (!string.IsNullOrEmpty(status))
                {
                    result = status == "00" ? true : result;
                }
            }
            return result;
        }

        private async Task<string> GetSessionIdFromTransferDetails(string craccount)
        {
            if (string.IsNullOrEmpty(craccount))
            {
                throw new GenericException("CR Account cannot be null or empty", $"{(int)HttpStatusCode.BadRequest}");
            }

            var record = await _uow.TransferDetails.GetAsync(x => x.CrAccount == craccount);
            if (record == null)
            {
                throw new GenericException("Transfer details not found", $"{(int)HttpStatusCode.NotFound}");
            }

            return record.SessionId;
        }

        public async Task<CODPaymentResponse> GetCODPaymentReceivedStatus(string craccount)
        {
            CODPaymentResponse result = new CODPaymentResponse();
            if (string.IsNullOrEmpty(craccount))
            {
                throw new GenericException("CR Account cannot be null or empty", $"{(int)HttpStatusCode.BadRequest}");
            }
            craccount = craccount.Trim();

            var response = await CheckISCODPaymentReceived(craccount);
            var codWaybill = string.Empty;
            if (response != null && !string.IsNullOrEmpty(response.Status) && response.Status.Equals("00"))
            {
                var craccountName = response.Transactions.FirstOrDefault().Craccountname;
                codWaybill = craccountName.Split('_').FirstOrDefault();
            }
            else
            {
                result.Status = false;
                result.Message = response.StatusDesc;
                return result;
            }

            if (!string.IsNullOrEmpty(codWaybill))
            {
                var codAmount = _uow.Shipment.GetAllAsQueryable().Where(x => x.Waybill == codWaybill).Select(x => x.CashOnDeliveryAmount).FirstOrDefault();

                if (codAmount.HasValue)
                {
                    var transferedAmount = Convert.ToDecimal(response.Transactions.FirstOrDefault().Amount);
                    if (transferedAmount == codAmount.Value || transferedAmount > codAmount.Value)
                    {
                        result.Status = true;
                        result.Message = "Transfer Successfully Confirmed";
                    }
                    else if (transferedAmount < codAmount.Value)
                    {
                        result.Status = false;
                        result.Message = $"Amount of {transferedAmount} transferred is less than the expected COD amount of {codAmount.Value}. This item can not be released at this time";
                    }
                }
                else
                {
                    var codAmountFromMobile = _uow.PreShipmentMobile.GetAllAsQueryable().Where(x => x.Waybill == codWaybill).Select(x => x.CashOnDeliveryAmount).FirstOrDefault();
                    if (codAmountFromMobile.HasValue)
                    {
                        var transferedAmount = Convert.ToDecimal(response.Transactions.FirstOrDefault().Amount);
                        if (transferedAmount == codAmountFromMobile.Value || transferedAmount > codAmountFromMobile.Value)
                        {
                            result.Status = true;
                            result.Message = "Transfer Successfully Confirmed";
                        }
                        else if (transferedAmount < codAmountFromMobile.Value)
                        {
                            result.Status = false;
                            result.Message = $"Amount of {transferedAmount} transferred is less than the expected COD amount of {codAmountFromMobile.Value}. This item can not be released at this time";
                        }
                    }
                    else if (!codAmountFromMobile.HasValue && !codAmount.HasValue)
                    {
                        result.Status = false;
                        result.Message = $"Shipment with waybill {codWaybill} has no COD Value or cannot be found.";
                    }
                }
            }
            return result;
        }

        private async Task<CODPaymentStatusResponse> CheckISCODPaymentReceived(string craccount)
        {
            CODPaymentStatusResponse result = new CODPaymentStatusResponse();
            string content = string.Empty;
            string url = ConfigurationManager.AppSettings["CellulantTransferUrl"];
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                httpClient.Timeout = new TimeSpan(0, 0, 30);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var request = new HttpRequestMessage(HttpMethod.Post, $"/GenericWave/Proxy/Query?craccount={craccount}&type=2") { };
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
                //var status = JObject.Parse(content)["status"].ToString();
                var resultResponse = JsonConvert.DeserializeObject<CODPaymentStatusResponse>(content);

                if (!string.IsNullOrEmpty(resultResponse.Status))
                {
                    result = resultResponse;
                }
            }
            return result;
        }



        public async Task<GenerateAccountDTO> GenerateAccountNumberCellulant(GenerateAccountPayloadDTO payload)
        {
            string url = ConfigurationManager.AppSettings["CellulantAccGeneration"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var result = new GenerateAccountDTO();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(payload);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                string message = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var res = JsonConvert.DeserializeObject<GenerateAccountResponseDTO>(message);
                    if (res.status == 41)
                    {
                        var err = JsonConvert.DeserializeObject<GenerateAccountErrorDTO>(message);
                        result.Error = err;
                        result.Succeeded = false;
                        return result;
                    }
                    result.ResponsePayload = res;
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    var res = JsonConvert.DeserializeObject<GenerateAccountErrorDTO>(message);
                    result.Error = res;
                    result.Succeeded = false;
                    return result;
                }
            }
            return result;
        }

        #endregion

        #region Cellulant Validate Payment
        public async Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference)
        {
            FlutterWebhookDTO webhook = new FlutterWebhookDTO();

            var paymentStatus = await QueryCellulantPaymentStatus(new CellulantPaymentQueryStatusDto
            {
                MerchantTransactionID = reference
            });

            if(paymentStatus.Status == null)
            {
                webhook.Message = paymentStatus.Message;
                webhook.Status = paymentStatus.Message;
                webhook.data.Processor_Response = paymentStatus.Message;
                webhook.data.Status = paymentStatus.StatusCode.ToString();
            }

            if(paymentStatus.Status.StatusCode.Equals(200) && paymentStatus.Status.StatusDescription.Equals("Successfully processed request"))
            {
                WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(reference);

                var transaction = new CellulantWebhookDTO
                {
                    RequestStatusDescription = "request fully paid",
                    MerchantTransactionID = paymentStatus.Results.MerchantTransactionID,
                    CheckoutRequestID = paymentStatus.Results.CheckoutRequestID,
                    AmountPaid = paymentStatus.Results.AmountPaid,
                    RequestStatusCode = 178,
                    Payments = new List<Payment>()
                };

                if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
                { 
                   var result = await ProcessValidatePaymentForWaybill(transaction);
                    webhook.Status = "success";
                    webhook.Message = result.StatusDescription;
                    webhook.data.Processor_Response = result.StatusDescription;
                    webhook.data.Status = result.StatusCode.ToString();
                }
                else
                {
                    var result = await ProcessValidatePaymentForWallet(transaction);
                    webhook.Status = "success";
                    webhook.Message = result.StatusDescription;
                    webhook.data.Processor_Response = result.StatusDescription;
                    webhook.data.Status = result.StatusCode.ToString();
                }
            }
            else
            {
                webhook.Message = paymentStatus.Status.StatusDescription;
                webhook.Status = paymentStatus.Status.StatusCode.ToString();
                webhook.data.Processor_Response = paymentStatus.Status.StatusDescription;
                webhook.data.Status = paymentStatus.Status.StatusCode.ToString();
            }

            //Acknowledge Payment with cellulant

            if(paymentStatus.Results != null)
            {
                await AcknowledgeCellulantPayment(new CellulantPaymentAcknowledgeDto
                {
                    CheckoutRequestID = paymentStatus.Results.CheckoutRequestID,
                    MerchantTransactionID = paymentStatus.Results.MerchantTransactionID,
                    ReceiptNumber = paymentStatus.Results.MerchantTransactionID,
                });
            }

            return ManageReturnResponse(webhook);
        }

        private async Task<CellulantPaymentResponse> ProcessValidatePaymentForWaybill(CellulantWebhookDTO payload)
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

        private async Task<CellulantPaymentResponse> ProcessValidatePaymentForWallet(CellulantWebhookDTO payload)
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
                    _uow.BeginTransaction(IsolationLevel.Serializable);
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

                            //Convert amount base on country rate if isConverted 
                            //1. CHeck if is converted equals true
                            if (paymentLog.isConverted)
                            {
                                //2. Get user country id
                                var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);
                                if (user == null)
                                {
                                    return result;
                                }

                                if (user.UserActiveCountryId <= 0)
                                {
                                    return result;
                                }

                                var userdestCountry = new CountryRouteZoneMap();

                                // Get conversion rate base of card type use
                                if (paymentLog.CardType == CardType.Naira)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 1 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Pound)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 62 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Dollar)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 207 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 76 && c.CompanyMap == CompanyMap.GIG);
                                }

                                if (userdestCountry == null)
                                {
                                    return result;
                                }

                                if (userdestCountry.Rate <= 0)
                                {
                                    return result;
                                }
                                //3. Convert base on country rate
                                var convertedAmount = Math.Round((userdestCountry.Rate * (double)paymentLog.Amount), 2);
                                paymentLog.Amount = (decimal)convertedAmount;
                            }

                            //update the wallet
                            await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                            {
                                WalletId = paymentLog.WalletId,
                                Amount = paymentLog.Amount,
                                CreditDebitType = CreditDebitType.Credit,
                                // Description = verifyResult.RequestStatusDescription,
                                Description = "Funding made through debit card",
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
                    _uow.Commit();
                    //await _uow.CompleteAsync();

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

        private PaystackWebhookDTO ManageReturnResponse(FlutterWebhookDTO flutterResponse)
        {
            var response = new PaystackWebhookDTO
            {
                Message = flutterResponse.Message
            };

            if (flutterResponse.Status.Equals("success"))
            {
                response.Status = true;
            }

            if (flutterResponse.data != null)
            {
                response.data.Status = flutterResponse.data.Status;
                response.data.Message = flutterResponse.data.Processor_Response;
                response.data.Gateway_Response = flutterResponse.data.Processor_Response;

                //if (flutterResponse.data.validateInstructions.Instruction != null)
                //{
                //    response.data.Message = flutterResponse.data.validateInstructions.Instruction;
                //    response.data.Gateway_Response = flutterResponse.data.validateInstructions.Instruction;
                //}
                //else if (flutterResponse.data.ChargeMessage != null)
                //{
                //    response.data.Message = flutterResponse.data.ChargeMessage;
                //    response.data.Gateway_Response = flutterResponse.data.ChargeMessage;
                //    response.Message = flutterResponse.data.ChargeMessage;
                //    if (!flutterResponse.data.Status.Equals("successful"))
                //    {
                //        response.Status = false;
                //    }
                //}
                //else
                //{
                //    response.data.Message = flutterResponse.data.ChargeResponseMessage;
                //    response.data.Gateway_Response = flutterResponse.data.ChargeResponseMessage;
                //}

                //if(flutterResponse.data.ChargeCode != null)
                //{
                //    if (flutterResponse.data.Status.Equals("successful") && flutterResponse.data.ChargeCode.Equals("00"))
                //    {
                //        response.data.Message = flutterResponse.data.ChargeMessage;
                //        response.data.Gateway_Response = flutterResponse.data.ChargeMessage;
                //        response.Message = flutterResponse.data.ChargeMessage;
                //        response.Status = true;
                //    }
                //}
            }
            else
            {
                response.data.Message = flutterResponse.Message;
                response.data.Gateway_Response = flutterResponse.Message;
                response.data.Status = flutterResponse.Status;
            }

            return response;
        }

        private async Task<string> GetTokenToValidatePayment()
        {
            using (var client = new HttpClient())
            {
                var result = "";
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["CellulantPaymentBaseUrl"];
                var login = ConfigurationManager.AppSettings["CellulantPaymentAuth"];
                baseurl = $"{baseurl}{login}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new CellulantPaymentLoginDto
                {
                    GrantType = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CellulantPaymentGrantType"]) ? " " : ConfigurationManager.AppSettings["CellulantPaymentGrantType"],
                    ClientId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CellulantPaymentClientID"]) ? " " : ConfigurationManager.AppSettings["CellulantPaymentClientID"],
                    ClientSecret = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CellulantPaymentClientSecret"]) ? " " : ConfigurationManager.AppSettings["CellulantPaymentClientSecret"],
                };

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<CellulantPaymentLoginResponseDto>(resultJson);
                result = jObject.AccessToken;

                return result;
            }
        }

        private async Task<CellulantPaymentQueryStatusResponseDto> QueryCellulantPaymentStatus(CellulantPaymentQueryStatusDto payload)
        {
            CellulantPaymentQueryStatusResponseDto result = new CellulantPaymentQueryStatusResponseDto();
            string token = await GetTokenToValidatePayment();
            using (var client = new HttpClient())
            {
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["CellulantPaymentBaseUrl"];
                var queryStatus = ConfigurationManager.AppSettings["CellulantPaymentQueryStatus"];
                baseurl = $"{ baseurl}{queryStatus}";
                payload.ServiceCode = ConfigurationManager.AppSettings["CellulantPaymentServiceCode"];
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<CellulantPaymentQueryStatusResponseDto>(resultJson);

                return result;
            }
        }

        private async Task<CellulantPaymentAcknowledgeResponseDto> AcknowledgeCellulantPayment(CellulantPaymentAcknowledgeDto payload)
        {
            CellulantPaymentAcknowledgeResponseDto result = new CellulantPaymentAcknowledgeResponseDto();
            string token = await GetTokenToValidatePayment();
            using (var client = new HttpClient())
            {
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["CellulantPaymentBaseUrl"];
                var acknowledge = ConfigurationManager.AppSettings["CellulantPaymentAcknowledge"];
                baseurl = $"{baseurl}{acknowledge}";

                //Set default property values
                payload.StatusCode = Convert.ToInt32(ConfigurationManager.AppSettings["CellulantPaymentStatusCode"]);
                payload.StatusDescription = ConfigurationManager.AppSettings["CellulantPaymentStatusDescription"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<CellulantPaymentAcknowledgeResponseDto>(resultJson);

                return result;
            }
        }
        #endregion
    }
}
