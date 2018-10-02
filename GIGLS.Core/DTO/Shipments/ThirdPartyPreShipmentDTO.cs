using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.Shipments
{
    public class ThirdPartyPreShipmentDTO
    {
        //PickUp' Information
        [Required]
        public string PickUpPhoneNumber { get; set; }

        [Required]
        public string PickUpAddress { get; set; }

        [Required]
        public string PickUpCity { get; set; }

        [Required]
        public string PickUpState { get; set; }

        [Required]
        public string PickUpCountry { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public string ReceiverPhoneNumber { get; set; }

        [Required]
        public string ReceiverEmail { get; set; }

        [Required]
        public string ReceiverAddress { get; set; }

        [Required]
        public string ReceiverCity { get; set; }

        [Required]
        public string ReceiverState { get; set; }

        [Required]
        public string ReceiverCountry { get; set; }

        //PreShipment Items
        [Required]
        public List<ThirdPartyPreShipmentItemDTO> PreShipmentItems { get; set; }


        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; } = 0;

        //
        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; } = 0;

        [Required]
        public decimal? Total { get; set; } = 0;

        public string Description { get; set; }


        //Receivers Information
        [Required]
        public int DepartureStationId { get; set; }

        [Required]
        public int DestinationStationId { get; set; }


        ///////Optional fields/////////

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //Delivery Options
        public int DeliveryOptionId { get; set; } = 1;
        public DeliveryOptionDTO DeliveryOption { get; set; }
        public List<int> DeliveryOptionIds { get; set; } = new List<int>();

        public decimal GrandTotal { get; set; }

        public decimal? ExpectedAmountToCollect { get; set; } = 0;
        public decimal? ActualAmountCollected { get; set; } = 0;

        //discount information
        public decimal? AppliedDiscount { get; set; } = 0;
        public decimal? DiscountValue { get; set; } = 0;

        public decimal? Insurance { get; set; } = 0;
        public decimal? Vat { get; set; } = 0;

        public decimal ShipmentPackagePrice { get; set; }

        //Agility Validations
        public decimal? CalculatedTotal { get; set; } = 0;

    }
}
