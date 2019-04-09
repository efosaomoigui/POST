using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IPreShipmentMobileService : IServiceDependencyMarker
    {

        Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<List<PreShipmentMobileDTO>> GetShipments(BaseFilterCriteria filterOptionsDto);
        Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill);
        Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser();
    }
}
