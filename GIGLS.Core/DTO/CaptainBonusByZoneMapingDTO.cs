using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class CaptainBonusByZoneMapingDTO : BaseDomainDTO
    {
        public int CaptainBonusByZoneMapingId { get; set; }
        public int Zone { get; set; }
        public decimal BonusAmount { get; set; }
        public bool IsActivated { get; set; }
    }
}
