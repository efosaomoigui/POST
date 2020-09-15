using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class HomeDeliveryLocationDTO : BaseDomainDTO
    {
        public int HomeDeliveryLocationId { get; set; }
        public string LGAName { get; set; }
        public string LGAState { get; set; }
        public bool Status { get; set; }
        public int StateId { get; set; }
    }
}
