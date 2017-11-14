using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class GroupWaybillNumberMonitor : BaseDomain
    {
        public int GroupWaybillNumberMonitorId { get; set; }
        public string ServiceCentreCode { get; set; }
        public string Code { get; set; }
    }
}
