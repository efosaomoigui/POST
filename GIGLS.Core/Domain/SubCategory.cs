using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class SubCategory : BaseDomain, IAuditable
    {
        public int SubCategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        [MaxLength(500)]
        public string SubCategoryName { get; set; }
        public decimal Weight { get; set; }

        [MaxLength(100)]
        public string WeightRange { get; set; }
    }
}
