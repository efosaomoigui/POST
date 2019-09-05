using GIGLS.Core;
using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class SpecialDomesticZonePrice : BaseDomain, IAuditable
    {
        public int SpecialDomesticZonePriceId { get; set; }

        public decimal? Weight { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        public int SpecialDomesticPackageId { get; set; }
        public virtual SpecialDomesticPackage SpecialDomesticPackage { get; set; }

        //public int UserId { get; set; }
        //public virtual User User { get; set; }

        public int CountryId { get; set; }
    }
}