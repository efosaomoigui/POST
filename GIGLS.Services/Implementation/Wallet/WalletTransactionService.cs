using GIGLS.Core.IServices.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletTransactionService : IWalletTransactionService
    {
        public Task AddWalletTransaction(WalletTransactionDTO walletTransaction)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WalletTransactionDTO>> GetWalletTransaction()
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransactionDTO> GetWalletTransactionById(int walletTransactionId)
        {
            throw new NotImplementedException();
        }

        public Task<WalletTransactionSummaryDTO> GetWalletTransactionByWalletId(int walletId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveWalletTransaction(int walletTransactionId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWalletTransaction(int walletTransactionId, WalletTransactionDTO walletTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
