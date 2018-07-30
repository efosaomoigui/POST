using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Shipments
{
    public interface IShipmentReturnService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentReturnDTO>> GetShipmentReturns();
        System.Tuple<Task<List<ShipmentReturnDTO>>, int> GetShipmentReturns(FilterOptionsDto filterOptionsDto);
        Task<ShipmentReturnDTO> GetShipmentReturnById(string waybill);
        Task AddShipmentReturn(ShipmentReturnDTO shipmentReturn);
        Task AddShipmentReturn(string waybill);
        Task UpdateShipmentReturn(ShipmentReturnDTO shipmentReturn);
        Task RemoveShipmentReturn(string waybill);
    }
}
