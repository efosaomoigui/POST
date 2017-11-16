using GIGLS.Core.Enums;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Core.IServices.Business
{
    public interface IScanService : IServiceDependencyMarker
    {
        //Task<bool> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus);
        Task<bool> ScanShipment(ScanDTO scan);
    }
}
