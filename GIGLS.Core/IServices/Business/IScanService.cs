using POST.Core.Enums;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;

namespace POST.Core.IServices.Business
{
    public interface IScanService : IServiceDependencyMarker
    {
        //Task<bool> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus);
        Task<bool> ScanShipment(ScanDTO scan);
        Task<bool> ScanMultipleShipment(List<ScanDTO> scanList);
        Task<bool> ScanSignOffDeliveryManifest(string manifest);
        Task ItemShippedFromUKScan(string manifestCode);
    }
}
