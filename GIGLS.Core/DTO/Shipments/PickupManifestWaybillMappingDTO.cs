using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class PickupManifestWaybillMappingDTO : BaseDomainDTO
    {
        public int PickupManifestWaybillMappingId { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public PickupManifestDTO PickupManifestDetails { get; set; }

        public string Waybill { get; set; }
        public PreShipmentMobileDTO PreShipment { get; set; }

        public List<string> Waybills { get; set; }

        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }
    }
}
