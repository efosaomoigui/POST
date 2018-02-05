using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class ShipmentCancel : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime ShipmentCreatedDate { get; set; }
        public string CancelledBy { get; set; }
    }
}
