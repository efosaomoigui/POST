using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class JobCardManagementPart : BaseDomain
    {
        public int JobCardManagementPartId { get; set; }
        public int Quantity { get; set; }

        public int PartId { get; set; }
        public virtual FleetPart FleetPart { get; set; }

        public int JobCardManagementId { get; set; }
        public JobCardManagement JobCardManagement { get; set; }
    }
}