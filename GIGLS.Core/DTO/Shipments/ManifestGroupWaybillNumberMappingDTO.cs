using POST.Core.DTO.ServiceCentres;
using POST.Core.Enums;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;

namespace POST.Core.DTO.Shipments
{
    public class ManifestGroupWaybillNumberMappingDTO : BaseDomainDTO
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public ManifestDTO ManifestDetails { get; set; }
        
        public string GroupWaybillNumber { get; set; }
        public List<string> GroupWaybillNumbers { get; set; }
    }


    public class AllManifestAndGroupWaybillDTO : BaseDomainDTO
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public ServiceCentreDTO  DestinationServiceCentre { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }   
        public int  DestinationServiceCentreId { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }
        public string SuperManifestCode { get; set; }
        public SuperManifestStatus SuperManifestStatus { get; set; }
        public ManifestType ManifestType { get; set; }
        public bool IsBulky { get; set; }
        public bool HasSuperManifest { get; set; }
        public bool ExpressDelivery { get; set; }

    }

    public class AllManifestAndGroupWaybillFilterDTO
    {
      
        public string Type { get; set; }
        public string Code { get; set; }

    }
}
