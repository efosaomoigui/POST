using POST.Core;
using POST.Core.Domain;
using POST.Core.Enums;

namespace GIGL.POST.Core.Domain
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