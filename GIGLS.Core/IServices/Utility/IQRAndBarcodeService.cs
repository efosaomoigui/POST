using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Utility
{
    public interface IQRAndBarcodeService : IServiceDependencyMarker
    {
        Task<PreShipmentMobileThirdPartyDTO> AddImage(string waybill);
    }
}
