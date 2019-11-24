using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class VehicleType : BaseDomain, IAuditable
    {
        public int VehicleTypeId { get; set; }

        [MaxLength(100)]
        public string Partnercode { get; set; }

        [MaxLength(100)]
        public string Vehicletype { get; set; }

        [MaxLength(100)]
        public string VehiclePlateNumber { get; set; }
        public string VehicleInsurancePolicyDetails { get; set; }

        public string VehicleRoadWorthinessDetails { get; set; }
        public string VehicleParticularsDetails { get; set; }

        public bool IsVerified { get; set; }
    }
}
