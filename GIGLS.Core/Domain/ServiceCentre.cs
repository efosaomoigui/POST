using GIGLS.Core;
using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class ServiceCentre : BaseDomain, IAuditable
    {
        public int ServiceCentreId { get; set; }

        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(100), MinLength(3)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsActive { get; set; }
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
        public decimal TargetAmount { get; set; }
        public int TargetOrder { get; set; }
        public bool IsDefault { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public bool IsHUB { get; set; }
        public bool IsGateway { get; set; }

        [MaxLength(128)]
        public string FormattedServiceCentreName { get; set; }
        public bool IsPublic { get; set; }
        public int LGAId { get; set; }
        public bool IsConsignable { get; set; }
        public string CrAccount { get; set; }

    }
}