using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Account
{
    public interface IGeneralLedgerRepository : IRepository<GeneralLedger>
    {
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(int[] serviceCentreIds);
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(CreditDebitType creditDebitType, int[] serviceCentreIds);
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(AccountFilterCriteria AccountFilterCriteria, int[] serviceCentreIds);
    }
}
