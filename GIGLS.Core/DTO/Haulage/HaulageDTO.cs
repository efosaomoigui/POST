using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Haulage
{
    public class HaulageDTO : BaseDomainDTO
    {
        public int HaulageId { get; set; }
        public decimal Tonne { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public decimal FixedRate { get; set; }
        public decimal AdditionalRate { get; set; }
    }
   
}