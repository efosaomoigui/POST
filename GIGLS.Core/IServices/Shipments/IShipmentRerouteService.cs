using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentRerouteService : IServiceDependencyMarker
    {
        Task<ShipmentDTO> AddRerouteShipment(ShipmentDTO shipment);
    }
}