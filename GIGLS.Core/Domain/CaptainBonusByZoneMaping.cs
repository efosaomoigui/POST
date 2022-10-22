using System.ComponentModel.DataAnnotations;

namespace POST.Core.Domain
{
    public class CaptainBonusByZoneMaping : BaseDomain
    {
        [Key]
        public int CaptainBonusByZoneMapingId { get; set; }
        // drop down of zones
        public int Zone { get; set; }
        public decimal BonusAmount { get; set; }
        public bool IsActivated { get; set; }
    }
}
