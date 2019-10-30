using GIGLS.Core.DTO.BankSettlement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.BankSettlement
{
    public interface IBankService : IServiceDependencyMarker
    {
        Task<IEnumerable<BankDTO>> GetBanks();
        Task<BankDTO> GetBankById(int bankId);
        Task<object> AddBank(BankDTO bankDTO);
        Task UpdateBank(int bankId, BankDTO bankDTO);
        Task DeleteBank(int bankId);
    }
}
