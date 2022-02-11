using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
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
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation.Utility.CellulantEncryptionService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
    public class KorapayPaymentService : IKorapayPaymentService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private IServiceCentreService _serviceCenterService;
        private readonly IWalletService _walletService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly INodeService _nodeService;
        private readonly IMessageSenderService _messageSenderService;
        public KorapayPaymentService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService,
            IWalletService walletService, IPaymentTransactionService paymentTransactionService, INodeService nodeService, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _nodeService = nodeService;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }

        #region Korapay Payment Gateway

        private async Task<bool> VerifyAndValidatePayment(KorapayWebhookDTO payload)
        {
            bool response = false;

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(payload?.Data?.Reference);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                response = await ProcessPaymentForWaybill(payload);
            }
            else
            {
                response = await ProcessPaymentForWallet(payload);
            }

            return response;
        }

        private async Task<bool> ProcessPaymentForWaybill(KorapayWebhookDTO payload)
        {
            //1. verify the payment 
            var verifyResult = payload;
            var result = false;

            if (verifyResult.Event.Equals("charge.success"))
            {
                if (verifyResult.Data != null)
                {
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == verifyResult.Data.PaymentReference);

                    if (paymentLog == null)
                        return result;

                    //2. if the payment successful
                    if (verifyResult.Data.TransactionStatus.Equals("success") && !paymentLog.IsWaybillSettled)
                    {
                        var checkAmount = ValidatePaymentValue(paymentLog.Amount,Convert.ToDecimal(verifyResult.Data.Amount));

                        if (checkAmount)
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
                    }

                    paymentLog.TransactionStatus = verifyResult?.Data?.Status;
                    paymentLog.TransactionResponse = verifyResult?.Data?.TransactionStatus;
                    result = true;
                    await _uow.CompleteAsync();
                }
            }

            return result;
        }

        private async Task<bool> ProcessPaymentForWallet(KorapayWebhookDTO payload)
        {
            //1. verify the payment 
            var verifyResult = payload;
            bool result = false;
            if (verifyResult.Event.Equals("charge.success"))
            {
                if (verifyResult.Data != null)
                {
                    _uow.BeginTransaction(IsolationLevel.RepeatableRead);
                    //get wallet payment log by reference code
                    var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == verifyResult.Data.PaymentReference);

                    if (paymentLog == null)
                        return result;

                    if (verifyResult.Data.TransactionStatus != null)
                    {
                        verifyResult.Data.TransactionStatus = verifyResult.Data.TransactionStatus.ToLower();
                    }

                    bool sendPaymentNotification = false;
                    var walletDto = new WalletDTO();
                    var userPayload = new UserPayload();
                    var bonusAddon = new BonusAddOn();
                    bool checkAmount = false;

                    //2. if the payment successful
                    if (verifyResult.Data.TransactionStatus.Equals("success") && !paymentLog.IsWalletCredited)
                    {
                        checkAmount = ValidatePaymentValue(paymentLog.Amount,Convert.ToDecimal(verifyResult.Data.Amount));

                        if (checkAmount)
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
                            //bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                            //Convert amount base on country rate if isConverted 
                            //1. CHeck if is converted equals true
                            if (paymentLog.isConverted)
                            {
                                //2. Get user country id
                                var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);
                                if (user == null)
                                {
                                    return result;
                                }

                                if (user.UserActiveCountryId <= 0)
                                {
                                    return result;
                                }

                                var userdestCountry = new CountryRouteZoneMap();

                                // Get conversion rate base of card type use
                                if (paymentLog.CardType == CardType.Naira)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 1 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Pound)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 62 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else if (paymentLog.CardType == CardType.Dollar)
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 207 && c.CompanyMap == CompanyMap.GIG);
                                }
                                else
                                {
                                    userdestCountry = await _uow.CountryRouteZoneMap.GetAsync(c => c.DepartureId == user.UserActiveCountryId && c.DestinationId == 76 && c.CompanyMap == CompanyMap.GIG);
                                }

                                if (userdestCountry == null)
                                {
                                    return result;
                                }

                                if (userdestCountry.Rate <= 0)
                                {
                                    return result;
                                }
                                //3. Convert base on country rate
                                var convertedAmount = Math.Round((userdestCountry.Rate * (double)paymentLog.Amount), 2);
                                paymentLog.Amount = (decimal)convertedAmount;
                            }

                            //update the wallet
                            await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                            {
                                WalletId = paymentLog.WalletId,
                                Amount = paymentLog.Amount,
                                CreditDebitType = CreditDebitType.Credit,
                                Description = "Funding made through debit card",
                                PaymentType = PaymentType.Online,
                                PaymentTypeReference = paymentLog.Reference,
                                UserId = customerId
                            }, false);

                            sendPaymentNotification = true;

                            //3. update the wallet payment log
                            paymentLog.IsWalletCredited = true;
                        }
                    }

                    paymentLog.TransactionStatus = verifyResult.Data.Status;
                    paymentLog.TransactionResponse = verifyResult.Data.TransactionStatus;
                    result = true;
                    _uow.Commit();

                    if (sendPaymentNotification)
                    {
                        await SendPaymentNotificationAsync(walletDto, paymentLog);
                    }

                    //Call Node API for subscription process
                    if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && checkAmount)
                    {
                        if (userPayload != null)
                        {
                            await _nodeService.WalletNotification(userPayload);
                        }
                    }
                }
            }
            return result;
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

        private bool ValidatePaymentValue(decimal shipmentAmount, decimal paymentAmount)
        {
            var factor = Convert.ToDecimal(Math.Pow(10, 0));
            paymentAmount = Math.Round(paymentAmount * factor) / factor;
            shipmentAmount = Math.Round(shipmentAmount * factor) / factor;

            decimal increaseShipmentPrice = shipmentAmount + 1;
            decimal decreaseShipmentPrice = shipmentAmount - 1;

            if (shipmentAmount == paymentAmount
                || increaseShipmentPrice == paymentAmount
                || decreaseShipmentPrice == paymentAmount)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Korapay Webhook


        public async Task<bool> VerifyAndValidatePaymentForWebhook(KorapayWebhookDTO webhook)
        {
            bool result = false;

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(webhook?.Data?.PaymentReference);

            var referenceCode = webhook?.Data?.PaymentReference;

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                //1. Get PaymentLog
                var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == referenceCode);

                if (paymentLog != null)
                {

                    if (paymentLog.OnlinePaymentType == OnlinePaymentType.Korapay)
                    {
                        result = await VerifyAndValidateKoraPayment(webhook);
                    }
                }
            }
            else
            {
                //1. Get PaymentLog
                var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == referenceCode);

                if (paymentLog != null)
                {
                    if (paymentLog.OnlinePaymentType == OnlinePaymentType.Korapay)
                    {
                        result = await VerifyAndValidateKoraPayment(webhook);
                    }
                }
            }

            return result;
        }

        private async Task<bool> VerifyAndValidateKoraPayment(KorapayWebhookDTO webhook)
        {
            var result = await VerifyAndValidatePayment(webhook);

            return result;
        }
        #endregion

        #region Korapay HTTP Calls

        public async Task<string> InitializeCharge(KoarapayInitializeCharge payload)
        {

            //if( string.IsNullOrEmpty(payload?.Amount) || Convert.ToDecimal(payload?.Amount) < 0)
            if( payload?.Amount == null|| payload?.Amount < 0)
            {
                throw new GenericException("Amount is invalid", $"{(int)HttpStatusCode.BadRequest}");
            }

            if (string.IsNullOrEmpty(payload.Currency))
            {
                throw new GenericException("Currency is required", $"{(int)HttpStatusCode.BadRequest}");
            }

            if (string.IsNullOrEmpty(payload.Reference))
            {
                throw new GenericException("Reference is required", $"{(int)HttpStatusCode.BadRequest}");
            }

            if (string.IsNullOrEmpty(payload.Customer.Email))
            {
                throw new GenericException("Email is required", $"{(int)HttpStatusCode.BadRequest}");
            }

            string result = string.Empty;

            using (var client = new HttpClient())
            {
                //Get Urls
                var url = ConfigurationManager.AppSettings["KorapayBaseUrl"];
                var initializeCharge = ConfigurationManager.AppSettings["KorapayInitializeCharge"];
                var secretkey = ConfigurationManager.AppSettings["KorapaySecretKey"];
                url = $"{url}{initializeCharge}";

                //setup client
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretkey}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var json = JsonConvert.SerializeObject(payload);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<KorapayInitializeChargeResponse>(resultJson);

                if(jObject != null && jObject.Data != null && !string.IsNullOrEmpty(jObject.Data.CheckoutUrl))
                {
                    result = jObject?.Data?.CheckoutUrl;
                }
                else
                {
                    throw new GenericException("Checkout url not returned successfully:");
                }
                
                return result;
            }
        }

        public async Task<KorapayQueryChargeResponse> QueryCharge(string reference)
        {
            if (string.IsNullOrEmpty(reference))
            {
                throw new GenericException("Reference is required", $"{(int)HttpStatusCode.BadRequest}");
            }

            KorapayQueryChargeResponse result = new KorapayQueryChargeResponse();

            using (var client = new HttpClient())
            {
                //Get Urls
                var url = ConfigurationManager.AppSettings["KorapayBaseUrl"];
                var queryCharge = ConfigurationManager.AppSettings["KorapayQueryCharge"];
                var secretkey = ConfigurationManager.AppSettings["KorapaySecretKey"];
                url = $"{url}{queryCharge}{reference}";

                //setup client
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretkey}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var response = await client.GetAsync(url);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<KorapayQueryChargeResponse>(resultJson);

                if (jObject != null && jObject.Data != null )
                {
                    result = jObject;
                }
                else
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                return result;
            }
        }
        #endregion

        #region Korapay Encrypt Payload

        public async Task<string> Encrypt(KorapayWebhookDTO payload)
        {
            var json = JsonConvert.SerializeObject(payload.Data).Replace("/", "\\/");
            var secretkey = ConfigurationManager.AppSettings["KorapaySecretKey"];

            return GetHMAC(json, secretkey);
        }

        private string GetHMAC(string text, string key)
        {
            key = key ?? "";

            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(hash);
            }
        }

        #endregion
    }
}
