using GIGLS.Core;
using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class Workshop : BaseDomain, IAuditable
    {
        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual User WorkshopSupervisor { get; set; }
    }
}