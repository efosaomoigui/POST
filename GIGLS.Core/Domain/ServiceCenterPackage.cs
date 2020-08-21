using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class ServiceCenterPackage : BaseDomain, IAuditable
    {
        [Key]
        public int ServiceCenterPackageId { get; set; }
        public int ShipmentPackageId { get; set; }
        public int ServiceCenterId { get; set; }
        public int InventoryOnHand { get; set; }
        public int MinimunRequired { get; set; }
    }

}
