using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using AutoMapper;
using System.Linq;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CashOnDeliveryAccountRepository : Repository<CashOnDeliveryAccount, GIGLSContext>, ICashOnDeliveryAccountRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync()
        {
            try
            {
                var codAccounts = _context.CashOnDeliveryAccount.Include("Wallet").ToList();
                var codAccountsDto = Mapper.Map<IEnumerable<CashOnDeliveryAccountDTO>>(codAccounts);
                return Task.FromResult(codAccountsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
