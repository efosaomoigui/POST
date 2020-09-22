using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class TransactionPreShipmentDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string shipmentstatus { get; set; }
        public decimal GrandTotal { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsRated { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public string PartnerImageUrl { get; set; }
        public List<PreShipmentItemMobileDTO> PreShipmentItems { get; set; } = null;
    }
}
