using GIGLS.Core.Enums;
using System.Collections.Generic;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class PreShipmentDTO : BaseDomainDTO
    {
        //Shipment Information
        public int PreShipmentId { get; set; }
        public string TempCode { get; set; }
        public string Waybill { get; set; }

        //Senders' Information
        
        public string CompanyType { get; set; }
        public string CustomerCode { get; set; }
        public int CountryId { get; set; }
        public decimal WalletBalance { get; set; }
        public string SenderUserId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderCity { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public PickupOptions PickupOptions { get; set; }

        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }

        //PreShipment Items
        public List<PreShipmentItemDTO> PreShipmentItems { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsProcessed { get; set; }
    }
}
