using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class GroupWaybillNumberDTO : BaseDomainDTO
    {
        public int GroupWaybillNumberId { get; set; }
        public string GroupWaybillCode { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual string ServiceCentreCode { get; set; }
    }
}
