using GIGL.POST.Core.Repositories;
using POST.Core.Domain.BankSettlement;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.BankSettlement;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.BankSettlement
{
    public interface ICODSettlementSheetRepository : IRepository<CODSettlementSheet>
    {
        Task<List<CODSettlementSheetDTO>> GetCODSettlementSheetsAsync(int[] serviceCentreIds);
    }
}
