namespace GIGLS.Core.DTO
{
    public class SubCategoryDTO
    {
        public int? SubCategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public int? CategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public decimal? Weight { get; set; }

        public string WeightRange { get; set; }
    }
}
