using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class ServiceCentreDTO : BaseDomainDTO
    {
        public ServiceCentreDTO()
        {
            Users = new List<UserDTO>();
            Shipments = new List<ShipmentDTO>();
        }
        public int ServiceCentreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public string Country { get; set; }
        public StationDTO Station { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<ShipmentDTO> Shipments { get; set; }
        public decimal TargetAmount { get; set; }
        public int TargetOrder { get; set; }
    }
}
