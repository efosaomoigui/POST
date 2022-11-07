using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.Domain.BankSettlement;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.BankSettlement;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.BankSettlement
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
