using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.CORE.DTO.Shipments
{
    public class FilterOptionsDto
    {
        public int count { get; set; }
        public int page { get; set; }
        public string sortorder { get; set; }
        public string sortvalue { get; set; }
        public string filter { get; set; }
        public string filterValue { get; set; }
    }
}
