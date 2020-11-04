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
            try
            {
                var wallets = _context.Wallets.ToList();

                var walletDto = Mapper.Map<IEnumerable<WalletDTO>>(wallets);
                return Task.FromResult(walletDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<Core.Domain.Wallet.Wallet> GetWalletsAsQueryable()
        {
            var wallets = _context.Wallets.AsQueryable();
            return wallets;
        }

        public async Task<decimal> GetTotalWalletBalance(int ActiveCountryId)
        {
            try
            {
                SqlParameter countryId = new SqlParameter("@CountryId", ActiveCountryId);

                SqlParameter[] param = new SqlParameter[]
                {
                    countryId
                };

                decimal summary = 0.00M;
                var summaryResult = await _context.Database.SqlQuery<decimal?>("WalletBalance " +
                   "@CountryId",
                   param).FirstOrDefaultAsync();

                if(summaryResult != null)
                {
                    summary = (decimal)summaryResult;
                }

                return await Task.FromResult(summary);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
