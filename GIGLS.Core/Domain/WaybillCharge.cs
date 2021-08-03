using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class WaybillCharge : BaseDomain, IAuditable
    {
        [Key]
        public int WaybillChargeId { get; set; }
        [MaxLength(128)]
        public string Waybill { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
    }

   
}
