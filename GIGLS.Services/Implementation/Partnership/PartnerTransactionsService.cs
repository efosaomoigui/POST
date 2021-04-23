using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerTransactionsService : IPartnerTransactionsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IPartnerPayoutService _partnerPayoutService;
        private readonly IMobileShipmentTrackingService _mobileShipmentTrackingService;

        public PartnerTransactionsService(IUnitOfWork uow, IUserService userService, IWalletService walletService,
                                        IWalletTransactionService walletTransactionService, IPartnerPayoutService partnerPayoutService,
                                        IMobileShipmentTrackingService mobileShipmentTrackingService)
        {
            _userService = userService;
            _uow = uow;
            _walletService = walletService;
            _walletTransactionService = walletTransactionService;
            _partnerPayoutService = partnerPayoutService;
            _mobileShipmentTrackingService = mobileShipmentTrackingService;
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

                    //check if request was fufilled
                    if (Response.status.ToLower() == "request_denied")
                    {
                        throw new GenericException($"Geo-Location service unavailable.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(Response);
        }

        public async Task<decimal> GetPriceForPartner(PartnerPayDTO partnerpay)
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

        public async Task<string> Encrypt(string clearText)
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
            await _partnerPayoutService.AddPartnerPayout(processedByData);
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

                var partnerTrans = await _uow.PartnerTransactions.GetAsync(x => x.Waybill == transactionsDTO.Waybill);
                if (partnerTrans != null)
                {
                    throw new GenericException($"This transaction already exists");
                }

                var partner = await _uow.Partner.GetAsync(x => x.Email == transactionsDTO.Email);
                if (partner == null)
                {
                    throw new GenericException($"This partner with Email {transactionsDTO.Email} does not exist");
                }

                var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();

                //Add to Partner Transactions
                var partnerTransactions = new PartnerTransactionsDTO
                {
                    Destination = preshipment.ReceiverAddress,
                    Departure = preshipment.SenderAddress,
                    AmountReceived = transactionsDTO.AmountReceived,
                    Waybill = preshipment.Waybill,
                    UserId = partner.UserId,
                };
                var partnerPayment = Mapper.Map<PartnerTransactions>(partnerTransactions);
                _uow.PartnerTransactions.Add(partnerPayment);

                //Add to Wallet 
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
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
                    UserId = partner.UserId,
                    DateOfEntry = DateTime.Now
                };
                var newWalletTransaction = Mapper.Map<WalletTransaction>(walletTransaction);
                _uow.WalletTransaction.Add(newWalletTransaction);

                //Update Status if not already updated
                string delivered = MobilePickUpRequestStatus.Delivered.ToString();
                string onwardProcessing = MobilePickUpRequestStatus.OnwardProcessing.ToString();

                if (preshipment.shipmentstatus != delivered && preshipment.shipmentstatus != onwardProcessing)
                {
                    preshipment.IsDelivered = true;
                    ShipmentScanStatus status = ShipmentScanStatus.MCRT;

                    if (preshipment.ZoneMapping == 1)
                    {
                        preshipment.shipmentstatus = delivered;
                        status = ShipmentScanStatus.MAHD;
                    }
                    else
                    {
                        preshipment.shipmentstatus = onwardProcessing;
                        status = ShipmentScanStatus.MSVC;
                    }

                    await ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = preshipment.Waybill,
                        ShipmentScanStatus = status
                    });
                }
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ScanMobileShipment(ScanDTO scan)
        {
            try
            {
                string scanStatus = scan.ShipmentScanStatus.ToString();
                await _mobileShipmentTrackingService.AddMobileShipmentTracking(new MobileShipmentTrackingDTO
                {
                    DateTime = DateTime.Now,
                    Status = scanStatus,
                    Waybill = scan.WaybillNumber,
                }, scan.ShipmentScanStatus);

            }
            catch (Exception)
            {
                throw new GenericException("Please an error occurred while trying to scan shipment.");
            }
        }

        public async Task CreditCaptainForMovementManifestTransaction(CreditPartnerTransactionsDTO transactionsDTO)
        {
            try
            {
                var manifest = await _uow.MovementManifestNumber.GetAsync(x => x.MovementManifestCode == transactionsDTO.Manifest);
                if (manifest == null)
                {
                    throw new GenericException($"This manifest {transactionsDTO.Manifest} does not exist");
                }

                var partnerTrans = await _uow.PartnerTransactions.GetAsync(x => x.Manifest == transactionsDTO.Manifest);
                if (partnerTrans != null)
                {
                    throw new GenericException($"This transaction already exists");
                }

                var userId = await _userService.GetCurrentUserId();

                var partner = await _uow.Partner.GetAsync(x => x.UserId == userId);
                if (partner == null)
                {
                    throw new GenericException($"This partner with Email {transactionsDTO.Email} does not exist");
                }

                var departure = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == manifest.DepartureServiceCentreId);
                var destination = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == manifest.DestinationServiceCentreId);

                var defaultServiceCenter = await _userService.GetGIGGOServiceCentre();

                //Add to Partner Transactions
                var partnerTransactions = new PartnerTransactionsDTO
                {
                    Destination = destination.Name,
                    Departure = departure.Name,
                    AmountReceived = transactionsDTO.AmountReceived,
                    Manifest = manifest.MovementManifestCode,
                    UserId = partner.UserId,
                };
                var partnerPayment = Mapper.Map<PartnerTransactions>(partnerTransactions);
                _uow.PartnerTransactions.Add(partnerPayment);

                //Add to Wallet 
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                wallet.Balance = wallet.Balance + transactionsDTO.AmountReceived;

                //Add to Wallet Transactions
                var walletTransaction = new WalletTransactionDTO
                {
                    WalletId = wallet.WalletId,
                    CreditDebitType = CreditDebitType.Credit,
                    Amount = transactionsDTO.AmountReceived,
                    ServiceCentreId = defaultServiceCenter.ServiceCentreId,
                    Manifest = manifest.MovementManifestCode,
                    Description = "Bonus Credit for Timely Delivery",
                    PaymentType = PaymentType.Online,
                    UserId = partner.UserId,
                    DateOfEntry = DateTime.Now
                };
                var newWalletTransaction = Mapper.Map<WalletTransaction>(walletTransaction);
                _uow.WalletTransaction.Add(newWalletTransaction);

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}