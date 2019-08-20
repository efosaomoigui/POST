using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class VehicleType : BaseDomain, IAuditable
    {
        public int VehicleTypeId { get; set; }
        public string Partnercode { get; set; }

        [MaxLength(100)]
        public string Vehicletype { get; set; }
        public string VehicleLicenseNumber { get; set; }
        public DateTime? VehicleLicenseExpiryDate { get; set; }

        public string VehicleLicenseImageDetails { get; set; }

        public string VehiclePlateNumber { get; set; }
        public string VehiceInsurancePolicyDetails { get; set; }

        public string VehiceRoadWorthinessDetails { get; set; }
        public string VehicleParticularsDetails { get; set; }
    }
}
