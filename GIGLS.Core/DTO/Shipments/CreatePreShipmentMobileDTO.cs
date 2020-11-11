using GIGLS.CORE.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.Shipments
{
    public class CreatePreShipmentMobileDTO : BaseDomainDTO
    {
        public int PreShipmentMobileId { get; set; }

        //Senders' Information
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string SenderPhoneNumber { get; set; }
        [Required]
        public int SenderStationId { get; set; }
        public string InputtedSenderAddress { get; set; }
        [Required]
        public string SenderLocality { get; set; }

        [Required]
        public int ReceiverStationId { get; set; }
        //public string CustomerCode { get; set; }
        [Required]
        public string SenderAddress { get; set; }

        ////Receivers Information
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string ReceiverPhoneNumber { get; set; }
        [Required]
        public string ReceiverAddress { get; set; }
        public string InputtedReceiverAddress { get; set; }

        [Required]
        public LocationDTO SenderLocation { get; set; }
        [Required]
        public LocationDTO ReceiverLocation { get; set; }

        //Shipment Items
        [Required]
        public List<PreShipmentItemMobileDTO> PreShipmentItems { get; set; } = null;
        
        [Required]
        public string VehicleType { get; set; }

        public bool IsBatchPickUp { get; set; }
        public string WaybillImage { get; set; }
        public string WaybillImageFormat { get; set; }
    }
}
