using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class TransitWaybillNumberDTO : BaseDomainDTO
    {
        public int TransitWaybillNumberId { get; set; }
        public string WaybillNumber { get; set; }

        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }

        public string UserId { get; set; }
        public bool IsGrouped { get; set; }
    }
}
