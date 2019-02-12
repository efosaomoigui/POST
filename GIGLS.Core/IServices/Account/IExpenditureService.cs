using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Expenses;
using GIGLS.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
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

