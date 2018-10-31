using GIGLS.Core;
using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class ServiceCentre : BaseDomain, IAuditable
    {
        public int ServiceCentreId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(100), MinLength(3)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
        public decimal TargetAmount { get; set; }
        public int TargetOrder { get; set; }
        public bool IsDefault { get; set; }
    }
}