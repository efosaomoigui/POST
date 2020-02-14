using GIGLS.Core.Enums;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class MobilePriceDTO
    {
        public decimal? DeliveryPrice { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? Vat { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Discount { get; set; }
        public PreShipmentMobileDTO PreshipmentMobile { get; set; }
        public List<PreShipmentMobileDTO> PreshipmentMobileList { get; set; }

        public decimal? MainCharge { get; set; }

        public decimal? PickUpCharge { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }
        public bool? IsWithinProcessingTime { get; set; }
    }

    public class MultipleMobilePriceDTO
    {
        //public decimal? DeliveryPrice { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? Vat { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? MainCharge { get; set; }
        public decimal? PickUpCharge { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
        public List<MobilePricePerReceiverDTO> receiversPriceDetails { get; set; }
        public bool IsWithinProcessingTime { get; set; }
    }
    public class MobilePricePerReceiverDTO
    {
        //public decimal? DeliveryPrice { get; set; }
        public decimal? ReceiverInsuranceValue { get; set; }
        public decimal? ReceiverVat { get; set; }
        public decimal? ReceiverDiscount { get; set; }
        public decimal? ReceiverMainCharge { get; set; }
        public List<ReceiverMobilePriceItemsDTO> ReceiverItemPrices { get; set; }
    }
    public class ReceiverMobilePriceItemsDTO
    {
        public string Description { get; set; }
        public string Weight { get; set; }
        public string Value { get; set; }
        public decimal? ItemCalculatedPrice { get; set; }
        public ShipmentType ShipmentType { get; set; }
    }

}
