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
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices.Customers;

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
        private readonly INodeService _nodeService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;


        public PaymentTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService,
            IGlobalPropertyService globalPropertyService, ICountryRouteZoneMapService countryRouteZoneMapService,
            IMessageSenderService messageSenderService, IFinancialReportService financialReportService, INodeService nodeService, INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            _globalPropertyService = globalPropertyService;
            _countryRouteZoneMapService = countryRouteZoneMapService;
            _messageSenderService = messageSenderService;
            _financialReportService = financialReportService;
            _nodeService = nodeService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;

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
            var shipment = await _uow.Shipment.GetAsync(s => s.Waybill == paymentTransaction.Waybill);

            //all account customers payment should be settle by wallet automatically
            //settlement by wallet
            if (paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                paymentTransaction.TransactionCode = shipment.CustomerCode;
                if (paymentTransaction.IsNotOwner)
                {
                    paymentTransaction.UserId = paymentTransaction.CustomerUserId;
                    paymentTransaction.TransactionCode = paymentTransaction.CustomerCode;
                    await ProcessWalletPaymentForShipment(paymentTransaction, shipment, invoiceEntity, generalLedgerEntity, paymentTransaction.UserId); 
                }
                else
                {
                    await ProcessWalletTransaction(paymentTransaction, shipment, invoiceEntity, generalLedgerEntity, currentUserId);
                }
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

            //Send Email to Sender when Payment for International Shipment has being made
            if (invoiceEntity.IsInternational == true)
            {
                var shipmentDTO = Mapper.Map<ShipmentDTO>(shipment);
                await _messageSenderService.SendOverseasPaymentConfirmationMails(shipmentDTO);
                return true;
            }

            var shipmentObjDTO = Mapper.Map<ShipmentDTO>(shipment);
            if (shipment.DepartureServiceCentreId == 309)
            {
                await _messageSenderService.SendMessage(MessageType.HOUSTON, EmailSmsType.SMS, smsData);
                //Commented this out 15/06/2021 to implement new email 
                //await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.Email, smsData);
                await _messageSenderService.SendEmailToCustomerForShipmentCreation(shipmentObjDTO);
            }
            else
            {

                //if (paymentTransaction.IsNotOwner)
                //{
                //  //TODO: SEND PAYMENT NOTIFICATION FOR ALREADY CREATED SHIPMENT 
                //}
                //Commented this out 15/06/2021 to implement new email
                //await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.All, smsData);
                //sperated the previous implementation into sms / email
                //else
                //{
                //    await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.SMS, smsData);
                //    await _messageSenderService.SendEmailToCustomerForShipmentCreation(shipmentObjDTO); 
                //}

                //Set sms template for Ghana or other coutry
                var countryId = await _userService.GetUserActiveCountryId();
                if (countryId == 1)
                {
                    if (shipmentObjDTO.ExpressDelivery)
                    {
                        await _messageSenderService.SendMessage(MessageType.CRTGF, EmailSmsType.SMS, smsData);
                    }
                    else
                    {
                        await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.SMS, smsData);
                    }
                    
                }
                else
                {
                    await _messageSenderService.SendMessage(MessageType.CRTGH, EmailSmsType.SMS, smsData);
                }
                await _messageSenderService.SendEmailToCustomerForShipmentCreation(shipmentObjDTO);
            }

            //grouping and manifesting shipment
            if (shipment.IsBulky)
            {
                await MappingWaybillNumberToGroupForBulk(shipment.Waybill);
            }
            else
            {
                await MappingWaybillNumberToGroup(shipment.Waybill); 
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
                    throw new GenericException(" Shipment successfully created, however payment could not be processed for ecommerce customer due to insufficient wallet balance ");
                }
            }

            //for other customers
            //deduct the price for the wallet and update wallet transaction table
            //--Update April 25, 2019: Corporate customers should be debited from wallet
            if (shipment != null && CompanyType.Client.ToString() == shipment.CompanyType)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Shipment successfully created, however payment could not be processed for customer due to insufficient wallet balance ");
                }
            }

            if (shipment != null && paymentTransaction.FromApp == true)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Shipment successfully created, however payment could not be processed for customer due to insufficient wallet balance ");
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

            var shipmentObjDTO = Mapper.Map<ShipmentDTO>(shipment);
            if (shipment.DepartureServiceCentreId == 309)
            {
                await _messageSenderService.SendMessage(MessageType.HOUSTON, EmailSmsType.SMS, smsData);
                //Commented this out 15/06/2021 to implement new email 
                //await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.Email, smsData);
                await _messageSenderService.SendEmailToCustomerForShipmentCreation(shipmentObjDTO);
            }
            else
            {
                //Commented this out 15/06/2021 to implement new email
                //await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.All, smsData);

                //sperated the previous implementation into sms / email
                await _messageSenderService.SendMessage(MessageType.CRT, EmailSmsType.SMS, smsData);
                await _messageSenderService.SendEmailToCustomerForShipmentCreation(shipmentObjDTO);
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


        public async Task<bool> ProcessPaymentTransactionGIGGO(PaymentTransactionDTO paymentTransaction)
        {
            var result = false;
           
            //check if waybill is from BOT
            var preshipment = await _uow.PreShipmentMobile.GetPreshipmentMobileByWaybill(paymentTransaction.Waybill);
            if (preshipment != null)
            {
                //CHECK IF IS BOT USER
                var customer = await _uow.Company.GetAsync(x => x.CustomerCode == preshipment.CustomerCode);
                if (customer != null && customer.TransactionType == WalletTransactionType.BOT)
                {
                    var nodePayload = new CreateShipmentNodeDTO()
                    {
                        waybillNumber = preshipment.Waybill,
                        customerId = preshipment.CustomerCode,
                        locality = preshipment.SenderLocality,
                        receiverAddress = preshipment.ReceiverAddress,
                        vehicleType = preshipment.VehicleType,
                        value = preshipment.Value,
                        zone = preshipment.ZoneMapping,
                        senderAddress = preshipment.SenderAddress,
                        receiverLocation = new NodeLocationDTO()
                        {
                            lng = preshipment.ReceiverLng,
                            lat = preshipment.ReceiverLat
                        },
                        senderLocation = new NodeLocationDTO()
                        {
                            lng = preshipment.SenderLng,
                            lat = preshipment.SenderLat
                        }
                    };
                    await _nodeService.CreateShipment(nodePayload);
                    var shipmentToUpdate = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == paymentTransaction.Waybill);
                    if (shipmentToUpdate != null)
                    {
                        //Update shipment to shipment created
                        shipmentToUpdate.shipmentstatus = "Shipment created";
                        var userId = await _userService.GetCurrentUserId();

                        var user = await _uow.PreShipmentMobile.GetBotUserWithPhoneNo(shipmentToUpdate.SenderPhoneNumber);
                        if (String.IsNullOrEmpty(user.CustomerCode))
                        {
                            //create this user first
                            var newCustomer = new CustomerDTO()
                            {
                                PhoneNumber = shipmentToUpdate.SenderPhoneNumber,
                                FirstName = shipmentToUpdate.SenderName,
                                LastName = shipmentToUpdate.SenderName,
                                UserActiveCountryId = 1,
                                City = shipmentToUpdate.SenderLocality,
                                CustomerType = CustomerType.IndividualCustomer,
                                Address = shipmentToUpdate.SenderAddress,
                                ReturnAddress = shipmentToUpdate.SenderAddress,
                                Gender = Gender.Male,
                                CustomerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.CustomerCodeIndividual)
                            };
                            var newIndCustomer = Mapper.Map<IndividualCustomer>(newCustomer);
                            _uow.IndividualCustomer.Add(newIndCustomer);
                            await _uow.CompleteAsync();
                            user = newCustomer;
                        }
                        if (!String.IsNullOrEmpty(user.CustomerCode))
                        {
                            //Pin Generation 
                            var message = new MobileShipmentCreationMessageDTO
                            {
                                SenderPhoneNumber = shipmentToUpdate.SenderPhoneNumber,
                                WaybillNumber = shipmentToUpdate.Waybill
                            };
                            var number = await _globalPropertyService.GenerateDeliveryCode();
                            var deliveryNumber = new DeliveryNumber
                            {
                                SenderCode = number,
                                IsUsed = false,
                                Waybill = shipmentToUpdate.Waybill
                            };
                            _uow.DeliveryNumber.Add(deliveryNumber);
                            message.QRCode = deliveryNumber.SenderCode;

                            if (user.CustomerType == CustomerType.IndividualCustomer)
                            {
                                var indCust = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode == user.CustomerCode);
                                if (indCust != null)
                                {
                                    shipmentToUpdate.CustomerCode = user.CustomerCode;
                                    shipmentToUpdate.CustomerType = CustomerType.IndividualCustomer.ToString();
                                    shipmentToUpdate.CompanyType = CompanyType.Client.ToString();
                                    shipmentToUpdate.UserId = userId;
                                    shipmentToUpdate.SenderPhoneNumber = indCust.PhoneNumber;
                                    message.SenderName = indCust.FirstName + " " + indCust.LastName;
                                }
                            }
                            else 
                            {
                                var compCust = await _uow.Company.GetAsync(x => x.CustomerCode == user.CustomerCode);
                                if (compCust != null)
                                {
                                    shipmentToUpdate.CustomerCode = user.CustomerCode;
                                    shipmentToUpdate.CustomerType = CustomerType.IndividualCustomer.ToString();
                                    shipmentToUpdate.CompanyType = compCust.CompanyType.ToString();
                                    shipmentToUpdate.UserId = userId;
                                    shipmentToUpdate.SenderPhoneNumber = compCust.PhoneNumber;
                                    message.SenderName = compCust.Name;
                                }
                            }
                            await _messageSenderService.SendMessage(MessageType.MCS, EmailSmsType.SMS, message);
                        }
                    }
                }
                await _uow.CompleteAsync();
            }
            result = true;
            return result;
        }


        private async Task<bool> ProcessWalletPaymentForShipment(PaymentTransactionDTO paymentTransaction, Shipment shipment, Invoice invoiceEntity, GeneralLedger generalLedgerEntity, string currentUserId)
        {
            //I used transaction code to represent wallet number when processing for wallet
            var wallet = await _walletService.GetWalletById(paymentTransaction.TransactionCode);

            decimal amountToDebit = invoiceEntity.Amount;

            amountToDebit = await GetActualAmountToDebitForNotOwner(shipment, amountToDebit, paymentTransaction);

            //Additions for Ecommerce customers (Max wallet negative payment limit)
            //var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);
            if (shipment != null && CompanyType.Ecommerce.ToString() == shipment.CompanyType && !paymentTransaction.FromApp)
            {
                //Gets the customer wallet limit for ecommerce
                decimal ecommerceNegativeWalletLimit = await GetEcommerceWalletLimit(shipment);

                //deduct the price for the wallet and update wallet transaction table
                if (wallet.Balance - amountToDebit < (Math.Abs(ecommerceNegativeWalletLimit) * (-1)))
                {
                    throw new GenericException("Payment could not be processed for customer due to insufficient wallet balance");
                }
            }

            //for other customers
            //deduct the price for the wallet and update wallet transaction table
            //--Update April 25, 2019: Corporate customers should be debited from wallet
            if (shipment != null && CompanyType.Client.ToString() == shipment.CompanyType)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Payment could not be processed for customer due to insufficient wallet balance ");
                }
            }

            if (shipment != null && paymentTransaction.FromApp == true)
            {
                if (wallet.Balance < amountToDebit)
                {
                    throw new GenericException("Payment could not be processed for customer due to insufficient wallet balance ");
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
            return true;
        }

        private async Task<decimal> GetActualAmountToDebitForNotOwner(Shipment shipment, decimal amountToDebit, PaymentTransactionDTO paymentTransaction)
        {
            //1. Get Customer Country detail
            int customerCountryId = 0;
            Rank rank = Rank.Basic;
            var user = await _uow.User.GetUserByChannelCode(paymentTransaction.CustomerCode);
            if (user != null)
            {
                if (UserChannelType.Ecommerce == user.UserChannelType || UserChannelType.Corporate == user.UserChannelType)
                {
                    var customer = _uow.Company.GetAllAsQueryable().Where(x => x.CustomerCode.ToLower() == paymentTransaction.CustomerCode.ToLower()).FirstOrDefault();
                    if (customer != null)
                    {
                        customerCountryId = customer.UserActiveCountryId;
                        rank = customer.Rank;
                    }
                }
                else
                {
                    customerCountryId = _uow.IndividualCustomer.GetAllAsQueryable().Where(x => x.CustomerCode.ToLower() == paymentTransaction.CustomerCode.ToLower()).Select(x => x.UserActiveCountryId).FirstOrDefault();
                }

                //check if the customer country is same as the country in the user table
                if (user.UserActiveCountryId != customerCountryId)
                {
                    throw new GenericException($"Payment Failed for waybill {shipment.Waybill}, Contact Customer Care", $"{(int)HttpStatusCode.Forbidden}");
                }

                //2. If the customer country !== Departure Country, Convert the payment
                if (customerCountryId != shipment.DepartureCountryId)
                {
                    var countryRateConversion = await _countryRouteZoneMapService.GetZone(customerCountryId, shipment.DepartureCountryId);

                    double amountToDebitDouble = (double)amountToDebit * countryRateConversion.Rate;

                    amountToDebit = (decimal)Math.Round(amountToDebitDouble, 2);
                }

                //3. if the shipment is International Shipment & Payment was initiated before the shipment get to Nigeria
                //5% discount should be give to the customer
                if (shipment.IsInternational)
                {
                    if (UserChannelType.Ecommerce.ToString() == shipment.CompanyType)
                    {
                        var discount = await GetDiscountForInternationalShipmentBasedOnRank(rank, customerCountryId);
                        amountToDebit = amountToDebit * discount;
                    }
                }
            }

            return amountToDebit;
        }

        #region IMPLEMENTING GROUPING AND MANIFESTING

        //map waybillNumber to groupWaybillNumber
        private async Task MappingWaybillNumberToGroup(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));


                
                //check if the group already exist for centre
                var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == false).FirstOrDefault();
                if (groupwaybillExist == null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {

                    //check if it has a manifest mapping
                    var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupwaybillExist.GroupWaybillCode).FirstOrDefault();
                    if (isManifestGroupWaybillMapped is null)
                    {
                        //map new waybill to existing groupwaybill 
                        await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                    }

                    else
                    {
                        //confirm if the manifest has been dispatched
                        var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                        if (manifestDispatched)
                        {
                            await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                        }
                        else
                        {
                            var manifest = await _uow.Manifest.GetAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode);
                            if (manifest is null)
                            {
                                await CreateNewManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                            }
                            else if(manifest != null)
                            {
                                //get date for the manifest
                                var today = DateTime.Now;
                                int hours = Convert.ToInt32((today - manifest.DateCreated).TotalHours);
                                if (hours >= 24)
                                {
                                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                                }
                                else
                                {
                                    //map new waybill to existing groupwaybill 
                                    await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, manifest, groupwaybillExist);
                                }
                            }
                        } 
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task MappingWaybillNumberToGroupForBulk(string waybill)
        {
            try
            {
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                //validate waybill 
                var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                if (shipment is null)
                {
                    throw new GenericException("Error Grouping waybill. An error occured while trying to group waybill automatically,please manually group waybill.");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();
                var destServiceCentre = await _uow.ServiceCentre.GetAsync(shipment.DestinationServiceCentreId);
                var deptServiceCentre = await _uow.ServiceCentre.GetAsync(departureServiceCenterId);
                var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));



                //check if the bulk manifest exist
                var bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery).FirstOrDefault();
                if (bulkManifest is null)
                {
                    await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                }
                else
                {
                    var today = DateTime.Now;
                    int hours = Convert.ToInt32((today - bulkManifest.DateCreated).TotalHours);
                    if (hours >= 24)
                    {
                        await NewGroupWaybillProcess(shipment, deptServiceCentre, destServiceCentre, currentUserId);
                    }
                    else
                    {
                        var groupwaybillExist = _uow.GroupWaybillNumber.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.ServiceCentreId == shipment.DestinationServiceCentreId && x.DepartureServiceCentreId == shipment.DepartureServiceCentreId && x.ExpressDelivery == shipment.ExpressDelivery && x.IsBulky == shipment.IsBulky).FirstOrDefault();
                        if (groupwaybillExist == null)
                        {
                            bulkManifest = _uow.Manifest.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.IsBulky && !x.IsDispatched && x.ExpressDelivery == shipment.ExpressDelivery).FirstOrDefault();
                            await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                        }
                        else
                        {
                            //map new waybill to existing groupwaybill
                            var isManifestGroupWaybillMapped = _uow.ManifestGroupWaybillNumberMapping.GetAllAsQueryable().OrderByDescending(x => x.DateCreated).Where(x => x.GroupWaybillNumber == groupwaybillExist.GroupWaybillCode).FirstOrDefault();
                            if (isManifestGroupWaybillMapped is null)
                            {
                                //map new waybill to existing groupwaybill 
                                await CreateNewManifestGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, groupwaybillExist);
                            }
                            var manifestDispatched = await _uow.Manifest.ExistAsync(x => x.ManifestCode == isManifestGroupWaybillMapped.ManifestCode && x.IsDispatched);
                            if (manifestDispatched)
                            {
                                await MapNewGroupWaybillToExistingManifest(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest);
                            }
                            else
                            {
                                await MapExistingGroupWaybill(shipment, deptServiceCentre, destServiceCentre, currentUserId, bulkManifest, groupwaybillExist); 
                            }
                        } 
                    }
                }
                shipment.IsGrouped = true;
                var updateTransitWaybill = await _uow.TransitWaybillNumber.GetAsync(x => x.WaybillNumber == shipment.Waybill);
                if (updateTransitWaybill != null)
                {
                    updateTransitWaybill.IsGrouped = true;
                }
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private async Task NewGroupWaybillProcess(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre,string userId)
        {
            // generate new manifest code
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, deptServiceCentre.Code);
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

            // create a group waybillnumbermapping
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybillNumber,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            //also create a minifest group manifest and add group waybill to it
            //Add new Mapping
            var newMapping = new ManifestGroupWaybillNumberMapping
            {
                ManifestCode = manifestCode,
                GroupWaybillNumber = newGroupWaybill.GroupWaybillCode,
                IsActive = true,
                DateMapped = DateTime.Now
            };
            _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);

            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);
        }

        private async Task CreateNewManifestGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId,GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, deptServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping); 
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

        }

        private async Task MapExistingGroupWaybill(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest, GroupWaybillNumber groupWaybill)
        {
            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping); 
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);

        }

        private async Task CreateNewManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, GroupWaybillNumber groupWaybill)
        {
            var manifestCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Manifest, deptServiceCentre.Code);
            //create new manifest
            var newManifest = new Manifest
            {
                DateTime = DateTime.Now,
                ManifestCode = manifestCode,
                ExpressDelivery = shipment.ExpressDelivery,
                IsBulky = shipment.IsBulky
            };
            _uow.Manifest.Add(newManifest);

            //also  map group waybill to existing manifest
            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
            if (!exist)
            {
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifestCode,
                    GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping); 
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybill.GroupWaybillCode);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybill.GroupWaybillCode,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);
        }

        private async Task MapNewGroupWaybillToExistingManifest(Shipment shipment, ServiceCentre deptServiceCentre, ServiceCentre destServiceCentre, string userId, Manifest manifest)
        {
            var groupWaybillNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, deptServiceCentre.Code);

            //create a groupwaybill for centre
            var newGroupWaybill = new GroupWaybillNumber
            {
                GroupWaybillCode = groupWaybillNumber,
                UserId = userId,
                ServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                HasManifest = true,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumber.Add(newGroupWaybill);

            var exist = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
            if (!exist)
            {
                //also  map group waybill to existing manifest
                var newMapping = new ManifestGroupWaybillNumberMapping
                {
                    ManifestCode = manifest.ManifestCode,
                    GroupWaybillNumber = groupWaybillNumber,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                };
                _uow.ManifestGroupWaybillNumberMapping.Add(newMapping); 
            }
            if (exist)
            {
                var gmw = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
                gmw.DateMapped = DateTime.Now;
            }

            //map new waybill to existing groupwaybill 
            var newGroupWaybillNoMapping = new GroupWaybillNumberMapping
            {
                GroupWaybillNumber = groupWaybillNumber,
                UserId = userId,
                DestinationServiceCentreId = destServiceCentre.ServiceCentreId,
                IsActive = true,
                DepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                ExpressDelivery = shipment.ExpressDelivery,
                OriginalDepartureServiceCentreId = deptServiceCentre.ServiceCentreId,
                WaybillNumber = shipment.Waybill,
                DateMapped = DateTime.Now,
                IsBulky = shipment.IsBulky
            };
            _uow.GroupWaybillNumberMapping.Add(newGroupWaybillNoMapping);
        }

        #endregion



    }
}                                                                                                      