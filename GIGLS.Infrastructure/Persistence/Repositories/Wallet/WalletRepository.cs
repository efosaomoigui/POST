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

        public Task<List<WalletDTO>> GetOutstaningCorporatePayments()
        {
            try
            {
                var outstandingPayments = _context.Wallets.Where(x => x.CompanyType == CompanyType.Corporate.ToString() && x.Balance < 0);
                var companiesDto = from w in outstandingPayments
                                   join c in _context.Company on w.CustomerCode equals c.CustomerCode
                                   join co in _context.Country on c.UserActiveCountryId equals co.CountryId
                                   select new WalletDTO
                                   {
                                       CustomerCode = w.CustomerCode,
                                       Balance = w.Balance,
                                       CustomerName = c.Name,
                                       Country = new CountryDTO
                                       {
                                           CountryId = co.CountryId,
                                           CountryName = co.CountryName,
                                           CurrencySymbol = co.CurrencySymbol,
                                           CurrencyCode = co.CurrencyCode,
                                           PhoneNumberCode = co.PhoneNumberCode
                                       },
                                   };
                return Task.FromResult(companiesDto.OrderBy(x => x.Balance).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
