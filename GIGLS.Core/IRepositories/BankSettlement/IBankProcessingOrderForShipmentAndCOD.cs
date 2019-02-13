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
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCOD();
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCODByRefCode(string refcode); 
    }

    public interface IBankProcessingOrderCodesRepository : IRepository<BankProcessingOrderCodes> 
    {
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode();
        Task<List<BankProcessingOrderCodesDTO>> GetProcessingOrderCodebyRefCode(string refcode);
        IQueryable<BankProcessingOrderCodesDTO> GetBankOrderProcessingCodeAsQueryable();
    }
}
