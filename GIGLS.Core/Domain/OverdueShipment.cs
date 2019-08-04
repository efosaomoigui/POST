using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.CORE.Domain
{
    public class OverdueShipment : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        public OverdueShipmentStatus OverdueShipmentStatus { get; set; }

        //Who processed the collection
        [MaxLength(128)]
        public string UserId { get; set; }
    }
}
