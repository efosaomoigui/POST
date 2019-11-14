using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class SubCategory : BaseDomain, IAuditable
    {
        public int SubCategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public string  Weight { get; set; }
    }
}
