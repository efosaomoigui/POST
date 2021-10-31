using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class Shipment : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentId { get; set; }

        [MaxLength(20)]
        public string SealNumber { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }
        public int AwaitingCollectionCount { get; set; }

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
        public int DestinationServiceCentreId { get; set; }

        [MaxLength(200)]
        public string ReceiverName { get; set; }

        [MaxLength(100)]
        public string ReceiverPhoneNumber { get; set; }

        [MaxLength(100)]
        public string ReceiverEmail { get; set; }

        [MaxLength(500)]
        public string ReceiverAddress { get; set; }

        [MaxLength(50)]
        public string ReceiverCity { get; set; }

        [MaxLength(50)]
        public string ReceiverState { get; set; }

        [MaxLength(50)]
        public string ReceiverCountry { get; set; }

        //Delivery Options
        public int DeliveryOptionId { get; set; }
        public virtual DeliveryOption DeliveryOption { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public virtual List<ShipmentItem> ShipmentItems { get; set; }
        public double ApproximateItemsWeight { get; set; }

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

        public decimal ShipmentPickupPrice { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        //payment method 
        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        //Cancelled shipment
        public bool IsCancelled { get; set; }
        public bool IsInternational { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DepositStatus DepositStatus { get; set; }

        public bool ReprintCounterStatus { get; set; }

        //Sender's Address - added for the special case of corporate customers
        [MaxLength(500)]
        public string SenderAddress { get; set; }

        [MaxLength(50)]
        public string SenderState { get; set; }
        //Shipment reroute
        public ShipmentReroute ShipmentReroute { get; set; }
        public bool IsCODPaidOut { get; set; }

        //use to optimise shipment progress for shipment that has depart service centre
        public ShipmentScanStatus ShipmentScanStatus { get; set; }

        public bool IsGrouped { get; set; }

        //Country info
        public int DepartureCountryId { get; set; }
        public int DestinationCountryId { get; set; }
        public decimal CurrencyRatio { get; set; }

        //new property for mobile
        [MaxLength(20)]
        public string DeliveryNumber { get; set; }
        public bool IsFromMobile { get; set; }

        public bool isInternalShipment { get; set; }
        public bool IsCargoed { get; set; }

        public InternationalShipmentType InternationalShipmentType { get; set; }

        public bool IsClassShipment { get; set; }

        [MaxLength(500)]
        public string FileNameUrl { get; set; }

        [MaxLength(100)]
        public string InternationalWayBill { get; set; }
        public decimal InternationalShippingCost { get; set; }
        [MaxLength(50)]
        public string Courier { get; set; }
        public bool ExpressDelivery { get; set; }
        public bool IsExported { get; set; }
        [MaxLength(128)]
        public string RequestNumber { get; set; }

    }

    public class IntlShipmentRequest : BaseDomain, IAuditable
    { 
        //Shipment Information 
        [Key]
        public int IntlShipmentRequestId { get; set; } 

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string RequestNumber { get; set; }

        //General Details comes with role user 
        [MaxLength(128)]
        public string UserId { get; set; }
        
        [MaxLength(50)]
        public string CustomerFirstName { get; set; }

        [MaxLength(50)]
        public string CustomerLastName { get; set; }

        [MaxLength(50)]
        public string CustomerType { get; set; }

        public int CustomerId { get; set; }
        public int CustomerCountryId { get; set; }

        [MaxLength(500)]
        public string CustomerAddress { get; set; }

        [MaxLength(100)]
        public string CustomerEmail { get; set; }

        [MaxLength(100)]
        public string CustomerPhoneNumber { get; set; }

        [MaxLength(50)]
        public string CustomerCity { get; set; }

        [MaxLength(50)]
        public string CustomerState { get; set; }

        [MaxLength(150)]
        public string ItemSenderfullName { get; set; }

        //Senders' Information
        public decimal Value { get; set; }

        //public PaymentStatus PaymentStatus { get; set; }

        //Receivers Information
        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentre DestinationServiceCentre { get; set; }
        public int DestinationCountryId { get; set; }

        [MaxLength(200)]
        public string ReceiverName { get; set; }

        [MaxLength(100)]
        public string ReceiverPhoneNumber { get; set; }

        [MaxLength(100)]
        public string ReceiverEmail { get; set; }

        [MaxLength(500)]
        public string ReceiverAddress { get; set; }

        [MaxLength(50)]
        public string ReceiverCity { get; set; }

        [MaxLength(50)]
        public string ReceiverState { get; set; }

        [MaxLength(50)]
        public string ReceiverCountry { get; set; }

        //Delivery Options 
        //public int DeliveryOptionId { get; set; }

        //public DeliveryOption DeliveryOption { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //Shipment Items
        public virtual List<IntlShipmentRequestItem> ShipmentRequestItems { get; set; }  
        public double ApproximateItemsWeight { get; set; }

        public decimal GrandTotal { get; set; }

        //discount information
        public decimal? Total { get; set; }

        //payment method 
        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        //Sender's Address - added for the special case of corporate customers
        [MaxLength(500)]
        public string SenderAddress { get; set; }

        [MaxLength(50)]
        public string SenderState { get; set; }

        public bool IsProcessed { get; set; }

        public bool IsMobile { get; set; }
        public bool Consolidated { get; set; }
        [MaxLength(128)]
        public string ConsolidationId { get; set; }
        public int RequestProcessingCountryId { get; set; }

    }

}