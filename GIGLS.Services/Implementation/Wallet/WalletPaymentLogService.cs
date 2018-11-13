using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.Enums;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
        {
            if (walletPaymentLogDto.UserId == null)
            {
                walletPaymentLogDto.UserId = await _userService.GetCurrentUserId();
            }

            var walletPaymentLog = Mapper.Map<WalletPaymentLog>(walletPaymentLogDto);
            _uow.WalletPaymentLog.Add(walletPaymentLog);
            await _uow.CompleteAsync();
            return new { id = walletPaymentLog.WalletPaymentLogId };
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
            await _uow.CompleteAsync();
        }
    }

}
