using POST.Core;
using POST.Core.Domain;
using POST.Core.Enums;

namespace GIGL.POST.Core.Domain
{
    public class DomesticZonePrice : BaseDomain, IAuditable
    {
        public int DomesticZonePriceId { get; set; }
        public decimal Weight { get; set; }
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        public decimal Price { get; set; }
        public RegularEcommerceType RegularEcommerceType { get; set; }

        public int CountryId { get; set; }
    }
}