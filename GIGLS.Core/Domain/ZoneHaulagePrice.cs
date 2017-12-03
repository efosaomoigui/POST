using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class ZoneHaulagePrice : BaseDomain, IAuditable
    {
        public int ZoneHaulagePriceId { get; set; }

        [ForeignKey("Haulage")]
        public int HaulageId { get; set; }
        public virtual Haulage Haulage { get; set; }

        [ForeignKey("Zone")]
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        public decimal Price { get; set; }
    }
}
