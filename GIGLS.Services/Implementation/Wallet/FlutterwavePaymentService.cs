using GIGLS.Core;
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
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class FlutterwavePaymentService : IFlutterwavePaymentService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly INodeService _nodeService;
        private readonly IMessageSenderService _messageSenderService;

        public FlutterwavePaymentService(IUserService userService, IWalletService walletService, IUnitOfWork uow, 
            IPaymentTransactionService paymentTransactionService, INodeService nodeService, IMessageSenderService messageSenderService)
        {
            _userService = userService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _uow = uow;
            _nodeService = nodeService;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }

        public async Task VerifyAndValidatePayment(FlutterWebhookDTO webhook)
        {
            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(webhook.data.TX_Ref);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                await ProcessPaymentForWaybill(webhook.data.TX_Ref);
            }
            else
            {
                await ProcessPaymentForWallet(webhook.data.TX_Ref);
            }
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

        private async Task<FlutterWebhookDTO> VerifyPayment(string reference)
        {
            FlutterWebhookDTO result = new FlutterWebhookDTO();

            string flutterSandBox = ConfigurationManager.AppSettings["FlutterSandBox"];
            string secretKey = ConfigurationManager.AppSettings["FlutterwaveSecretKey"];
            string authorization = "Bearer " + secretKey;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //int transactionId = await GetTransactionPaymentIdUsingRefCode(reference);
            int transactionId = await GetTransactionPaymentIdUsingRefCodeV2(reference);
            string verifyUrl = flutterSandBox + "transactions/" + transactionId + "/verify";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", authorization);

                var response = await client.GetAsync(verifyUrl);
                string responseResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<FlutterWebhookDTO>(responseResult);
            }

            return result;
        }

        private async Task<int> GetTransactionPaymentIdUsingRefCode(string reference)
        {
            int id = 0;
            string flutterSandBox = ConfigurationManager.AppSettings["FlutterSandBox"];
            string secretKey = ConfigurationManager.AppSettings["FlutterwaveSecretKey"];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            string verifyUrl = flutterSandBox + "transactions?tx_ref=" + reference;
            string authorization = "Bearer " + secretKey;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", authorization);

                var response = await client.GetAsync(verifyUrl);
                string responseResult = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<FlutterTransactionWebhookDTO>(responseResult);

                if (result.Status.Equals("success"))
                {
                    if(result.data.Count > 0)
                    {
                        id = result.data[0].Id;
                    }
                }
            }

            return id;
        }

        private async Task<int> GetTransactionPaymentIdUsingRefCodeV2(string reference)
        {
            int id = 0;
            string verifyUrl = ConfigurationManager.AppSettings["FlutterVerifyV2"];
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
                var response = await client.PostAsync(verifyUrl, data);
                string responseResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FlutterWebhookDTO>(responseResult);

                if (result.Status.Equals("success"))
                {
                    if (result.data != null)
                    {
                        id = result.data.TxId;
                    }
                }
            }

            return id;
        }

        private async Task<FlutterWebhookDTO> VerifyPaymentV2(string reference)
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
            var verifyResult = await VerifyPayment(webhook.data.TX_Ref);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == webhook.data.TX_Ref);

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

            return result;
        }

        private async Task<FlutterWebhookDTO> ProcessPaymentForWaybill(string referenceCode)
        {
            //1. verify the payment 
            var verifyResult = await VerifyPayment(referenceCode);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == referenceCode);

                    if (paymentLog == null)
                        return verifyResult;

                    //2. if the payment successful
                    if (verifyResult.data.Status.Equals("successful") && !paymentLog.IsWaybillSettled && verifyResult.data.Amount == paymentLog.Amount)
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
                    paymentLog.TransactionResponse = verifyResult.data.Processor_Response;
                    await _uow.CompleteAsync();
                }
            }

            return verifyResult;
        }

        private async Task<bool> ProcessPaymentForWalletOld(string reference)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyPayment(reference);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == reference);

                    if (paymentLog == null)
                        return result;

                    if(verifyResult.data.Status != null)
                    {
                        verifyResult.data.Status = verifyResult.data.Status.ToLower();
                    }

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
                    paymentLog.TransactionResponse = verifyResult.data.Processor_Response;
                    await _uow.CompleteAsync();
                    result = true;
                }
            }
            return result;
        }

        private async Task<FlutterWebhookDTO> ProcessPaymentForWallet(string reference)
        {
            //1. verify the payment 
            var verifyResult = await VerifyPayment(reference);

            if (verifyResult.Status.Equals("success"))
            {
                if (verifyResult.data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == reference);

                    if (paymentLog == null)
                        return verifyResult;

                    if (verifyResult.data.Status != null)
                    {
                        verifyResult.data.Status = verifyResult.data.Status.ToLower();
                    }

                    bool sendPaymentNotification = false;
                    var walletDto = new WalletDTO();
                    var userPayload = new UserPayload();
                    var bonusAddon = new BonusAddOn();

                    //2. if the payment successful
                    if (verifyResult.data.Status.Equals("successful") && !paymentLog.IsWalletCredited && verifyResult.data.Amount == paymentLog.Amount)
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
                        bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                        //update the wallet
                        await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                        {
                            WalletId = paymentLog.WalletId,
                            Amount = bonusAddon.Amount,
                            CreditDebitType = CreditDebitType.Credit,
                            Description = bonusAddon.Description,
                            PaymentType = PaymentType.Online,
                            PaymentTypeReference = paymentLog.Reference,
                            UserId = customerId
                        }, false);

                        sendPaymentNotification = true;
                    }

                    //3. update the wallet payment log
                    if (verifyResult.data.Status.Equals("successful"))
                    {
                        paymentLog.IsWalletCredited = true;
                    }

                    paymentLog.TransactionStatus = verifyResult.data.Status;
                    paymentLog.TransactionResponse = verifyResult.data.Processor_Response;
                    await _uow.CompleteAsync();

                    if (sendPaymentNotification)
                    {
                        await SendPaymentNotificationAsync(walletDto, paymentLog);
                    }

                    if (bonusAddon.BonusAdded)
                    {
                        await SendVisaBonusNotificationAsync(bonusAddon, verifyResult, walletDto);
                    }

                    //Call Node API for subscription process
                    if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription)
                    {
                        if (userPayload != null)
                        {
                            await _nodeService.WalletNotification(userPayload);
                        }
                    }
                }
            }
            return verifyResult;
        }

        //Generate security for webhook
        public Task<string> GetSecurityKey()
        {
            var securityKey = ConfigurationManager.AppSettings["FlutterwaveApiSecurityKey"];
            return Task.FromResult(Decrypt(securityKey));
        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText is null)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

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
            FlutterWebhookDTO webhook = new FlutterWebhookDTO();

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(reference);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
               webhook = await ProcessPaymentForWaybill(reference);
            }
            else
            {
                webhook = await ProcessPaymentForWallet(reference);
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
            catch (Exception)
            {
                throw;
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

        private async Task<BonusAddOn> ProcessBonusAddOnForCardType(FlutterWebhookDTO verifyResult, int countryId)
        {
            BonusAddOn result = new BonusAddOn
            {
                Description = "Funding made through debit card.",
                Amount = verifyResult.data.Amount
            };

            if(verifyResult.data.Card.CardType != null)
            {
                if (verifyResult.data.Card.CardType.Contains("visa"))
                {
                    bool isPresent = await IsTheCardInTheList(verifyResult.data.Card.CardBIN, countryId);
                    if (isPresent)
                    {
                        result.Amount = await CalculateCardBonus(result.Amount, countryId);
                        result.Description = $"{result.Description}. Bonus Added for Using {verifyResult.data.Card.Brand}";
                        result.BonusAdded = true;
                    }
                }
            }

            return result;
        }

        private async Task<decimal> CalculateCardBonus(decimal amount, int countryId)
        {
            var global = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBusinessCardBonus.ToString() && s.CountryId == countryId);
            if (global != null)
            {
                decimal bonusPercentage = decimal.Parse(global.Value);
                decimal bonusValue = bonusPercentage / 100M;
                decimal price = amount * bonusValue;
                amount = amount + price;
            }
            return amount;
        }

        private async Task<bool> IsTheCardInTheList(string bin, int countryId)
        {
            bool result = false;
            var global = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBusinessCardList.ToString() && s.CountryId == countryId);
            if (global != null)
            {
                int.TryParse(bin, out int binInt);

                List<int> visaList = new List<int>(Array.ConvertAll(global.Value.Split(','), int.Parse));
                if (visaList.Contains(binInt))
                {
                    result = true;
                }
            }
            return result;
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

        private async Task SendVisaBonusNotificationAsync(BonusAddOn bonusAddon, FlutterWebhookDTO verifyResult, WalletDTO walletDto)
        {
            string body = $"{bonusAddon.Description} / Bin {verifyResult.data.Card.CardBIN} / Ref code {verifyResult.data.TX_Ref}  / Bank {verifyResult.data.Card.Brand}";

            var message = new MessageDTO()
            {
                Subject = "Visa Business Card Bonus",
                CustomerCode = walletDto.CustomerEmail,
                CustomerName = walletDto.CustomerName,
                Body = body
            };

            //send a copy to chairman
            var visaBonusEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBonusEmail.ToString() && s.CountryId == 1);

            if (visaBonusEmail != null)
            {
                //seperate email by comma and send message to those email
                string[] emails = visaBonusEmail.Value.Split(',').ToArray();

                foreach (string email in emails)
                {
                    message.ToEmail = email;
                    await _messageSenderService.SendEcommerceRegistrationNotificationAsync(message);
                }
            }
        }

    }
}
