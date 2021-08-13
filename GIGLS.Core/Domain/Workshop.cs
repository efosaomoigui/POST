using GIGLS.Core;
using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class Workshop : BaseDomain, IAuditable
    {
        public int WorkshopId { get; set; }

        [MaxLength(100)]
        public string WorkshopName { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }
        public virtual User WorkshopSupervisor { get; set; }
    }
}