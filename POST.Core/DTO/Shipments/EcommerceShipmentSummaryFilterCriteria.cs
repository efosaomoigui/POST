using POST.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Shipments
{
    public class EcommerceShipmentSummaryFilterCriteria : BaseFilterCriteria
    {
        public string CompanyType { get; set; }
        public bool IsAgility { get; set; }
        public bool IsMobile { get; set; }
    }
}
