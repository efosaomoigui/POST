using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class GroupWaybillNumberDTO : BaseDomainDTO
    {
        public int GroupWaybillNumberId { get; set; }
        public string GroupWaybillCode { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }

        public int ServiceCentreId { get; set; }
        public string ServiceCentreCode { get; set; }

        public bool HasManifest { get; set; }

        //
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentre DepartureServiceCentre { get; set; }
        public ServiceCentre DestinationServiceCentre { get; set; }

        public List<string> WaybillNumbers { get; set; }
        public List<object> WaybillsWithDate { get; set; }
    }

    public class MovementManifestNumberDTO : BaseDomainDTO 
    {
        public int MovementManifestNumberId { get; set; } 
        public string MovementManifestCode { get; set; } 
        public bool IsActive { get; set; }

        public string UserId { get; set; }

        public int ServiceCentreId { get; set; }
        public string ServiceCentreCode { get; set; }

        public bool HasManifest { get; set; }
        public MovementStatus MovementStatus { get; set; }
        //
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentre DepartureServiceCentre { get; set; }
        public ServiceCentre DestinationServiceCentre { get; set; }

        public List<string> ManifestNumbers { get; set; } 
        public List<object> ManifestNumbersWithDate { get; set; } 
    }
}