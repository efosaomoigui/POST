using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using GIGLS.Core.Domain.Wallet;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletPaymentLogRepository : Repository<WalletPaymentLog, GIGLSContext>, IWalletPaymentLogRepository
    {
        public WalletPaymentLogRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<WalletPaymentLogDTO>> GetWalletPaymentLogs()
        {
            try
            {
                var walletPaymentLogs = Context.WalletPaymentLog;

                var walletPaymentLogsDTO = from w in walletPaymentLogs
                                           select new WalletPaymentLogDTO
                                           {
                                               WalletPaymentLogId = w.WalletPaymentLogId,
                                               WalletId = w.WalletId,
                                               Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                               {
                                                   WalletId = x.WalletId,
                                                   Balance = x.Balance,
                                                   CustomerCode = x.CustomerCode,
                                                   WalletNumber = x.WalletNumber,
                                                   CustomerType = x.CustomerType,
                                               }).FirstOrDefault(),
                                               Amount = w.Amount,
                                               TransactionStatus = w.TransactionStatus,
                                               UserId = w.UserId,
                                               DateCreated = w.DateCreated,
                                               DateModified = w.DateModified
                                           };
                return Task.FromResult(walletPaymentLogsDTO.OrderBy(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
