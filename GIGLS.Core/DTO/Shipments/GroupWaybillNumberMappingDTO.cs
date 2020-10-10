using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using System;
using System.Collections.Generic;

namespace GIGLS.CORE.DTO.Shipments
{
    public class GroupWaybillNumberMappingDTO : BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }

        public List<string> WaybillNumbers { get; set; }

        public List<ShipmentDTO> Shipments { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }

        //Original Information - used for Transit Manifest Tracking
        public int OriginalDepartureServiceCentreId { get; set; }
        public virtual ServiceCentre OriginalDepartureServiceCentre { get; set; }
    }

    public class MovementManifestNumberMappingDTO : BaseDomain 
    {
        public int MovementManifestNumberMappingId { get; set; } 
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string MovementManifestCode { get; set; } 
        public string ManifestNumber { get; set; } 

        public List<string> ManifestNumbers { get; set; } 

        public List<ShipmentDTO> Shipments { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }

        //Original Information - used for Transit Manifest Tracking
        public int OriginalDepartureServiceCentreId { get; set; }
        public virtual ServiceCentre OriginalDepartureServiceCentre { get; set; }
    }
}