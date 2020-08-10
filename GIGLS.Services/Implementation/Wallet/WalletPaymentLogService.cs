using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayStack.Net;
using System.Configuration;
using GIGLS.CORE.DTO.Report;
using System.Net;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.OnlinePayment;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletPaymentLogService : IWalletPaymentLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IPaystackPaymentService _paystackPaymentService;
        private readonly IUssdService _ussdService;

        public WalletPaymentLogService(IUserService userService, IUnitOfWork uow, IPaystackPaymentService paystackPaymentService, IUssdService ussdService)
        {
            _userService = userService;
            _paystackPaymentService = paystackPaymentService;
            _ussdService = ussdService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<WalletPaymentLogDTO>> GetWalletPaymentLogs()
        {
            var walletPaymentLog = await _uow.WalletPaymentLog.GetWalletPaymentLogs();
            var walletPaymentLogDto = Mapper.Map<IEnumerable<WalletPaymentLogDTO>>(walletPaymentLog);
            return walletPaymentLogDto;
        }

        public Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto)
        {
            var walletPaymentLogView = _uow.WalletPaymentLog.GetWalletPaymentLogs(filterOptionsDto);
            return walletPaymentLogView;
        }

        public Task<List<WalletPaymentLogView>> GetWalletPaymentLogs(DateFilterCriteria baseFilter)
        {
            var walletPaymentLogView = _uow.WalletPaymentLog.GetFromWalletPaymentLogView(baseFilter);
            return walletPaymentLogView;
        }

        public Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogViewBySearchParameter(string searchItem)
        {
            var walletPaymentLogView = _uow.WalletPaymentLog.GetFromWalletPaymentLogViewBySearchParameter(searchItem);
            return walletPaymentLogView;
        }

        public async Task<object> AddWalletPaymentLogOld(WalletPaymentLogDTO walletPaymentLogDto)
        {
            if (walletPaymentLogDto.UserId == null)
            {
                walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();
            }

            //Get the Customer Activity country
            if(walletPaymentLogDto.PaymentCountryId == 0)
            {
                //use the current user id to get the country of the user
                var user = await _uow.User.GetUserById(walletPaymentLogDto.UserId);
                walletPaymentLogDto.PaymentCountryId = user.UserActiveCountryId;

                //set Nigeria as default country if no country assign for the customer
                if(walletPaymentLogDto.PaymentCountryId == 0)
                {
                    walletPaymentLogDto.PaymentCountryId = 1;
                }
            }

            var walletPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
            walletPaymentLog.Wallet = null;
            _uow.WalletPaymentLog.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.WalletPaymentLogId };
        }

        public async Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
        {
            walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();

            //Get the Customer Activity country
            var user = await _uow.User.GetUserById(walletPaymentLogDto.UserId);
            walletPaymentLogDto.PaymentCountryId = user.UserActiveCountryId;

            //set Nigeria as default country if no country assign for the customer
            if (walletPaymentLogDto.PaymentCountryId == 0)
            {
                walletPaymentLogDto.PaymentCountryId = 1;
            }
                       
            var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == user.UserChannelCode);
            if (wallet != null)
            {
                walletPaymentLogDto.WalletId = wallet.WalletId;
            }

            var walletPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
            walletPaymentLog.Wallet = null;
            _uow.WalletPaymentLog.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.WalletPaymentLogId };
        }

        public async Task<USSDResponse> InitiatePaymentUsingUSSD(WalletPaymentLogDTO walletPaymentLogDto)
        {
            walletPaymentLogDto.OnlinePaymentType = OnlinePaymentType.USSD;

            //1. Get the country of the user
            walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();

            //Get the Customer Activity country
            var user = await _uow.User.GetUserById(walletPaymentLogDto.UserId);
            walletPaymentLogDto.PaymentCountryId = user.UserActiveCountryId;

            //set Nigeria as default country if no country assign for the customer
            if (walletPaymentLogDto.PaymentCountryId == 0)
            {
                walletPaymentLogDto.PaymentCountryId = 1;
            }

            if (string.IsNullOrWhiteSpace(walletPaymentLogDto.PhoneNumber))
            {
                walletPaymentLogDto.PhoneNumber = user.PhoneNumber;
            }

            //3. Send reguest to Oga USSD 
            var ussdResponse = await ProcessPaymentForUSSD(walletPaymentLogDto);

            if (ussdResponse.Status == "success")
            {
                if(ussdResponse.data != null)
                {
                    walletPaymentLogDto.ExternalReference = ussdResponse.data.Order_Reference;
                }

                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (wallet != null)
                {
                    walletPaymentLogDto.WalletId = wallet.WalletId;
                }

                //3. Add record to waybill payment log with the order id
                var newPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
                newPaymentLog.Wallet = null;
                _uow.WalletPaymentLog.Add(newPaymentLog);
                await _uow.CompleteAsync();

                //4. Send SMS to the customer phone number
            }

            return ussdResponse;
        }

        public async Task PaystackPaymentService(WalletPaymentLogDTO WalletPaymentInfo)
        {
            var result = AddWalletPaymentLog(WalletPaymentInfo);

            var LiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(LiveSecret);

            // Initializing a transaction
            var response = api.Transactions.Initialize(WalletPaymentInfo.Email, WalletPaymentInfo.PaystackAmount);

            // Verifying a transaction
            var verifyResponse = api.Transactions.Verify(WalletPaymentInfo.Reference); // auto or supplied when initializing;
            
            /* 
                You can save the details from the json object returned above so that the authorization code 
                can be used for charging subsequent transactions

                // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
                // Save 'authCode' for future charges!
            */
            if (verifyResponse.Status)
            {
                await UpdateWalletPaymentLog(WalletPaymentInfo.Reference, WalletPaymentInfo);
            }

            //return response.Status;

        }

        public async Task<WalletPaymentLogDTO> GetWalletPaymentLogById(int walletPaymentLogId)
        {
            var walletPaymentLog = await _uow.WalletPaymentLog.GetAsync(walletPaymentLogId);

            if (walletPaymentLog == null)
            {
                throw new GenericException("Wallet Payment Log Information does not exist", $"{(int)HttpStatusCode.NotFound}");
            }
            return Mapper.Map<WalletPaymentLogDTO>(walletPaymentLog);
        }
        
        public async Task RemoveWalletPaymentLog(int walletPaymentLogId)
        {
            var walletPaymentLog = await _uow.WalletPaymentLog.GetAsync(walletPaymentLogId);

            if (walletPaymentLog == null)
            {
                throw new GenericException("Wallet Payment Log Information does not exist", $"{(int)HttpStatusCode.NotFound}");
            }
            _uow.WalletPaymentLog.Remove(walletPaymentLog);
            await _uow.CompleteAsync();
        }

        public async Task UpdateWalletPaymentLog(string reference, WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLogList = await _uow.WalletPaymentLog.FindAsync(s => s.Reference == reference);
            var walletPaymentLog = walletPaymentLogList.FirstOrDefault();

            if (walletPaymentLog == null)
            {
                throw new GenericException($"Wallet Payment Log Information does not exist for this reference: {reference}.", $"{(int)HttpStatusCode.NotFound}");
            }

            walletPaymentLog.IsWalletCredited = walletPaymentLogDto.IsWalletCredited;
            walletPaymentLog.TransactionStatus = walletPaymentLogDto.TransactionStatus;
            walletPaymentLogDto.TransactionResponse = walletPaymentLogDto.TransactionResponse;
            await _uow.CompleteAsync();
        }
        
        public async Task AddWalletPaymentLogMobile(WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
            walletPaymentLog.Wallet = null;
            _uow.WalletPaymentLog.Add(walletPaymentLog);
            await _uow.CompleteAsync();
        }

        private async Task<USSDResponse> ProcessPaymentForUSSD(WalletPaymentLogDTO walletPaymentLogDto)
        {
            try
            {
                string countryCode = string.Empty;

                var country = await _uow.Country.GetAsync(x => x.CountryId == walletPaymentLogDto.PaymentCountryId);
                if (country != null)
                {
                    countryCode = country.CurrencyCode.Length <= 2 ? country.CurrencyCode : country.CurrencyCode.Substring(0, 2);
                }

                var ussdData = new USSDDTO
                {
                    amount = (int)walletPaymentLogDto.Amount,
                    msisdn = walletPaymentLogDto.PhoneNumber,
                    desc = walletPaymentLogDto.UserId,
                    reference = walletPaymentLogDto.Reference,
                    country_code = countryCode,
                    gateway_code = walletPaymentLogDto.GatewayCode
                };

                var responseResult = await _ussdService.ProcessPaymentForUSSD(ussdData);
                return responseResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaymentResponse> VerifyAndValidatePayment(string referenceCode)
        {
            PaymentResponse result = new PaymentResponse();

            //1. Get PaymentLog
            var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == referenceCode);

            if (paymentLog != null)
            {
                if (paymentLog.OnlinePaymentType == OnlinePaymentType.USSD)
                {
                    result = await VerifyAndValidateUSSDPayment(referenceCode);
                }
                else
                {
                    result = await _paystackPaymentService.VerifyAndProcessPayment(referenceCode);
                }
            }
            else
            {
                result.Result = false;
                result.Message = "";
                result.GatewayResponse = "Wallet Payment Log Information does not exist";
            }

            return result;
        }

        private async Task<PaymentResponse> VerifyAndValidateUSSDPayment(string referenceCode)
        {
            PaymentResponse response = new PaymentResponse();

            var result = await _ussdService.VerifyAndValidatePayment(referenceCode);

            response.Result = result.Status;
            response.Status = result.data.Status;
            response.Message = result.Message;
            response.GatewayResponse = result.data.Gateway_Response;
            return response;
        }
    }
}
