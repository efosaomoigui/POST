using POST.CORE.DTO;

namespace POST.Core.DTO
{
    public class CaptainBonusByZoneMapingDTO : BaseDomainDTO
    {
        public int CaptainBonusByZoneMapingId { get; set; }
        public int Zone { get; set; }
        public decimal BonusAmount { get; set; }
        public bool IsActivated { get; set; }
    }
}
