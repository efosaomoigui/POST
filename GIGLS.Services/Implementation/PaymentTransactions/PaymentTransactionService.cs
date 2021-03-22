using GIGLS.Core.IServices.PaymentTransactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IServices.Utility;
using GIGL.GIGLS.Core.Domain;
using System.Linq;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IMessageService;
using System.Text;
using System.Security.Cryptography;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using System.Net;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.Services.Implementation.PaymentTransactions
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ICountryRouteZoneMapService _countryRouteZoneMapService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IFinancialReportService _financialReportService;

        public PaymentTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService,
            IGlobalPropertyService globalPropertyService, ICountryRouteZoneMapService countryRouteZoneMapService,
            IMessageSenderService messageSenderService, IFinancialReportService financialReportService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            _globalPropertyService = globalPropertyService;
            _countryRouteZoneMapService = countryRouteZoneMapService;
            _messageSenderService = messageSenderService;
            _financialReportService = financialReportService;
            MapperConfig.Initialize();
        }

        //used for transaction, hence private
        private async Task<object> AddPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new GenericException("Null Input");

            var transactionExist = await _uow.PaymentTransaction.ExistAsync(x => x.Waybill.Equals(paymentTransaction.Waybill));

            if (transactionExist == true)
                throw new GenericException($"Payment Transaction for {paymentTransaction.Waybill} already exist");

            var payment = Mapper.Map<PaymentTransaction>(paymentTransaction);
            _uow.PaymentTransaction.Add(payment);
            //await _uow.CompleteAsync();
            return new { Id = payment.PaymentTransactionId };
        }

        public async Task<PaymentTransactionDTO> GetPaymentTransactionById(string waybill)
        {
            var transaction = await _uow.PaymentTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            return Mapper.Map<PaymentTransactionDTO>(transaction);
        }

        public Task<IEnumerable<PaymentTransactionDTO>> GetPaymentTransactions()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<PaymentTransactionDTO>>(_uow.PaymentTransaction.GetAll()));
        }

        public async Task RemovePaymentTransaction(string waybill)
        {
            var transaction = await _uow.PaymentTransaction.GetAsync(x => x.Waybill.Equals(waybill));

            if (transaction == null)
            {
                throw new GenericException("Payment Transaction does not exist");
            }
            _uow.PaymentTransaction.Remove(transaction);
            await _uow.CompleteAsync();
        }

        public async Task UpdatePaymentTransaction(string waybill, PaymentTransactionDTO paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new GenericException("Null Input");

            var payment = await _uow.PaymentTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            if (payment == null)
                throw new GenericException($"No Payment Transaction exist for {waybill} waybill");

            payment.TransactionCode = paymentTransaction.TransactionCode;
            payment.PaymentStatus = paymentTransaction.PaymentStatus;
            payment.PaymentTypes = paymentTransaction.PaymentType;
            await _uow.CompleteAsync();
        }

        public async Task<bool> ProcessPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            var result = false;
            var returnWaybill = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew == paymentTransaction.Waybill);

            if (returnWaybill != null)
            {
                result = await ProcessReturnPaymentTransaction(paymentTransaction);
            }
            else
            {
                result = await ProcessNewPaymentTransaction(paymentTransaction);
            }

            return result;
        }

        public async Task<bool> ProcessNewPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            var result = false;

            if (paymentTransaction == null)
                throw new GenericException("Null Input");

            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();
            paymentTransaction.UserId = currentUserId;

            //get Ledger, Invoice, shipment
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);

            //all account customers payment should be settle by wallet automatically
            //settlement by wallet
            if (paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                paymentTransaction.TransactionCode = shipment.CustomerCode;
                await ProcessWalletTransaction(paymentTransaction, shipment, invoiceEntity, generalLedgerEntity, currentUserId);
            }

            // create payment
            paymentTransaction.PaymentStatus = PaymentStatus.Paid;
            var paymentTransactionId = await AddPaymentTransaction(paymentTransaction);

            // update GeneralLedger
            generalLedgerEntity.IsDeferred = false;
            generalLedgerEntity.PaymentType = paymentTransaction.PaymentType;
            generalLedgerEntity.PaymentTypeReference = paymentTransaction.TransactionCode;

            //update invoice
            invoiceEntity.PaymentDate = DateTime.Now;
            invoiceEntity.PaymentMethod = paymentTransaction.PaymentType.ToString();
            await BreakdownPayments(invoiceEntity, paymentTransaction);

            invoiceEntity.PaymentStatus = paymentTransaction.PaymentStatus;
            invoiceEntity.PaymentTypeReference = paymentTransaction.TransactionCode;
            await _uow.CompleteAsync();

            //QR Code
            var deliveryNumber = await _uow.DeliveryNumber.GetAsync(s => s.Waybill == shipment.Waybill);

            //send sms to the customer
            var smsData = new ShipmentTrackingDTO
            {
                Waybill = shipment.Waybill,
                QRCode = deliveryNumber.SenderCode
            };

            if(shipment.DepartureCountryId == 1)
            {
                //Add to Financial Reports
                var financialReport = new FinancialReportDTO
                {
                    Source = ReportSource.Agility,
                    Waybill = shipment.Waybill,
                    PartnerEarnings = 0.0M,
                    GrandTotal = invoiceEntity.Amount,
                    Earnings = invoiceEntity.Amount,
                    Demurrage = 0.00M,
                    CountryId = invoiceEntity.CountryId
                };
                await _financialReportService.AddReport(financialReport);
            }
            else
            {
                var countryRateConversion = await _countryRouteZoneMapService.GetZone(shipment.DestinationCountryId, shipment.DepartureCountryId);

                double amountToDebitDouble = (double)invoiceEntity.Amount * countryRateConversion.Rate;

                var amountToDebit = (decimal)Math.Round(amountToDebitDouble, 2);

                //Add to Financial Reports
                var financialReport = new FinancialReportDTO
                {
                    Source = ReportSource.Intl,
                    Waybill = shipment.Waybill,
                    PartnerEarnings = 0.0M,
                    GrandTotal = amountToDebit,
                    Earnings = amountToDebit,
                    Demurrage = 0.00M,
                    ConversionRate = countryRateConversion.Rate,
                    CountryId = shipment.DestinationCountryId
                };
                await _financialReportService.AddReport(financialReport);
            }

            if (shipment.DepartureServiceCentreId == 309)
            {
                await _messageSenderService.SendMessage(MessageType.HOUSTON, EmailSmsType.SMS, smsData);
                await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.Email, smsData);
            }
            else
            {
                await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.All, smsData);
            }

            //Send Email to Sender when Payment for International Shipment has being made
            if(invoiceEntity.IsInternational == true && paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                var shipmentDTO = new ShipmentDTO
                {
                    CustomerType = shipment.CustomerType,
                    CustomerId = shipment.CustomerId,
                    ReceiverName = shipment.ReceiverName,
                    Waybill = shipment.Waybill,
                    PickupOptions = shipment.PickupOptions,
                    ReceiverEmail = shipment.ReceiverEmail,
                    GrandTotal = shipment.GrandTotal,
                    DepartureCountryId = shipment.DepartureCountryId,
                    DestinationCountryId = shipment.DestinationCountryId,
                    DestinationServiceCentreId = shipment.DestinationServiceCentreId,
                    DepartureServiceCentreId = shipment.DepartureServiceCentreId,
                    CustomerDetails = new CustomerDTO { },
                    DepartureServiceCentre = new ServiceCentreDTO { },
                    DestinationServiceCentre = new ServiceCentreDTO { }
                };

                if (shipmentDTO.CustomerType.Contains("Individual"))
                {
                    shipmentDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
                CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDTO.CustomerType);

                var customerObj = await _messageSenderService.GetCustomer(shipmentDTO.CustomerId, customerType);
                shipmentDTO.CustomerDetails.Email = customerObj.Email;
                shipmentDTO.CustomerDetails.PhoneNumber = customerObj.PhoneNumber;

                await _messageSenderService.SendGenericEmailMessage(MessageType.IPC, shipmentDTO);

                //Send email to Chinalu and Peter
                var mails = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.IntlShipmentPaymentMonitoringEmails.ToString() && s.CountryId == 1);

                if (mails != null)
                {
                    //seperate email by comma and send message to those email
                    string[] paymentEmails = mails.Value.Split(',').ToArray();

                    foreach (string email in paymentEmails)
                    {
                        // send email message for payment notification
                        shipmentDTO.CustomerDetails.Email = email;
                        await _messageSenderService.SendGenericEmailMessage(MessageType.IPC, shipmentDTO);
                    }
                }
            }

            result = true;
            return result;
        }

        private async Task ProcessWalletTransaction(PaymentTransactionDTO paymentTransaction, Shipment shipment, Invoice invoiceEntity, GeneralLedger generalLedgerEntity, string currentUserId)
        {
            //I used transaction code to represent wallet number when processing for wallet
            var wallet = await _walletService.GetWalletById(paymentTransaction.TransactionCode);

            decimal amountToDebit = invoiceEntity.Amount;

            amountToDebit = await GetActualAmountToDebit(shipment, amountToDebit);

            //Additions for Ecommerce customers (Max wallet negative payment limit)
            //var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);
            if (shipment != null && CompanyType.Ecommerce.ToString() == shipment.CompanyType && !paymentTransaction.FromApp)
            {
                //Gets the customer wallet limit for ecommerce
                decimal ecommerceNegativeWalletLimit = await GetEcommerceWalletLimit(shipment);

                //deduct the price for the wallet and update wallet transaction table
                if (wallet.Balance - amountToDebit < (Math.Abs(ecommerceNegativeWalletLimit) * (-1)))
                {
                    throw new GenericException("Ecommerce Customer. Insufficient Balance in the Wallet");
                }
            }

            //for other customers
            //deduct the price for the wallet and update wallet transaction table
            //--Update April 25, 2019: Corporate customers should be debited from wallet
            if (shipment != null && CompanyType.Client.ToString() == shipment.CompanyType)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Insufficient Balance in the Wallet");
                }
            }

            if (shipment != null && paymentTransaction.FromApp == true)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Insufficient Balance in the Wallet");
                }
            }

            int[] serviceCenterIds = { };

            if (!paymentTransaction.FromApp)
            {
                serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            }
            else
            {
                var gigGOServiceCenter = await _userService.GetGIGGOServiceCentre();
                serviceCenterIds = new int[] { gigGOServiceCenter.ServiceCentreId };
            }

            var newWalletTransaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                Amount = amountToDebit,
                DateOfEntry = DateTime.Now,
                ServiceCentreId = serviceCenterIds[0],
                UserId = currentUserId,
                CreditDebitType = CreditDebitType.Debit,
                PaymentType = PaymentType.Wallet,
                Waybill = paymentTransaction.Waybill,
                Description = generalLedgerEntity.Description
            };
            //get the balance after transaction
            if (newWalletTransaction.CreditDebitType == CreditDebitType.Credit)
            {
                newWalletTransaction.BalanceAfterTransaction = wallet.Balance + newWalletTransaction.Amount;
            }
            else
            {
                newWalletTransaction.BalanceAfterTransaction = wallet.Balance - newWalletTransaction.Amount;
            }
            wallet.Balance = wallet.Balance - amountToDebit;

            _uow.WalletTransaction.Add(newWalletTransaction);
        }


        private async Task BreakdownPayments(Invoice invoiceEntity, PaymentTransactionDTO paymentTransaction)
        {
            if (paymentTransaction.PaymentType == PaymentType.Cash)
            {
                invoiceEntity.Cash = invoiceEntity.Amount;
            }
            else if (paymentTransaction.PaymentType == PaymentType.Transfer)
            {
                invoiceEntity.Transfer = invoiceEntity.Amount;
            }
            else if (paymentTransaction.PaymentType == PaymentType.Pos)
            {
                invoiceEntity.Pos = invoiceEntity.Amount;
            }
        }

        public async Task<bool> ProcessReturnPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            var result = false;

            if (paymentTransaction == null)
                throw new GenericException("Null Input");

            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();

            //get Ledger, Invoice, shipment
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);

            //all account customers payment should be settle by wallet automatically
            //settlement by wallet
            if (shipment.CustomerType == CustomerType.Company.ToString() || paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                //I used transaction code to represent wallet number when processing for wallet
                if (string.IsNullOrWhiteSpace(paymentTransaction.TransactionCode))
                {
                    paymentTransaction.TransactionCode = shipment.CustomerCode;
                }
                var wallet = await _walletService.GetWalletById(paymentTransaction.TransactionCode);

                //Additions for Ecommerce customers (Max wallet negative payment limit)
                //var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);
                //if (shipment != null && CompanyType.Ecommerce.ToString() == shipment.CompanyType)
                //{
                //    //Gets the customer wallet limit for ecommerce
                //    decimal ecommerceNegativeWalletLimit = await GetEcommerceWalletLimit(shipment);

                //    //deduct the price for the wallet and update wallet transaction table
                //    if (wallet.Balance - invoiceEntity.Amount < (System.Math.Abs(ecommerceNegativeWalletLimit) * (-1)))
                //    {
                //        throw new GenericException("Ecommerce Customer. Insufficient Balance in the Wallet");
                //    }
                //}


                decimal amountToDebit = invoiceEntity.Amount;

                amountToDebit = await GetActualAmountToDebit(shipment, amountToDebit);

                //for other customers
                //deduct the price for the wallet and update wallet transaction table
                if (shipment != null && CompanyType.Ecommerce.ToString() != shipment.CompanyType)
                {
                    if (wallet.Balance < amountToDebit)
                    {
                        throw new GenericException("Insufficient Balance in the Wallet");
                    }
                }

                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

                var newWalletTransaction = new WalletTransaction
                {
                    WalletId = wallet.WalletId,
                    Amount = amountToDebit,
                    DateOfEntry = DateTime.Now,
                    ServiceCentreId = serviceCenterIds[0],
                    UserId = currentUserId,
                    CreditDebitType = CreditDebitType.Debit,
                    PaymentType = PaymentType.Wallet,
                    Waybill = paymentTransaction.Waybill,
                    Description = generalLedgerEntity.Description
                };

                if (newWalletTransaction.CreditDebitType == CreditDebitType.Credit)
                {
                    newWalletTransaction.BalanceAfterTransaction = wallet.Balance + newWalletTransaction.Amount;
                }
                else
                {
                    newWalletTransaction.BalanceAfterTransaction = wallet.Balance - newWalletTransaction.Amount;
                }

                wallet.Balance = wallet.Balance - amountToDebit;

                _uow.WalletTransaction.Add(newWalletTransaction);
            }

            // create payment
            paymentTransaction.UserId = currentUserId;
            paymentTransaction.PaymentStatus = PaymentStatus.Paid;
            var paymentTransactionId = await AddPaymentTransaction(paymentTransaction);

            // update GeneralLedger
            generalLedgerEntity.IsDeferred = false;
            generalLedgerEntity.PaymentType = paymentTransaction.PaymentType;
            generalLedgerEntity.PaymentTypeReference = paymentTransaction.TransactionCode;

            //update invoice
            invoiceEntity.PaymentDate = DateTime.Now;
            invoiceEntity.PaymentMethod = paymentTransaction.PaymentType.ToString();
            await BreakdownPayments(invoiceEntity, paymentTransaction);
            invoiceEntity.PaymentStatus = paymentTransaction.PaymentStatus;
            invoiceEntity.PaymentTypeReference = paymentTransaction.TransactionCode;
            await _uow.CompleteAsync();

            //Add to Financial Reports
            var financialReport = new FinancialReportDTO
            {
                Source = ReportSource.Agility,
                Waybill = shipment.Waybill,
                PartnerEarnings = 0.0M,
                GrandTotal = invoiceEntity.Amount,
                Earnings = invoiceEntity.Amount,
                Demurrage = 0.00M,
                CountryId = invoiceEntity.CountryId
            };
            await _financialReportService.AddReport(financialReport);

            //QR Code
            var deliveryNumber = await _uow.DeliveryNumber.GetAsync(s => s.Waybill == shipment.Waybill);

            //send sms to the customer
            var smsData = new Core.DTO.Shipments.ShipmentTrackingDTO
            {
                Waybill = shipment.Waybill,
                QRCode = deliveryNumber.SenderCode
            };

            if (shipment.DepartureServiceCentreId == 309)
            {
                await _messageSenderService.SendMessage(MessageType.HOUSTON, EmailSmsType.SMS, smsData);
                await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.Email, smsData);
            }
            else
            {
                await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.All, smsData);
            }

            result = true;
            return result;

        }

        private async Task<decimal> GetEcommerceWalletLimit(Shipment shipment)
        {
            decimal ecommerceNegativeWalletLimit = 0;

            //Get the Customer Wallet Limit Category
            var companyObj = await _uow.Company.GetAsync(x => x.CustomerCode.ToLower() == shipment.CustomerCode.ToLower());
            var customerWalletLimitCategory = companyObj.CustomerCategory;

            var userActiveCountryId = await _userService.GetUserActiveCountryId();

            switch (customerWalletLimitCategory)
            {
                case CustomerCategory.Gold:
                    {
                        //get max negati ve wallet limit from GlobalProperty
                        var ecommerceNegativeWalletLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceGoldNegativeWalletLimit, userActiveCountryId);
                        ecommerceNegativeWalletLimit = decimal.Parse(ecommerceNegativeWalletLimitObj.Value);
                        break;
                    }
                case CustomerCategory.Premium:
                    {
                        //get max negati ve wallet limit from GlobalProperty
                        var ecommerceNegativeWalletLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommercePremiumNegativeWalletLimit, userActiveCountryId);
                        ecommerceNegativeWalletLimit = decimal.Parse(ecommerceNegativeWalletLimitObj.Value);
                        break;
                    }
                case CustomerCategory.Normal:
                    {
                        //get max negati ve wallet limit from GlobalProperty
                        var ecommerceNegativeWalletLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceNegativeWalletLimit, userActiveCountryId);
                        ecommerceNegativeWalletLimit = decimal.Parse(ecommerceNegativeWalletLimitObj.Value);
                        break;
                    }
                default:
                    break;
            }

            return ecommerceNegativeWalletLimit;
        }

        private async Task<decimal> GetActualAmountToDebit(Shipment shipment, decimal amountToDebit)
        {
            //1. Get Customer Country detail
            int customerCountryId = 0;
            Rank rank = Rank.Basic;

            if (UserChannelType.Ecommerce.ToString() == shipment.CompanyType || UserChannelType.Corporate.ToString() == shipment.CompanyType)
            {
                //customerCountryId = _uow.Company.GetAllAsQueryable()
                //    .Where(x => x.CustomerCode.ToLower() == shipment.CustomerCode.ToLower()).Select(x => x.UserActiveCountryId).FirstOrDefault();

                var customer = _uow.Company.GetAllAsQueryable().Where(x => x.CustomerCode.ToLower() == shipment.CustomerCode.ToLower()).FirstOrDefault();
                if(customer != null)
                {
                    customerCountryId = customer.UserActiveCountryId;
                    rank = customer.Rank;
                }
            }
            else
            {
                customerCountryId = _uow.IndividualCustomer.GetAllAsQueryable().Where(x => x.CustomerCode.ToLower() == shipment.CustomerCode.ToLower()).Select(x => x.UserActiveCountryId).FirstOrDefault();
            }

            //check if the customer country is same as the country in the user table
            var user = await _uow.User.GetUserByChannelCode(shipment.CustomerCode);
            if(user != null)
            {
                if(user.UserActiveCountryId != customerCountryId)
                {
                    throw new GenericException($"Payment Failed for waybill {shipment.Waybill}, Contact Customer Care", $"{(int)HttpStatusCode.Forbidden}");
                }
            }

            //2. If the customer country !== Departure Country, Convert the payment
            if (customerCountryId != shipment.DepartureCountryId)
            {
                var countryRateConversion = await _countryRouteZoneMapService.GetZone(customerCountryId, shipment.DepartureCountryId);

                double amountToDebitDouble = (double)amountToDebit * countryRateConversion.Rate;

                amountToDebit = (decimal)Math.Round(amountToDebitDouble, 2);
            }

            //if the shipment is International Shipment & Payment was initiated before the shipment get to Nigeria
            //5% discount should be give to the customer
            if (shipment.IsInternational)
            {
                //check if the shipment has a scan of AISN in Tracking Table, 
                //bool isPresent = await _uow.ShipmentTracking.ExistAsync(x => x.Waybill == shipment.Waybill 
                //&& x.Status == ShipmentScanStatus.AISN.ToString());

                //if (!isPresent)
                //{
                //    //amountToDebit = amountToDebit * 0.95m;
                //    var discount = GetDiscountForInternationalShipmentBasedOnRank(rank);
                //    amountToDebit = amountToDebit * discount;
                //}           

                if(UserChannelType.Ecommerce.ToString() == shipment.CompanyType)
                {
                    var discount = await GetDiscountForInternationalShipmentBasedOnRank(rank, customerCountryId);
                    amountToDebit = amountToDebit * discount;
                }
            }
            return amountToDebit;
        }

        private async Task<decimal> GetDiscountForInternationalShipmentBasedOnRank(Rank rank, int countryId)
        {
            decimal percentage = 0.00M;

            if (rank == Rank.Class)
            {
               var  globalProperty = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.InternationalRankClassDiscount.ToString() && s.CountryId == countryId);
               if(globalProperty != null)
                {
                    percentage = Convert.ToDecimal(globalProperty.Value);                    
                }
            }
            else
            {
                var globalProperty = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.InternationalBasicClassDiscount.ToString() && s.CountryId == countryId);
                if (globalProperty != null)
                {
                    percentage = Convert.ToDecimal(globalProperty.Value);
                }
            }

            decimal discount = ((100M - percentage) / 100M);
            return discount;
        }

        private async Task<DeliveryNumberDTO> GenerateDeliveryNumber(int value, string waybill)
        {
            int maxSize = 6;
            char[] chars = new char[54];
            string a;
            a = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            var strippedText = result.ToString();
            var number = new DeliveryNumber
            {
                SenderCode = "DN" + strippedText.ToUpper(),
                IsUsed = false,
                Waybill = waybill
            };
            var deliverynumberDTO = Mapper.Map<DeliveryNumberDTO>(number);
            _uow.DeliveryNumber.Add(number);
            await _uow.CompleteAsync();
            return await Task.FromResult(deliverynumberDTO);
        }
    }
}