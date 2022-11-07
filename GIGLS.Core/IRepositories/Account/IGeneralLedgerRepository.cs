using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.Enums;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IGeneralLedgerRepository : IRepository<GeneralLedger>
    {
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(int[] serviceCentreIds);
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(CreditDebitType creditDebitType, int[] serviceCentreIds);
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(AccountFilterCriteria AccountFilterCriteria, int[] serviceCentreIds);
    }
}
