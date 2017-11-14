using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.JobCards
{
    public class JobCardManagementDTO : BaseDomainDTO
    {
        public int JobCardManagementId { get; set; }
        public string SupervisorComment { get; set; }
        public string MechanicComment { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public string JobCardMaintenanceStatus { get; set; }
        public string JobCard { get; set; }
        public string Workshop { get; set; }
        public string MechanicUser { get; set; }
        public string MechanicSupervisor { get; set; }
    }
}
