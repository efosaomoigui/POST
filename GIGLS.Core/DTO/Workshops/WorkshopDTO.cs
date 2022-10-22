using POST.CORE.DTO;
using System;

namespace POST.Core.DTO.Workshops
{
    public class WorkshopDTO : BaseDomainDTO
    {
        public int WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string WorkshopSupervisor { get; set; }
    }
}
