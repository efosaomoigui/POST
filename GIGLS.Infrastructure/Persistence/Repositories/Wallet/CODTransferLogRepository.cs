using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Wallet;
using POST.Core.IRepositories.Wallet;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.SqlClient;
using POST.Core.Enums;
using POST.Core.DTO;
using POST.Core.Domain.Wallet;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class CODTransferLogRepository : Repository<Core.Domain.Wallet.CODTransferLog, GIGLSContext>, ICODTransferLogRepository
    {
        private GIGLSContext _context;

        public CODTransferLogRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        //public Task<IEnumerable<CODWalletDTO>> GetCODWalletsAsync()
        //{
        //    var wallets = _context.CODWallet.ToList();
        //    var walletDto = Mapper.Map<IEnumerable<CODWalletDTO>>(wallets);
        //    return Task.FromResult(walletDto);
        //}

        //public IQueryable<CODWallet> GetCODWalletsAsQueryable()
        //{
        //    var wallets = _context.CODWallet.AsQueryable();
        //    return wallets;
        //}
    }
}
