using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Haulage
{
    public class PackingListDTO : BaseDomainDTO
    {
        public int PackingListId { get; set; }
        public string Waybill { get; set; }
        public string Items { get; set; }
    }
}
