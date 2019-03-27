using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using AutoMapper;
using System.Linq;
using GIGLS.Core.Enums;

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
                codAccounts = codAccounts.OrderByDescending(x => x.DateCreated).ToList();
                var codAccountsDto = Mapper.Map<IEnumerable<CashOnDeliveryAccountDTO>>(codAccounts);
                return Task.FromResult(codAccountsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync(CODStatus codStatus)
        {
            try
            {
                var codAccounts = _context.CashOnDeliveryAccount.Include("Wallet").Where(s => s.CODStatus == codStatus).ToList();
                var codAccountsDto = Mapper.Map<IEnumerable<CashOnDeliveryAccountDTO>>(codAccounts);
                return Task.FromResult(codAccountsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<CashOnDeliveryAccount> GetCODCollectedAsQueryable()  
        {
                var codAccounts = _context.CashOnDeliveryAccount.AsQueryable();
                return codAccounts;
        }

    }

    public class CashOnDeliveryRegisterAccountRepository : Repository<CashOnDeliveryRegisterAccount, GIGLSContext>, ICashOnDeliveryRegisterAccountRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryRegisterAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<CashOnDeliveryRegisterAccount> GetCODAsQueryable()
        {
            var codAccounts = _context.CashOnDeliveryRegisterAccount.AsQueryable();
            return codAccounts;
        }

    }


    public class DemurrageRegisterAccountRepository : Repository<DemurrageRegisterAccount, GIGLSContext>, IDemurrageRegisterAccountRepository
    {
        private GIGLSContext _context;

        public DemurrageRegisterAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DemurrageRegisterAccount> GetDemurrageAsQueryable()
        {
            var DemurrageAccounts = _context.DemurrageRegisterAccount.AsQueryable();
            return DemurrageAccounts;
        } 

    }
}
