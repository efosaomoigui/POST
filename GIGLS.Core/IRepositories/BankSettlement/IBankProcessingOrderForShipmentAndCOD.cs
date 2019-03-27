using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.BankSettlement
{
    public interface IBankProcessingOrderForShipmentAndCODRepository : IRepository<BankProcessingOrderForShipmentAndCOD>
    {
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCOD(DepositType type);
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCODByRefCode(string refcode);
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetAllWaybillsForBankProcessingOrders(DepositType type);
        Task<IQueryable<BankProcessingOrderForShipmentAndCOD>> GetAllWaybillsForBankProcessingOrdersAsQueryable(DepositType type);
    }

    public interface IBankProcessingOrderCodesRepository : IRepository<BankProcessingOrderCodes> 
    {
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode(DepositType type);
        Task<List<BankProcessingOrderCodesDTO>> GetProcessingOrderCodebyRefCode(string refcode);
        IQueryable<BankProcessingOrderCodesDTO> GetBankOrderProcessingCodeAsQueryable();
    }

    
}
