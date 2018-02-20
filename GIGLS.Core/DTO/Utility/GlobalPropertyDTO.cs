using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Utility
{
    public class GlobalPropertyDTO : BaseDomainDTO
    {
        public int GlobalPropertyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
