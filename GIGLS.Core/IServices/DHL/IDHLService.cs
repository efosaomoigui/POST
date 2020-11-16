using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.DHL
{
    public interface IDHLService : IServiceDependencyMarker
    {
        Task<TotalNetResult> GetInternationalShipmentPrice(InternationalShipmentDTO shipmentDTO);
        Task<InternationalShipmentDTO> AddInternationalShipment(InternationalShipmentDTO shipmentDTO)
    }
}
