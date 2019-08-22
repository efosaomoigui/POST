using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class VehicleTypeDTO
    {
        public int VehicleTypeId { get; set; }
        public string Partnercode { get; set; }

        public string Vehicletype { get; set; }

        public string VehiclePlateNumber { get; set; }
        public string VehiceInsurancePolicyDetails { get; set; }
        public string VehiceRoadWorthinessDetails { get; set; }
        public string VehicleParticularsDetails { get; set; }
    }
}
