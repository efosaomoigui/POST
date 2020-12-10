using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ShipmentContact : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentContactId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        public ShipmentContactStatus Status { get; set; }

        [MaxLength(128)]
        public string ContactedBy { get; set; }

        public int NoOfContact { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

    }
}
