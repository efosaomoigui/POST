using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.BankSettlement
{
    public interface ICODSettlementSheetRepository : IRepository<CODSettlementSheet>
    {
        Task<List<CODSettlementSheetDTO>> GetCODSettlementSheetsAsync(int[] serviceCentreIds);
    }
}
