using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IInternationalPriceService : IServiceDependencyMarker
    {
        Task<InternationalShipmentDTO> GetPrice(InternationalShipmentDTO shipmentDTO, CompanyMap companyMap);
    }
}
