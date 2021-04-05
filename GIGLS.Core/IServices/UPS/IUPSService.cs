using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.UPS;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.UPS
{
    public interface IUPSService : IServiceDependencyMarker
    {
        Task<UPSShipmentResponsePayload> CreateUPSShipment(InternationalShipmentDTO shipmentDto);
    }
}
