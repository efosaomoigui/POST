using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.CORE.DTO.Report
{
    public class BaseFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ServiceCentreId { get; set; }
        public int StationId { get; set; }
        public int StateId { get; set; }
    }
}
