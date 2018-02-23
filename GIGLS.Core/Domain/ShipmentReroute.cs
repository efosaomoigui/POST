using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
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

        public string RerouteBy { get; set; }

        public ShipmentRerouteInitiator ShipmentRerouteInitiator { get; set; }
    }
}
