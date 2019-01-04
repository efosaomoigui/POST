using GIGLS.Core;
using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.CORE.Domain
{
    public class ShipmentReturn : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string WaybillNew { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string WaybillOld { get; set; }

        public decimal Discount { get; set; }
        public decimal OriginalPayment { get; set; }
        public int ServiceCentreId { get; set; }
    }
}
