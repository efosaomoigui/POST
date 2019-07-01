using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class Category: BaseDomain, IAuditable
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
