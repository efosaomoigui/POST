using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.ShipmentScan
{
    public class ScanStatus : BaseDomain, IAuditable
    {
        public int ScanStatusId { get; set; }

        [StringLength(10)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        public string Incident { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public bool HiddenFlag { get; set; }
    }
}
