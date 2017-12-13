using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CashOnDeliveryBalanceRepository : Repository<CashOnDeliveryBalance, GIGLSContext>, ICashOnDeliveryBalanceRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryBalanceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalanceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
