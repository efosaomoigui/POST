using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
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
        Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetAllWaybillsForBankProcessingOrdersV2(DepositType type, string refcode, int[] serviceCenters);
    }

    public interface IBankProcessingOrderCodesRepository : IRepository<BankProcessingOrderCodes> 
    {
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode(DepositType type);
        Task<List<BankProcessingOrderCodesDTO>> GetProcessingOrderCodebyRefCode(string refcode) ; 
        IQueryable<BankProcessingOrderCodesDTO> GetBankOrderProcessingCodeAsQueryable();
        Task<Shipment> GetShipmentByWaybill(string waybill);
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria);
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria, int[] serviceCenters);
        Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByServiceCenter(DepositType type, BankDepositFilterCriteria dateFilterCriteria, ServiceCentreDTO[] sc);
        Task<BankProcessingOrderCodesDTO> GetBankOrderProcessingCodeV2(DepositType type, string refcode);
    }

    public interface ICodPayOutListRepository : IRepository<CodPayOutList>
    {
        //Task<Shipment> GetShipmentByWaybill(string waybill);
    }

}
