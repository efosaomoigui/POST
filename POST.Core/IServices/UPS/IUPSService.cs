using POST.Core.DTO.DHL;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.UPS;
using System.Threading.Tasks;

namespace POST.Core.IServices.UPS
{
    public interface IUPSService : IServiceDependencyMarker
    {
        Task<InternationalShipmentWaybillDTO> CreateInternationalShipment(InternationalShipmentDTO shipmentDTO);
    }
}
