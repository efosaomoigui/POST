using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.ShipmentScan
{
    public class MobileScanStatus : BaseDomain, IAuditable
    {
        public int MobileScanStatusId { get; set; }

        [StringLength(10)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        public string Incident { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
    }
}
