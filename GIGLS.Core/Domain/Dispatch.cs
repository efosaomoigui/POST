using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class Dispatch : BaseDomain, IAuditable
    {
        [Key]
        public int DispatchId { get; set; }
        public string RegistrationNumber { get; set; }

        [MaxLength(50), MinLength(5)]
        [Index(IsUnique = true)]
        public string ManifestNumber { get; set; }

        public decimal Amount { get; set; }
        public int RescuedDispatchId { get; set; }
        public string DriverDetail { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceivedBy { get; set; }
        public DispatchCategory DispatchCategory { get; set; }

        public int? ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        public int? DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }
        
        public int? DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }

        public int DepartureServiceCenterId { get; set; }
        public int DestinationServiceCenterId { get; set; }

        public bool IsSuperManifest { get; set; }

    }

    public class MovementDispatch : BaseDomain, IAuditable 
    {
        [Key]
        public int DispatchId { get; set; }
        public string RegistrationNumber { get; set; }

        [MaxLength(50), MinLength(5)]
        [Index(IsUnique = true)]
        public string MovementManifestNumber { get; set; } 

        public decimal Amount { get; set; }
        public int RescuedDispatchId { get; set; }
        public string DriverDetail { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceivedBy { get; set; }
        public DispatchCategory DispatchCategory { get; set; }

        public int? ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        public int? DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }

        public int? DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }

        public int DepartureServiceCenterId { get; set; }
        public int DestinationServiceCenterId { get; set; }

        public bool IsSuperManifest { get; set; }
        public decimal BonusAmount { get; set; }
    }
}
