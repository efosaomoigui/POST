using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Partnership
{
    public class PartnerTransactions: BaseDomain, IAuditable
    {
        public int PartnerTransactionsID { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; }
        public decimal AmountReceived { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public bool IsProcessed { get; set; }
        [MaxLength(100)]
        public string Manifest { get; set; }
    }
}
