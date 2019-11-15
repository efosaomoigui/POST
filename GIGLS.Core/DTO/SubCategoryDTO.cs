using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class SubCategoryDTO
    {
        public int? SubCategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public int? CategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public string Weight { get; set; }

        public string WeightRange { get; set; }
    }
}
