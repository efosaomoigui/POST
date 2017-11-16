using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Account
{
    public class VATDTO : BaseDomainDTO
    {
        public int VATId { get; set; }
        public string Name { get; set; }
        public VATType Type { get; set; }
        public decimal Value { get; set; }
    }
}
