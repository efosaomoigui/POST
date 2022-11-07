using POST.Core.IServices;
using POST.Core.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.Enums;

namespace POST.Core.IServices.Account
{
    public interface IGeneralLedgerService : IServiceDependencyMarker
    {
        Task<IEnumerable<GeneralLedgerDTO>> GetGeneralLedgers();
        Task<IEnumerable<GeneralLedgerDTO>> GetGeneralLedgersAsync(CreditDebitType creditDebitType);
        Task<GeneralLedgerDTO> GetGeneralLedgerById(int generalLedgerId);
        Task<object> AddGeneralLedger(GeneralLedgerDTO generalLedger);
        Task UpdateGeneralLedger(int generalLedgerId, GeneralLedgerDTO generalLedger);
        Task RemoveGeneralLedger(int generalLedgerId);
    }
}
