﻿using GIGLS.Core.IServices.PaymentTransactions;
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

namespace GIGLS.Services.Implementation.PaymentTransactions
{
    public class PaymentPartialTransactionService : IPaymentPartialTransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;

        public PaymentPartialTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            MapperConfig.Initialize();
        }

        //used for transaction, hence private
        private async Task<object> AddPaymentPartialTransaction(PaymentPartialTransactionDTO paymentPartialTransaction)
        {
            if (paymentPartialTransaction == null)
                throw new GenericException("Null Input");

            var transactionExist = await _uow.PaymentPartialTransaction.ExistAsync(x => x.Waybill.Equals(paymentPartialTransaction.Waybill));

            if (transactionExist == true)
                throw new GenericException($"Payment Partial Transaction for {paymentPartialTransaction.Waybill} already exist");

            var payment = Mapper.Map<PaymentPartialTransaction>(paymentPartialTransaction);
            _uow.PaymentPartialTransaction.Add(payment);
            //await _uow.CompleteAsync();
            return new { Id = payment.PaymentPartialTransactionId };
        }

        public async Task<PaymentPartialTransactionDTO> GetPaymentPartialTransactionById(string waybill)
        {
            var transaction = await _uow.PaymentPartialTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            return Mapper.Map<PaymentPartialTransactionDTO>(transaction);
        }

        public Task<IEnumerable<PaymentPartialTransactionDTO>> GetPaymentPartialTransactions()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<PaymentPartialTransactionDTO>>(_uow.PaymentPartialTransaction.GetAll()));
        }

        public async Task RemovePaymentPartialTransaction(string waybill)
        {
            var transaction = await _uow.PaymentPartialTransaction.GetAsync(x => x.Waybill.Equals(waybill));

            if (transaction == null)
            {
                throw new GenericException("Payment Partial Transaction does not exist");
            }
            _uow.PaymentPartialTransaction.Remove(transaction);
            await _uow.CompleteAsync();
        }

        public async Task UpdatePaymentPartialTransaction(string waybill, PaymentPartialTransactionDTO paymentPartialTransaction)
        {
            if (paymentPartialTransaction == null)
                throw new GenericException("Null Input");

            var payment = await _uow.PaymentPartialTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            if (payment == null)
                throw new GenericException($"No Payment Partial Transaction exist for {waybill} waybill");

            payment.TransactionCode = paymentPartialTransaction.TransactionCode;
            payment.PaymentStatus = paymentPartialTransaction.PaymentStatus;
            payment.PaymentTypes = paymentPartialTransaction.PaymentType;
            await _uow.CompleteAsync();
        }

        public async Task<bool> ProcessPaymentPartialTransaction(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            var result = false;

            if (paymentPartialTransactionProcessDTO == null)
                throw new GenericException("Null Input");

            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();

            //get Ledger and Invoice
            var waybill = paymentPartialTransactionProcessDTO.Waybill;
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == waybill);

            //get the GrandTotal Amount to be paid
            var grandTotal = invoiceEntity.Amount;

            //get total amount customer is paying
            decimal totalAmountPaid = 0;
            foreach (var paymentPartialTransaction in paymentPartialTransactionProcessDTO.PaymentPartialTransactionDTOs)
            {
                ////////////////////////////////////////////
                //settlement by wallet
                if (paymentPartialTransaction.PaymentType == PaymentType.Wallet)
                {
                    //I used transaction code to represent wallet number when process for wallet
                    var wallet = await _walletService.GetWalletById(paymentPartialTransaction.TransactionCode);

                    //deduct the price for the wallet and update wallet transaction table
                    if (wallet.Balance < paymentPartialTransaction.Amount)
                    {
                        throw new GenericException("Insufficient Balance in the Wallet");
                    }

                    wallet.Balance = wallet.Balance - paymentPartialTransaction.Amount;

                    var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

                    var newWalletTransaction = new WalletTransaction
                    {
                        WalletId = wallet.WalletId,
                        Amount = paymentPartialTransaction.Amount,
                        DateOfEntry = DateTime.Now,
                        ServiceCentreId = serviceCenterIds[0],
                        UserId = currentUserId,
                        CreditDebitType = CreditDebitType.Debit,
                        PaymentType = PaymentType.Wallet,
                        Waybill = waybill,
                        Description = generalLedgerEntity.Description
                    };

                    _uow.WalletTransaction.Add(newWalletTransaction);
                }

                // create paymentPartial
                paymentPartialTransaction.Waybill = waybill;
                paymentPartialTransaction.UserId = currentUserId;
                paymentPartialTransaction.PaymentStatus = PaymentStatus.Paid;
                var paymentTransactionId = await AddPaymentPartialTransaction(paymentPartialTransaction);

                //////////////////////////////////////////
                totalAmountPaid += paymentPartialTransaction.Amount;
            }
            // get the balance
            decimal balanceAmount = grandTotal - totalAmountPaid;


            /////2.  When payment is complete and balance is 0
            if (balanceAmount <= 0)
            {
                // update GeneralLedger
                generalLedgerEntity.IsDeferred = false;
                generalLedgerEntity.PaymentType = PaymentType.Partial;
                //generalLedgerEntity.PaymentTypeReference = paymentPartialTransaction.TransactionCode;

                //update invoice
                invoiceEntity.PaymentDate = DateTime.Now;
                invoiceEntity.PaymentMethod = PaymentType.Partial.ToString();
                invoiceEntity.PaymentStatus = PaymentStatus.Paid;
            }

            await _uow.CompleteAsync();
            result = true;

            return result;
        }

    }
}
