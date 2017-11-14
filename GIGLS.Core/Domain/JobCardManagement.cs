using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class JobCardManagement : BaseDomain
    {
        public int JobCardManagementId { get; set; }
        public string SupervisorComment { get; set; }
        public string MechanicComment { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public JobCardMaintenanceStatus JobCardMaintenanceStatus { get; set; }

        public int WorkshopId { get; set; }
        public virtual Workshop Workshop { get; set; }
        
        public virtual User MechanicUser { get; set; }
        
        public virtual User MechanicSupervisor { get; set; }

        public int JobCardId { get; set; }
        public virtual JobCard JobCard { get; set; }
    }
}