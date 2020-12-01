using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain   //
{
    public class GroupWaybillNumberMapping : BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100), MinLength(5)]
        public string GroupWaybillNumber { get; set; }

        [MaxLength(100), MinLength(5)]
        public string WaybillNumber { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public virtual ServiceCentre DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }

        //Original Information - used for Transit Manifest Tracking
        public int OriginalDepartureServiceCentreId { get; set; }
        public virtual ServiceCentre OriginalDepartureServiceCentre { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
    }

     
    public class MovementManifestNumberMapping : BaseDomain
    {
        public int MovementManifestNumberMappingId { get; set; } 

        [MaxLength(100), MinLength(5)]
        public string MovementManifestCode { get; set; } 

        [MaxLength(100), MinLength(5)]
        public string ManifestNumber { get; set; } 

        [MaxLength(128)]
        public string UserId { get; set; }
    }
}