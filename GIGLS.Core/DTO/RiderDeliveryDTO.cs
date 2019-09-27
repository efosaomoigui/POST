using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO
{
    public class RiderDeliveryDTO : BaseDomainDTO     
    {
        public int RiderDeliveryId { get; set; }
        public string Waybill { get; set; }
        public string DriverId { get; set; }
        public decimal CostOfDelivery { get; set; }
        public DateTime DeliveryDate { get; set; }
        public UserDTO UserDetail { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
    }
}
