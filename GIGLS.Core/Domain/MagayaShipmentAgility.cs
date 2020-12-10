using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThirdParty.WebServices.Magaya.Business.New;

namespace GIGL.GIGLS.Core.Domain
{
    public class MagayaShipmentAgility : BaseDomain, IAuditable 
    {

        //Shipment Information
        [Key]
        public int MagayaShipmentId { get; set; }
        public int ServiceCenterCountryId { get; set; }
        public int ServiceCenterId { get; set; } 

        [MaxLength(20)]
        public string SealNumber { get; set; }

        public string Waybill { get; set; }

        //Senders' Information
        public decimal? Value { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        [MaxLength(100)]
        public string Type { get; set; }

        [MaxLength(100)]
        public string CompanyType { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }

        //Receivers Information
        public string OriginPort { get; set; }

        public string DestinationPort { get; set; }


        [MaxLength(200)]
        public string ShipperName { get; set; }

        [MaxLength(100)]
        public string ShipperPhoneNumber { get; set; }

        [MaxLength(100)]
        public string ShipperEmail { get; set; }

        [MaxLength(500)]
        public string ShipperAddress { get; set; }

        [MaxLength(50)]
        public string ShipperCity { get; set; }

        [MaxLength(50)]
        public string ShipperState { get; set; }

        [MaxLength(50)]
        public string ShipperCountry { get; set; }

        [MaxLength(200)]
        public string ConsigneeName { get; set; }

        [MaxLength(100)]
        public string ConsigneePhoneNumber { get; set; }

        [MaxLength(100)]
        public string ConsigneeEmail { get; set; }

        [MaxLength(500)]
        public string ConsigneeAddress { get; set; }

        [MaxLength(50)]
        public string ConsigneeCity { get; set; }

        [MaxLength(50)]
        public string ConsigneeState { get; set; }

        [MaxLength(50)]
        public string ConsigneeCountry { get; set; }

        //Delivery Options
        //public int DeliveryOptionId { get; set; }
        //public virtual DeliveryOption DeliveryOption { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //General but optional
        //public bool IsDomestic { get; set; }
        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public string MagayaShipmentItemsXml { get; set; }   
        public string ShipmentType { get; set; }
        public double ApproximateItemsWeight { get; set; }

        public decimal GrandTotal { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public decimal? ExpectedAmountToCollect { get; set; }
        public decimal? ActualAmountCollected { get; set; }

        //General Details comes with role user
        public Guid ShipmentGUID { get; set; }

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

        //Shipment reroute
        public bool IsCODPaidOut { get; set; }

        //use to optimise shipment progress for shipment that has depart service centre
        public ShipmentScanStatus ShipmentScanStatus { get; set; }

        public bool IsGrouped { get; set; }

        //Country info
        public decimal CurrencyRatio { get; set; }

        //new property for mobile
        [MaxLength(20)]
        public string DeliveryNumber { get; set; }
        public bool IsFromMobile { get; set; }

        //Collection information
        public bool IsShipmentCollected { get; set; } 
    }
}