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
        public NewPreShipmentMobileDTO NewPreshipmentMobile { get; set; }              

        public decimal? MainCharge { get; set; }

        public decimal? PickUpCharge { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }
        public bool? IsWithinProcessingTime { get; set; }
    }

    public class NewMobilePriceDTO
    {
        //public decimal? GrandTotal { get; set; }
        public NewPreShipmentMobileDTO PreshipmentMobile { get; set; }
        public List<RecieverMobilePriceDTO> recieverMobilePrices { get; set; }


        public decimal? PickUpCharge { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }
        public bool? IsWithinProcessingTime { get; set; }
        public decimal? GrandTotal { get; set; } //Calculated total + pickup price
    }
    public class RecieverMobilePriceDTO
    {
        public decimal? DeliveryPrice { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? Vat { get; set; }
        
        public decimal? Discount { get; set; }
        public ReceiverPreShipmentMobileDTO Receiver { get; set; }

        public decimal MainCharge { get; set; }
    }
}
