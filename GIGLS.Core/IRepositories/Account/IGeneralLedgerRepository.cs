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
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync();
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(CreditDebitType creditDebitType);
        Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(AccountFilterCriteria AccountFilterCriteria);
    }
}
