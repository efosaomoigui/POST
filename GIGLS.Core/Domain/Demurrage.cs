using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Demurrage : BaseDomain, IAuditable
    {
        public int DemurrageId { get; set; }
        public int DayCount { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(100), MinLength(5)]
        public string WaybillNumber { get; set; }
        public decimal AmountPaid { get; set; }

        //who approved the amount of demurrage
        public string ApprovedBy { get; set; }
        public string ApprovedId { get; set; }

        //who processed the release
        [MaxLength(128)]
        public string UserId { get; set; }
        public int ServiceCenterId { get; set; }

        [MaxLength(50)]
        public string ServiceCenterCode { get; set; }

        public int CountryId { get; set; }
    }
}