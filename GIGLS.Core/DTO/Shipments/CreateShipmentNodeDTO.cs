using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class CreateShipmentNodeDTO
    {
        public string waybillNumber { get; set; }
        public string customerId { get; set; }
        public string locality { get; set; }
        public string receiverAddress { get; set; }
        public string vehicleType { get; set; }
        public decimal value { get; set; }
        public int? zone { get; set; }
        public NodeLocationDTO receiverLocation { get; set; }
        public string senderAddress { get; set; }
        public NodeLocationDTO senderLocation { get; set; }
    }
}