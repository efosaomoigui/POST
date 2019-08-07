using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;

namespace GIGL.GIGLS.Core.Domain
{
    public class JobCard : BaseDomain, IAuditable
    {
        public int JobCardId { get; set; }
        public string JobDescription { get; set; }
        public JobCardType JobCardType { get; set; }
        public JobCardStatus JobCardStatus { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public virtual Fleet Fleet { get; set; }
        
        public virtual User Requester { get; set; }       
        public virtual User Approver { get; set; }
    }
}