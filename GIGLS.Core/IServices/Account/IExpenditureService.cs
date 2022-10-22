using POST.Core.DTO.Account;
using POST.Core.DTO.Expenses;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Account
{
    public interface IExpenditureService : IServiceDependencyMarker
    {
        //Report pull from General Ledger
        Task<IEnumerable<GeneralLedgerDTO>> GetExpenditures();
        Task<object> AddExpenditure(GeneralLedgerDTO generalLedger);

        //Report pull from Expenditure
        Task<object> AddExpenditure(ExpenditureDTO expenditureDto);
        Task<IEnumerable<ExpenditureDTO>> GetExpenditures(ExpenditureFilterCriteria expenditureFilterCriteria);
    }
}

