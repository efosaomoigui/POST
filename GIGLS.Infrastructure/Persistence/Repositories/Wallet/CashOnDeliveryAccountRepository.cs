using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CashOnDeliveryAccountRepository : Repository<CashOnDeliveryAccount, GIGLSContext>, ICashOnDeliveryAccountRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryAccountRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
