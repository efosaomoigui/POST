using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Shipments
{
    public interface IShipmentContactService : IServiceDependencyMarker
    {
        Task<List<ShipmentContactDTO>> GetShipmentContact(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<bool> AddOrUpdateShipmentContactAndHistory(ShipmentContactDTO shipmentContactDTO);
        Task<List<ShipmentContactHistoryDTO>> GetShipmentContactHistoryByWaybill(string waybill);
    }
}