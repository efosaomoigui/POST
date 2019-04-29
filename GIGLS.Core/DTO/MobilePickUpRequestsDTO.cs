using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class MobilePickUpRequestsDTO
    {
        public int MobilePickUpRequestsId { get; set; }
        public int PreShipmentMobileId { get; set; }
        public PreShipmentMobileDTO PreShipment { get; set; }
        public string Status { get; set; }
        public string Waybill { get; set; }

        public string UserId { get; set; }
        public  UserDTO User { get; set; }
    }
}
