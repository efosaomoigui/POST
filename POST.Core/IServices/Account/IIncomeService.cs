using POST.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Account
{
    public interface IIncomeService : IServiceDependencyMarker
    {
        Task<IEnumerable<GeneralLedgerDTO>> GetAllIncome();
    }
}

