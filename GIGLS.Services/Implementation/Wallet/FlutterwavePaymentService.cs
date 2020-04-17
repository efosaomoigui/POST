using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
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
            string secretKey = ConfigurationManager.AppSettings["FlutterwaveSecretKey"];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var obj = new
            {
                txref = reference,
                SECKEY = secretKey
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
                    if (verifyResult.data.Status.Equals("successful") && verifyResult.data.ChargeCode.Equals("00") && !paymentLog.IsWaybillSettled && verifyResult.data.Amount == paymentLog.Amount)
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

                    paymentLog.TransactionStatus = verifyResult.data.Status;

                    if (verifyResult.data.validateInstructions.Instruction != null)
                    {
                        paymentLog.TransactionResponse = verifyResult.data.validateInstructions.Instruction;
                    }
                    else if(verifyResult.data.ChargeMessage != null)
                    {
                        paymentLog.TransactionResponse = verifyResult.data.ChargeMessage;
                    }
                    else
                    {
                        paymentLog.TransactionResponse = verifyResult.data.ChargeResponseMessage;
                    }

                    if(verifyResult.data.ChargeCode != null)
                    {
                        if (verifyResult.data.Status.Equals("successful") && verifyResult.data.ChargeCode.Equals("00"))
                        {
                            paymentLog.TransactionResponse = verifyResult.data.ChargeMessage;
                        }
                    }
                    result = true;
                    await _uow.CompleteAsync();
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

                    if (verifyResult.data.validateInstructions.Instruction != null)
                    {
                        paymentLog.TransactionResponse = verifyResult.data.validateInstructions.Instruction;
                    }
                    else if(verifyResult.data.ChargeMessage != null)
                    {
                        paymentLog.TransactionResponse = verifyResult.data.ChargeMessage;
                    }
                    else
                    {
                        paymentLog.TransactionResponse = verifyResult.data.ChargeResponseMessage;
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

            return ManageReturnResponse(webhook);
        }

        public async Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingOTP(WaybillPaymentLog paymentLog, string otp)
        {
            //1. verify the payment  
            //NetworkProvider represent the security code generated from Flutterwave
            var verifyResult = await SubmitOTPForPayment(paymentLog.NetworkProvider, otp);

            if  (verifyResult.Status.Equals("success"))
            {
                //get wallet payment log by reference code
                var waybillPaymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == paymentLog.Reference);

                if (paymentLog == null)
                {
                    return new PaystackWebhookDTO
                    {
                        Status = false,
                        Message = $"No online payment process occurred for the waybill {paymentLog.Waybill}",
                        data = new Core.DTO.OnlinePayment.Data
                        {
                            Message = $"No online payment process occurred for the waybill {paymentLog.Waybill}",
                            Status = "failed"
                        }
                    };
                }

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("successful") && verifyResult.data.ChargeResponseCode.Equals("00") && !paymentLog.IsWaybillSettled && verifyResult.data.Amount == paymentLog.Amount)
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

                paymentLog.TransactionStatus = verifyResult.data.Status;

                if (verifyResult.data.validateInstructions.Instruction != null)
                {
                    paymentLog.TransactionResponse = verifyResult.data.validateInstructions.Instruction;
                }
                else if (verifyResult.data.ChargeMessage != null)
                {
                    paymentLog.TransactionResponse = verifyResult.data.ChargeMessage;
                }
                else
                {
                    paymentLog.TransactionResponse = verifyResult.data.ChargeResponseMessage;
                }

                if (verifyResult.data.Acctvalrespcode.Equals("00"))
                {
                    paymentLog.TransactionResponse = verifyResult.data.Acctvalrespmsg;
                    verifyResult.data.ChargeResponseMessage = verifyResult.data.Acctvalrespmsg;
                }

                await _uow.CompleteAsync();
            }

            return ManageReturnResponse(verifyResult);
        }

        private async Task<FlutterWebhookDTO> SubmitOTPForPayment(string reference, string otp)
        {
            try
            {
                FlutterWebhookDTO result = new FlutterWebhookDTO();

                string flutterSandBox = ConfigurationManager.AppSettings["FlutterSandBox"];
                string flutterValidateOtp = flutterSandBox + ConfigurationManager.AppSettings["FlutterValidateOTP"];
                string PBFPubKey = ConfigurationManager.AppSettings["FlutterwavePubKey"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var obj = new FlutterWaveOTPObject
                {
                    PBFPubKey = PBFPubKey,
                    transactionreference = reference,
                    otp = otp                    
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(obj);
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(flutterValidateOtp, data);
                    string responseResult = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<FlutterWebhookDTO>(responseResult);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                if (flutterResponse.data.validateInstructions.Instruction != null)
                {
                    response.data.Message = flutterResponse.data.validateInstructions.Instruction;
                    response.data.Gateway_Response = flutterResponse.data.validateInstructions.Instruction;
                }
                else if (flutterResponse.data.ChargeMessage != null)
                {
                    response.data.Message = flutterResponse.data.ChargeMessage;
                    response.data.Gateway_Response = flutterResponse.data.ChargeMessage;
                    response.Message = flutterResponse.data.ChargeMessage;
                    if (!flutterResponse.data.Status.Equals("successful"))
                    {
                        response.Status = false;
                    }
                }
                else
                {
                    response.data.Message = flutterResponse.data.ChargeResponseMessage;
                    response.data.Gateway_Response = flutterResponse.data.ChargeResponseMessage;
                }

                if(flutterResponse.data.ChargeCode != null)
                {
                    if (flutterResponse.data.Status.Equals("successful") && flutterResponse.data.ChargeCode.Equals("00"))
                    {
                        response.data.Message = flutterResponse.data.ChargeMessage;
                        response.data.Gateway_Response = flutterResponse.data.ChargeMessage;
                        response.Message = flutterResponse.data.ChargeMessage;
                        response.Status = true;
                    }
                }
            }
            else
            {
                response.data.Message = flutterResponse.Message;
                response.data.Gateway_Response = flutterResponse.Message;
                response.data.Status = flutterResponse.Status;
            }

            return response;
        }
    }
}
