﻿namespace GIGLS.Core.Domain
{
    public class SubCategory : BaseDomain, IAuditable
    {
        public int SubCategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public decimal Weight { get; set; }
        public string WeightRange { get; set; }
    }
}
