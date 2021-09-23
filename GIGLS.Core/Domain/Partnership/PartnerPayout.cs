using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Partnership
{
    public class PartnerPayout : BaseDomain, IAuditable
    {
        public int PartnerPayoutId { get; set; }
        
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string ProcessedBy { get; set; }

        public DateTime DateProcessed { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PartnerType PartnerType { get; set; }
    }
}
