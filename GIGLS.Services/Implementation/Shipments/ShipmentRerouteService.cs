using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentRerouteService : IShipmentRerouteService
    {
        public Task<ShipmentDTO> AddRerouteShipment(ShipmentDTO shipment)
        {
            return Task.FromResult(shipment);
        }
    }
}
