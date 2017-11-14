using GIGLS.Core;
using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class DomesticZonePrice : BaseDomain, IAuditable
    {
        public int DomesticZonePriceId { get; set; }
        public decimal Weight { get; set; }
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        public decimal Price { get; set; }
    }
}