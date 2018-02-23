using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentRerouteService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentRerouteDTO>> GetRerouteShipments();
        Task<ShipmentDTO> AddRerouteShipment(ShipmentDTO shipment);
        Task<ShipmentRerouteDTO> GetRerouteShipment(string waybill);        
        Task UpdateRerouteShipment(ShipmentRerouteDTO rerouteDto);
        Task DeleteRerouteShipment(string waybill);
    }
}