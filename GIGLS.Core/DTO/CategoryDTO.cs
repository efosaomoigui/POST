using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class CategoryDTO : BaseDomainDTO
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
