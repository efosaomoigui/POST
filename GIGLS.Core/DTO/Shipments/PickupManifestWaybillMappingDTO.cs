using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class PickupManifestWaybillMappingDTO : BaseDomainDTO
    {
        public int ManifestWaybillMappingId { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public PickupManifestDTO PickupManifestDetails { get; set; }

        public string Waybill { get; set; }
        public ShipmentDTO Shipment { get; set; }

        public List<string> Waybills { get; set; }

        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }

        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public string ScanDescription { get; set; }

        public string DispatchRider { get; set; }
    }
}
