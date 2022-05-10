using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.SqlClient;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO;
using GIGLS.Core.Domain.Wallet;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
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
