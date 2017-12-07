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
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;

        public PaymentTransactionService(IUnitOfWork uow, IUserService userService, IWalletService walletService)
        {
            _uow = uow;
            _userService = userService;
            _walletService = walletService;
            MapperConfig.Initialize();
        }

        //used for transaction, hence private
        private async Task<object> AddPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new GenericException("NULL_INPUT");

            var transactionExist = await _uow.PaymentTransaction.ExistAsync(x => x.Waybill.Equals(paymentTransaction.Waybill));

            if (transactionExist == true)
                throw new GenericException($"PAYMENT_TRANSACTION_FOR_{paymentTransaction.Waybill}_ALREADY_EXIST");
            
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
                throw new GenericException("PAYMENT_TRANSACTION_DOES_NOT_EXIST");
            }
            _uow.PaymentTransaction.Remove(transaction);
            await _uow.CompleteAsync();
        }

        public async Task UpdatePaymentTransaction(string waybill, PaymentTransactionDTO paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new GenericException("NULL_INPUT");

            var payment = await _uow.PaymentTransaction.GetAsync(x => x.Waybill.Equals(waybill));
            if (payment == null)
                throw new GenericException($"NO_PAYMENT_TRANSACTION_EXIST_FOR_{waybill}_WAYBILL");

            payment.TransactionCode = paymentTransaction.TransactionCode;
            payment.PaymentStatus = paymentTransaction.PaymentStatus;
            payment.PaymentTypes = paymentTransaction.PaymentType;
            await _uow.CompleteAsync();
        }

        public async Task<bool> ProcessPaymentTransaction(PaymentTransactionDTO paymentTransaction)
        {
            var result = false;

            if (paymentTransaction == null)
                throw new GenericException("Null Input");
               
            // get the current user info
            var currentUserId = await _userService.GetCurrentUserId();

            //get Ledger and Invoice
            var generalLedgerEntity = await _uow.GeneralLedger.GetAsync(s => s.Waybill == paymentTransaction.Waybill);
            var invoiceEntity = await _uow.Invoice.GetAsync(s => s.Waybill == paymentTransaction.Waybill);

            //settlement by wallet
            if (paymentTransaction.PaymentType == PaymentType.Wallet)
            {
                //I used transaction code to represent wallet number when process for wallet
                var wallet = await _walletService.GetWalletById(paymentTransaction.TransactionCode);

                //deduct the price for the wallet and update wallet transaction table
                if(wallet.Balance < invoiceEntity.Amount)
                {
                    throw new GenericException("Insufficient Balance in the Wallet");
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
            var paymentTransactionId =  await AddPaymentTransaction(paymentTransaction);

            // update GeneralLedger
            generalLedgerEntity.IsDeferred = false;
            generalLedgerEntity.PaymentType = paymentTransaction.PaymentType;
            generalLedgerEntity.PaymentTypeReference = paymentTransaction.TransactionCode;

            //update invoice
            invoiceEntity.PaymentDate = DateTime.Now;
            invoiceEntity.PaymentMethod = paymentTransaction.PaymentType.ToString();
            invoiceEntity.PaymentStatus = paymentTransaction.PaymentStatus;

            await _uow.CompleteAsync();
            result = true;

            return result;
        }

    }
}
