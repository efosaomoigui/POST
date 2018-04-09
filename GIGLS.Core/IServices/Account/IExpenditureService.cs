using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IExpenditureService : IServiceDependencyMarker
    {
        Task<IEnumerable<GeneralLedgerDTO>> GetExpenditures();
        Task<object> AddExpenditure(GeneralLedgerDTO generalLedger);
    }
}

