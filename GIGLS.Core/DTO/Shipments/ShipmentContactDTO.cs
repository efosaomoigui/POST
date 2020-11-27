using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentContactDTO
    {
        
        public int ShipmentContactId { get; set; }
        public string Waybill { get; set; }
        public ShipmentContactStatus Status { get; set; }
        public string ContactedBy { get; set; }
        public int NoOfContact { get; set; }
        public string UserId { get; set; }
        public DateTime ShipmentCreatedDate { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public int Age { get; set; }
        public string ShipmentStatus { get; set; }
        public string DepartureServiceCentre { get; set; }
        public string DestinationServiceCentre { get; set; }
    }
}
