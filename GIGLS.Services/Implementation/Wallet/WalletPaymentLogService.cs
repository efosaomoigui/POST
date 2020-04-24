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

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletPaymentLogService : IWalletPaymentLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public WalletPaymentLogService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
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
        
        public async Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
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
            }

            var walletPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
            walletPaymentLog.Wallet = null;
            _uow.WalletPaymentLog.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.WalletPaymentLogId };
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
                throw new GenericException("Wallet Payment Log Information does not exist");
            }
            return Mapper.Map<WalletPaymentLogDTO>(walletPaymentLog);
        }
        
        public async Task RemoveWalletPaymentLog(int walletPaymentLogId)
        {
            var walletPaymentLog = await _uow.WalletPaymentLog.GetAsync(walletPaymentLogId);

            if (walletPaymentLog == null)
            {
                throw new GenericException("Wallet Payment Log Information does not exist");
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
                throw new GenericException($"Wallet Payment Log Information does not exist for this reference: {reference}.");
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
    }

}
