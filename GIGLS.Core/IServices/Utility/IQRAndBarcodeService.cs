using POST.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace POST.Core.IServices.Utility
{
    public interface IQRAndBarcodeService : IServiceDependencyMarker
    {
        Task<PreShipmentMobileThirdPartyDTO> AddImage(string waybill);
    }
}
