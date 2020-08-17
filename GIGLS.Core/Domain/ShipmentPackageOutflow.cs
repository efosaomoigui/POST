using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ShipmentPackageOutflow : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentPackageOutflowId { get; set; }

        [MaxLength(500)]
        public string StoreKeeperId { get; set; }
        public int ServiceCenterId { get; set; }
        public int NumberShipped { get; set; }
        public int ShipmentPackageId { get; set; }
        public int StoreCenterId { get; set; }
    }
}
