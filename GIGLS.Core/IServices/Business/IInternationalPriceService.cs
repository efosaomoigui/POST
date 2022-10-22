using POST.Core.DTO.Shipments;
using POST.Core.Enums;
using System.Threading.Tasks;

namespace POST.Core.IServices.Business
{
    public interface IInternationalPriceService : IServiceDependencyMarker
    {
        Task<InternationalShipmentDTO> GetPrice(InternationalShipmentDTO shipmentDTO, CompanyMap companyMap);
    }
}
