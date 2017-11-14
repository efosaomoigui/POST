using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletRepository : Repository<Core.Domain.Wallet.Wallet, GIGLSContext>, IWalletRepository
    {
        private GIGLSContext _context;

        public WalletRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<WalletDTO>> GetWalletsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
