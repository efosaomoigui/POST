using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using GIGLS.Core.DTO.Report;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Domain.Wallet;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerTransactionsService : IPartnerTransactionsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IPartnerPayoutService _partnerPayoutService;

        public PartnerTransactionsService(IUnitOfWork uow, IUserService userService, IWalletService walletService,
                                        IWalletTransactionService walletTransactionService, IPartnerPayoutService partnerPayoutService)
        {
            _userService = userService;
            _uow = uow;
            _walletService = walletService;
            _walletTransactionService = walletTransactionService;
            _partnerPayoutService = partnerPayoutService;
            MapperConfig.Initialize();
        }

        public async Task<RootObject> GetGeoDetails(LocationDTO location)
        {
            var Response = new RootObject();
            try
            {
                var GoogleURL = ConfigurationManager.AppSettings["DistanceURL"];
                var GoogleApiKey = ConfigurationManager.AppSettings["DistanceApiKey"];
                GoogleApiKey = await Decrypt(GoogleApiKey);
                var finalURL = $"{GoogleURL}{GoogleApiKey}&units=metric&origins={location.OriginLatitude},{location.OriginLongitude}&destinations={location.DestinationLatitude},{location.DestinationLongitude}";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalURL);
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream result = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(result);
                    string responseFromServer = reader.ReadToEnd();
                    Response = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                }
            }
            catch (Exception ex)
            {
                // An exception occurred making the REST call
                throw ex;
            }

            return await Task.FromResult(Response);
        }

        public async Task<decimal> GetPriceForPartner (PartnerPayDTO partnerpay)
        {
            var TotalPrice = 0.0M;
            if (partnerpay.ZoneMapping == 1)
            {
                var TotalAmount = (partnerpay.ShipmentPrice);
                var amount = (0.8M * TotalAmount);
                TotalPrice = Convert.ToDecimal(string.Format("{0:F2}", amount));
            }
            else
            {
                //var distance = Convert.ToDecimal(partnerpay.Distance);
                //var actualdistance = distance / 1000;
                //var TotalAmountBasedonDistance = actualdistance * 3;
                //var Time = Convert.ToDecimal(partnerpay.Time);
                //var actualTimeinMinutes = Convert.ToDecimal(string.Format("{0:F2}", (Time / 60)));
                //var TotalAmountBasedonTime = actualTimeinMinutes * 2;
                //var TotalAmountBasedonShipment = partnerpay.ShipmentPrice * 0.05M;
                //var Totalprice = TotalAmountBasedonDistance + TotalAmountBasedonTime + TotalAmountBasedonShipment;
                //Totalprice = Convert.ToDecimal(string.Format("{0:F2}", Totalprice));
                //var Sumofpickupandgooglapicalc =  (Totalprice + partnerpay.ShipmentPrice);
                //var pickupprice = partnerpay.PickUprice;
                var TotalAmount = (partnerpay.PickUprice);
                var amount = (0.8M * TotalAmount);
                TotalPrice = Convert.ToDecimal(string.Format("{0:F2}", amount));
            }
            return await Task.FromResult(TotalPrice);
        }

        public async Task<object> AddPartnerPaymentLog(PartnerTransactionsDTO walletPaymentLogDto)
        {
            walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();
            var walletPaymentLog = Mapper.Map<PartnerTransactions>(walletPaymentLogDto);
            _uow.PartnerTransactions.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.PartnerTransactionsID };
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public async Task<string> Decrypt(string cipherText)
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

        public async Task ProcessPartnerTransactions(List<ExternalPartnerTransactionsPaymentDTO> paymentLogDto)
        {
            var gigGOServiceCenter = await _userService.GetGIGGOServiceCentre();

            //Get current User
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _userService.GetUserById(currentUserId);
            var processedBy = user.Email;

            List<ExternalPartnerTransactionsPaymentDTO> newPaymentLogDto = new List<ExternalPartnerTransactionsPaymentDTO>();

            foreach (var partner in paymentLogDto)
            {
                var existingParntner = await _uow.Partner.GetAsync(s => s.PartnerCode == partner.Code);

                if (existingParntner != null)
                {
                    // Get Date Info
                    var queryDate = partner.filterCriteria.getStartDateAndEndDate();
                    var startDate = queryDate.Item1;
                    var endDate = queryDate.Item2;

                    partner.filterCriteria.StartDate = startDate;
                    partner.filterCriteria.EndDate = endDate;

                    partner.GIGGOServiceCenter = gigGOServiceCenter.ServiceCentreId;
                    partner.ProcessedBy = processedBy;
                    partner.UserId = existingParntner.UserId;

                    newPaymentLogDto.Add(partner);
                }
            }

            foreach (var partner in newPaymentLogDto)
            {
                await UpdatePartnerTransactionPayment(partner);
            }
        }

        private async Task UpdatePartnerTransactionPayment(ExternalPartnerTransactionsPaymentDTO partner)
        {
            //var transactions = await _uow.PartnerTransactions.FindAsync(x => x.UserId == partner.UserId 
            //                                && x.DateCreated >= partner.filterCriteria.StartDate 
            //                                && x.DateCreated <= partner.filterCriteria.EndDate);

            //if(transactions != null)
            //{
            //    foreach (var transaction in transactions)
            //    {
            //        transaction.IsProcessed = true;
            //    }
            //}

            await AddPartnerWallet(partner);
        }

        private async Task AddPartnerWallet(ExternalPartnerTransactionsPaymentDTO partner)
        {
            var wallet = await _walletService.GetWalletBalance(partner.Code);
            var amountLeft = (wallet.Balance - Convert.ToDecimal(partner.Amount));
            var addtransaction = new WalletTransactionDTO
            {
                WalletId = wallet.WalletId,
                CreditDebitType = CreditDebitType.Debit,
                Amount = partner.Amount,
                ServiceCentreId = partner.GIGGOServiceCenter,
                //Waybill = waybill,
                Description = "Debit for shipment delivery payout",
                PaymentType = PaymentType.Transfer,
                UserId = partner.UserId
            };
            var walletTransaction = await _walletTransactionService.AddWalletTransaction(addtransaction);
            var updatedwallet = await _uow.Wallet.GetAsync(wallet.WalletId);
            updatedwallet.Balance = amountLeft;

            await AddPartnerPayoutData(partner);

        }

        private async Task AddPartnerPayoutData(ExternalPartnerTransactionsPaymentDTO partner)
        {
            var processedByData = new PartnerPayoutDTO
            {
                Amount = partner.Amount,
                ProcessedBy = partner.ProcessedBy,
                PartnerName = partner.PartnerName,
                StartDate = partner.filterCriteria.StartDate,
                EndDate = partner.filterCriteria.EndDate
            };
            var addPartnerPayout = await _partnerPayoutService.AddPartnerPayout(processedByData);
        }

        public async Task<List<PartnerPayoutDTO>> GetPartnersPayout(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var partnersPayout = await _uow.PartnerPayout.GetPartnerPayoutByDate(filterCriteria);

            return partnersPayout;
        }

        public async Task CreditPartnerTransactionByAdmin(CreditPartnerTransactionsDTO transactionsDTO)
        {
            try
            {
                var preshipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == transactionsDTO.Waybill);
                if (preshipment == null)
                {
                    throw new GenericException($"This waybill {transactionsDTO.Waybill} does not exist");
                }

                var partner = await _userService.GetUserByEmail(transactionsDTO.Email);
                if (partner == null)
                {
                    throw new GenericException($"This partner with Email {transactionsDTO.Email} does not exist");
                }

                var partnerTrans = await _uow.PartnerTransactions.GetAsync(x => x.Waybill == transactionsDTO.Waybill);
                if(partnerTrans != null)
                {
                    throw new GenericException($"This transaction already exists");
                }

                var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();

                //Add to Partner Transactions
                var partnerTransactions = new PartnerTransactionsDTO
                {
                    Destination = preshipment.ReceiverAddress,
                    Departure = preshipment.SenderAddress,
                    AmountReceived = transactionsDTO.AmountReceived,
                    Waybill = preshipment.Waybill,
                    UserId = partner.Id,
                };
                var partnerPayment = Mapper.Map<PartnerTransactions>(partnerTransactions);
                _uow.PartnerTransactions.Add(partnerPayment);

                //Add to Wallet 
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.UserChannelCode);
                wallet.Balance = wallet.Balance + transactionsDTO.AmountReceived;

                //Add to Wallet Transactions
                var walletTransaction = new WalletTransactionDTO
                {
                    WalletId = wallet.WalletId,
                    CreditDebitType = CreditDebitType.Credit,
                    Amount = transactionsDTO.AmountReceived,
                    ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                    Waybill = preshipment.Waybill,
                    Description = "Credit for Delivered Shipment Request",
                    PaymentType = PaymentType.Online,
                    UserId = partner.Id,
                    DateOfEntry = DateTime.Now
                };
                var newWalletTransaction = Mapper.Map<WalletTransaction>(walletTransaction);
                _uow.WalletTransaction.Add(newWalletTransaction);

                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}
