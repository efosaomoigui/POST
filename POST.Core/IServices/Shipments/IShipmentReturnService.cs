using POST.Core.IServices;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Shipments
{
    public interface IShipmentReturnService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentReturnDTO>> GetShipmentReturns();
        Task<System.Tuple<List<ShipmentReturnDTO>, int>> GetShipmentReturns(FilterOptionsDto filterOptionsDto);
        Task<ShipmentReturnDTO> GetShipmentReturnById(string waybill);
        Task AddShipmentReturn(ShipmentReturnDTO shipmentReturn);
        Task<string> AddShipmentReturn(string waybill);
        Task UpdateShipmentReturn(ShipmentReturnDTO shipmentReturn);
        Task RemoveShipmentReturn(string waybill);
    }
}
