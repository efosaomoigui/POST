using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.JobCards
{
    public class JobCardDTO : BaseDomainDTO
    {
        public int JobCardId { get; set; }
        public string JobDescription { get; set; }
        public JobCardType JobCardType { get; set; }
        public JobCardStatus JobCardStatus { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public string FleetNumber { get; set; }
        public string Requester { get; set; }
        public string Approver { get; set; }
    }
}
