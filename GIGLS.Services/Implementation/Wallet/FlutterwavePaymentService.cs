﻿using GIGLS.Core;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class FlutterwavePaymentService : IFlutterwavePaymentService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;
        private readonly IPaymentTransactionService _paymentTransactionService;

        public FlutterwavePaymentService(IUserService userService, IWalletService walletService, IUnitOfWork uow, IPaymentTransactionService paymentTransactionService)
        {
            _userService = userService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task VerifyAndValidatePayment(FlutterWebhookDTO webhook)
        {
            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(webhook.data.TXRef);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                await ProcessPaymentForWaybill(webhook);
            }
            else
            {
                await ProcessPaymentForWallet(webhook);
            }
        }


        private WaybillWalletPaymentType GetPackagePaymentType(string refCode)
        {
            if (refCode.StartsWith("wb"))
            {
                return WaybillWalletPaymentType.Waybill;
            }

            return WaybillWalletPaymentType.Wallet;
        }

        private async Task<FlutterWebhookDTO> VerifyPayment(string reference)
        {
            FlutterWebhookDTO result = new FlutterWebhookDTO();

            string flutterSandBox = ConfigurationManager.AppSettings["FlutterSandBox"];
            string flutterVerify = flutterSandBox + ConfigurationManager.AppSettings["FlutterVerify"];
            string PBFPubKey = ConfigurationManager.AppSettings["FlutterwavePubKey"];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var obj = new
            {
                txref = reference,
                SECKEY = PBFPubKey
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(obj);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(flutterVerify, data);
                string responseResult = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<FlutterWebhookDTO>(responseResult);
            }

            return result;
        }

        private async Task<bool> ProcessPaymentForWaybill(FlutterWebhookDTO webhook)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyPayment(webhook.data.TXRef);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == webhook.data.TXRef);

                    if (paymentLog == null)
                        return result;

                    //2. if the payment successful
                    if (verifyResult.data.Status.Equals("successful") && !paymentLog.IsWaybillSettled && verifyResult.data.Amount == paymentLog.Amount)
                    {
                        //3. Process payment for the waybill if successful
                        PaymentTransactionDTO paymentTransaction = new PaymentTransactionDTO
                        {
                            Waybill = paymentLog.Waybill,
                            PaymentType = PaymentType.Cash,
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

                    paymentLog.TransactionStatus = verifyResult.data.Status;
                    paymentLog.TransactionResponse = verifyResult.data.ChargeResponseCode + " | " + verifyResult.data.ChargeResponseMessage;

                    if (verifyResult.data.validateInstructions != null)
                    {
                        paymentLog.TransactionResponse = paymentLog.TransactionResponse + " | " + verifyResult.data.validateInstructions.Instruction;
                    }

                    await _uow.CompleteAsync();
                    result = true;
                }
            }

            return await Task.FromResult(result);
        }

        private async Task<bool> ProcessPaymentForWallet(FlutterWebhookDTO webhook)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyPayment(webhook.data.TXRef);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == webhook.data.TXRef);

                    if (paymentLog == null)
                        return result;

                    //2. if the payment successful
                    if (verifyResult.data.Status.Equals("successful") && !paymentLog.IsWalletCredited && verifyResult.data.Amount == paymentLog.Amount)
                    {
                        //a. update the wallet for the customer
                        string customerId = null;  //set customer id to null

                        //get wallet detail to get customer code
                        var walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                        if (walletDto != null)
                        {
                            //use customer code to get customer id
                            var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                            if (user != null)
                                customerId = user.Id;
                        }

                        //update the wallet
                        await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                        {
                            WalletId = paymentLog.WalletId,
                            Amount = verifyResult.data.Amount,
                            CreditDebitType = CreditDebitType.Credit,
                            Description = "Funding made through online payment",
                            PaymentType = PaymentType.Online,
                            PaymentTypeReference = paymentLog.Reference,
                            UserId = customerId
                        }, false);
                    }

                    //3. update the wallet payment log
                    if (verifyResult.data.Status.Equals("successful"))
                    {
                        paymentLog.IsWalletCredited = true;
                    }

                    paymentLog.TransactionStatus = verifyResult.data.Status;
                    paymentLog.TransactionResponse = verifyResult.data.ChargeResponseCode + " | " + verifyResult.data.ChargeResponseMessage;

                    if (verifyResult.data.validateInstructions != null)
                    {
                        paymentLog.TransactionResponse = paymentLog.TransactionResponse + " | " + verifyResult.data.validateInstructions.Instruction;
                    }

                    await _uow.CompleteAsync();
                    result = true;
                }
            }
            return await Task.FromResult(result);
        }

        //Generate security for webhook
        public async Task<string> GetSecurityKey()
        {
            var securityKey = ConfigurationManager.AppSettings["FlutterwaveApiSecurityKey"];
            return await Decrypt(securityKey);
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

        public async Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference)
        {
            var response = new PaystackWebhookDTO();

            var webhook = await VerifyPayment(reference);

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(reference);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                await ProcessPaymentForWaybill(webhook);
            }
            else
            {
                await ProcessPaymentForWallet(webhook);
            }

            response.Message = webhook.Message;
            if (webhook.Status == "success")
            {
                response.Status = true;
            }

            if (webhook.data != null)
            {
                response.data.Status = webhook.data.Status;
                response.data.Message = webhook.data.ChargeResponseCode + " | " + webhook.data.ChargeResponseMessage;

                if (webhook.data.validateInstructions != null)
                {
                    response.data.Message = response.data.Message + " | " + webhook.data.validateInstructions.Instruction;
                }
            }
            else
            {
                response.data.Message = webhook.Message;
                response.data.Status = webhook.Status;
            }

            return response;
        }
    }
}
