using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Partnership
{
    public class VehicleTypeDTO: BaseDomainDTO
    {
        public int VehicleTypeId { get; set; }
        public string Partnercode { get; set; }

        public string Vehicletype { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhoneNumber { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public string PartnerEmail { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
        public DateTime ActivityDate { get; set; }
        public PartnerType PartnerType { get; set; }
        public FleetPartnerDTO EnterprisePartner { get; set; }

        public string VehiclePlateNumber { get; set; }
        public string VehicleInsurancePolicyDetails { get; set; }
        public string VehicleRoadWorthinessDetails { get; set; }
        public string VehicleParticularsDetails { get; set; }
        public bool IsVerified { get; set; }
        public bool Contacted { get; set; }
    }
}
