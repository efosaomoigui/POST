using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletTransactionRepository : Repository<WalletTransaction, GIGLSContext>, IWalletTransactionRepository
    {
        public WalletTransactionRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
