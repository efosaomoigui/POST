using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class GiglgoStation: BaseDomain, IAuditable
    {
        public int GiglgoStationId { get; set; }
        public int StationId { get; set; }
        public string StateName { get; set; }
        public string StationCode { get; set; }

        public bool IsActive { get; set; }

    }
}
