using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.UPS;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.UPS
{
    public interface IUPSService : IServiceDependencyMarker
    {
        Task<InternationalShipmentWaybillDTO> CreateInternationalShipment(InternationalShipmentDTO shipmentDTO);
    }
}
