using POST.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POST.Core.Domain
{
    public class ShipmentReroute : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string WaybillNew { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string WaybillOld { get; set; }

        [MaxLength(128)]
        public string RerouteBy { get; set; }

        [MaxLength(500)]
        public string RerouteReason { get; set; }

        public ShipmentRerouteInitiator ShipmentRerouteInitiator { get; set; }
    }
}
