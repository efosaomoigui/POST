using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class Location : BaseDomain, IAuditable
    {
        public int LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
