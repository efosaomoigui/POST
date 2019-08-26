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

namespace GIGLS.Services.Implementation.PaymentTransactions
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IGlobalPropertyService _globalPropertyService;

        public PaymentTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService,
            IGlobalPropertyService globalPropertyService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            _globalPropertyService = globalPropertyService;
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

            if(returnWaybill != null)
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

            //get Ledger, Invoice, shipment
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);

            //all account customers payment should be settle by wallet automatically
            //settlement by wallet
            if (shipment.CustomerType == CustomerType.Company.ToString() || paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                //I used transaction code to represent wallet number when processing for wallet
                var wallet = await _walletService.GetWalletById(paymentTransaction.TransactionCode);

                //Additions for Ecommerce customers (Max wallet negative payment limit)
                //var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == paymentTransaction.Waybill);
                if (shipment != null && CompanyType.Ecommerce.ToString() == shipment.CompanyType)
                {
                    //Gets the customer wallet limit for ecommerce
                    decimal ecommerceNegativeWalletLimit = await GetEcommerceWalletLimit(shipment);

                    //deduct the price for the wallet and update wallet transaction table
                    if (wallet.Balance - invoiceEntity.Amount < (System.Math.Abs(ecommerceNegativeWalletLimit) * (-1)))
                    {
                        throw new GenericException("Ecommerce Customer. Insufficient Balance in the Wallet");
                    }
                }
                
                //for other customers
                //deduct the price for the wallet and update wallet transaction table
                //--Update April 25, 2019: Corporate customers should be debited from wallet
                if (shipment != null && CompanyType.Client.ToString() == shipment.CompanyType)
                {
                    if (wallet.Balance < invoiceEntity.Amount)
                    {
                        throw new GenericException("Insufficient Balance in the Wallet");
                    }
                }

                wallet.Balance = wallet.Balance - invoiceEntity.Amount;

                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

                var newWalletTransaction = new WalletTransaction
                {
                    WalletId = wallet.WalletId,
                    Amount = invoiceEntity.Amount,
                    DateOfEntry = DateTime.Now,
                    ServiceCentreId = serviceCenterIds[0],
                    UserId = currentUserId,
                    CreditDebitType = CreditDebitType.Debit,
                    PaymentType = PaymentType.Wallet,
                    Waybill = paymentTransaction.Waybill,
                    Description = generalLedgerEntity.Description
                };

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
            invoiceEntity.PaymentStatus = paymentTransaction.PaymentStatus;
            invoiceEntity.PaymentTypeReference = paymentTransaction.TransactionCode;

            await _uow.CompleteAsync();
            result = true;

            return result;
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


                //for other customers
                //deduct the price for the wallet and update wallet transaction table
                if (shipment != null && CompanyType.Ecommerce.ToString() != shipment.CompanyType)
                {
                    if (wallet.Balance < invoiceEntity.Amount)
                    {
                        throw new GenericException("Insufficient Balance in the Wallet");
                    }
                }

                wallet.Balance = wallet.Balance - invoiceEntity.Amount;

                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

                var newWalletTransaction = new WalletTransaction
                {
                    WalletId = wallet.WalletId,
                    Amount = invoiceEntity.Amount,
                    DateOfEntry = DateTime.Now,
                    ServiceCentreId = serviceCenterIds[0],
                    UserId = currentUserId,
                    CreditDebitType = CreditDebitType.Debit,
                    PaymentType = PaymentType.Wallet,
                    Waybill = paymentTransaction.Waybill,
                    Description = generalLedgerEntity.Description
                };

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
            invoiceEntity.PaymentStatus = paymentTransaction.PaymentStatus;
            invoiceEntity.PaymentTypeReference = paymentTransaction.TransactionCode;

            await _uow.CompleteAsync();
            result = true;

            return result;
        }

        private async Task<decimal> GetEcommerceWalletLimit(Shipment shipment)
        {
            decimal ecommerceNegativeWalletLimit = 0;

            //Get the Customer Wallet Limit Category
            var companyObj = await _uow.Company.GetAsync(x => x.CustomerCode.ToLower() == shipment.CustomerCode.ToLower());
            var customerWalletLimitCategory = companyObj.CustomerCategory;

            var userActiveCountryId = 1;
            try
            {
                userActiveCountryId = await _userService.GetUserActiveCountryId();
            }
            catch (Exception ex) { }

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
    }
}
