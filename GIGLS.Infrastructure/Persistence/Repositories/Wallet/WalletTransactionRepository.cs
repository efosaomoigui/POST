using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.Entity;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletTransactionRepository : Repository<WalletTransaction, GIGLSContext>, IWalletTransactionRepository
    {
        private GIGLSContext _context;

        public WalletTransactionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionAsync(int[] serviceCentreIds)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }
            ////

            var walletTransactions = walletTransactionContext.Include(s => s.ServiceCentre).ToList();
            var walletTransactionDTO = Mapper.Map<IEnumerable<WalletTransactionDTO>>(walletTransactions);
            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }
    }
}
