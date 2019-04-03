using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
   public  class ThirdPartyShipmentDTO
    {
        public string CustomerCode { get; set; }
        public string UserId { get; set; }
        public int SenderStationId { get; set; }
        public int ReceiverStationId { get; set; }
        public decimal DeliveryPrice { get; set; }
        public double Vat { get; set; }

        public double Discount { get; set; }
        public List<ThirdPartyShipmentItemsDTO> ShipmentItems { get; set; }

        public double InsuranceValue { get; set; }
    }
}
