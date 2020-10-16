using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class GroupWaybillNumber : BaseDomain, IAuditable
    {
        public int GroupWaybillNumberId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string GroupWaybillCode { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        //Destination Service centre
        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        public bool HasManifest { get; set; }
        public int DepartureServiceCentreId { get; set; }
    }

    public class MovementManifestNumber : BaseDomain, IAuditable 
    {
        public int MovementManifestNumberId { get; set; }  

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string MovementManifestCode { get; set; } 
        public int DepartureServiceCentreId { get; set; }
        public virtual ServiceCentre DepartureServiceCentre { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }
        public string DriverCode { get; set; }
        public string DestinationServiceCentreCode { get; set; } 
        public MovementStatus MovementStatus { get; set; }
        public bool IsDriverValid { get; set; }
        public bool IsDestinationServiceCentreValid { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        public string DriverUserId { get; set; }
        public string DestinationServiceCentreUserId { get; set; } 

    }
}