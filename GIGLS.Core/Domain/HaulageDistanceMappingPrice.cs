using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class HaulageDistanceMappingPrice : BaseDomain, IAuditable
    {
        public int HaulageDistanceMappingPriceId { get; set; }

        public int StartRange { get; set; }
        public int EndRange { get; set; }

        [ForeignKey("Haulage")]
        public int HaulageId { get; set; }
        public virtual Haulage Haulage { get; set; }

        public decimal Price { get; set; }

        public int CountryId { get; set; }
    }
}
