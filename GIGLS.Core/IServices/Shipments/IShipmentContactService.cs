using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentContactService : IServiceDependencyMarker
    {
        Task<List<ShipmentContactDTO>> GetShipmentContact(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<bool> AddOrUpdateShipmentContactAndHistory(ShipmentContactDTO shipmentContactDTO);
        Task<List<ShipmentContactHistoryDTO>> GetShipmentContactHistoryByWaybill(string waybill);
    }
}