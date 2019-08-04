using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class PreShipment : BaseDomain, IAuditable
    {

        //Shipment Information
        [Key]
        public int PreShipmentId { get; set; }
        public string SealNumber { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        //Senders' Information
        public decimal Value { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        [MaxLength(100)]
        public string CustomerType { get; set; }
        public int CustomerId { get; set; }

        [MaxLength(100)]
        public string CompanyType { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public virtual ServiceCentre DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }

        //Delivery Options
        public int DeliveryOptionId { get; set; }
        public virtual DeliveryOption DeliveryOption { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //General but optional
        //public bool IsDomestic { get; set; }
        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public virtual List<PreShipmentItem> PreShipmentItems { get; set; }

        public decimal GrandTotal { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public decimal? ExpectedAmountToCollect { get; set; }
        public decimal? ActualAmountCollected { get; set; }

        //General Details comes with role user
        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; }

        //discount information
        public decimal? AppliedDiscount { get; set; }
        public decimal? DiscountValue { get; set; }

        public decimal? Insurance { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Total { get; set; }

        public decimal ShipmentPackagePrice { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        //Cancelled shipment
        public bool IsCancelled { get; set; }
        public bool IsInternational { get; set; }

        public string Description { get; set; }

        public PreShipmentRequestStatus RequestStatus { get; set; }
        public PreShipmentProcessingStatus ProcessingStatus { get; set; }

        //Receivers Information
        public int DepartureStationId { get; set; }
        public virtual Station DepartureStation { get; set; }

        public int DestinationStationId { get; set; }
        public virtual Station DestinationStation { get; set; }

        public bool IsMapped { get; set; }
        public string DeclinedReason { get; set; }

        //Agility Validations
        public decimal? CalculatedTotal { get; set; } = 0;

    }
}