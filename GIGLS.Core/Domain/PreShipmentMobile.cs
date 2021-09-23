using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class PreShipmentMobile : BaseDomain, IAuditable
    {
        [Key]
        public int PreShipmentMobileId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        //Senders' Information
        [MaxLength(500)]
        public string SenderName { get; set; }

        [MaxLength(100)]
        public string SenderPhoneNumber { get; set; }

        public decimal Value { get; set; }
        public DateTime? DeliveryTime { get; set; }

        [MaxLength(100)]
        public string CustomerType { get; set; }

        [MaxLength(100)]
        public string CompanyType { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }

        //Receivers Information
        [MaxLength(500)]
        public string ReceiverName { get; set; }

        [MaxLength(100)]
        public string ReceiverPhoneNumber { get; set; }

        [MaxLength(500)]
        public string ReceiverEmail { get; set; }

        [MaxLength(500)]
        public string ReceiverAddress { get; set; }

        [MaxLength(500)]
        public string ReceiverCity { get; set; }

        [MaxLength(500)]
        public string ReceiverState { get; set; }

        [MaxLength(100)]
        public string ReceiverCountry { get; set; }

        [MaxLength(500)]
        public string InputtedReceiverAddress { get; set; }

        [MaxLength(500)]
        public string SenderLocality { get; set; }

        [MaxLength(500)]
        public string SenderAddress { get; set; }

        [MaxLength(500)]
        public string InputtedSenderAddress { get; set; }

        public int SenderStationId { get; set; }

        public int ReceiverStationId { get; set; }
        public virtual Location SenderLocation { get; set; }
        public virtual Location ReceiverLocation { get; set; }
        //Delivery Options
        public bool IsHomeDelivery { get; set; }

        //General but optional

        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public virtual List<PreShipmentItemMobile> PreShipmentItems { get; set; }

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

        //discount information
        public decimal? Total { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Vat { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? DeliveryPrice { get; set; }
        public decimal? ShipmentPackagePrice { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        //Cancelled shipment
        public bool IsCancelled { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDelivered { get; set; }

        [MaxLength(500)]
        public string DeclinedReason { get; set; }

        //Agility Validations
        public double? CalculatedTotal { get; set; } = 0;

        [MaxLength(500)]
        public string shipmentstatus { get; set; }

        [MaxLength(500)]
        public string VehicleType { get; set; }

        public int? ZoneMapping { get; set; }

        [MaxLength(500)]
        public string ActualReceiverFirstName { get; set; }

        [MaxLength(500)]
        public string ActualReceiverLastName { get; set; }

        [MaxLength(500)]
        public string ActualReceiverPhoneNumber { get; set; }

        public int CountryId { get; set; }

        [MaxLength(500)]
        public string ServiceCentreAddress { get; set; }
        public virtual Location serviceCentreLocation {get;set;}
        public bool? IsApproved { get; set; }
        public decimal ShipmentPickupPrice { get; set; }

        [MaxLength(20)]
        public string DeliveryNumber { get; set; }
        public DateTime? TimeAssigned { get; set; }
        public DateTime? TimePickedUp { get; set; }
        public DateTime? TimeDelivered { get; set; }

        [MaxLength(500)]
        public string IndentificationUrl { get; set; }

        [MaxLength(500)]
        public string DeliveryAddressImageUrl { get; set; }

        public bool IsScheduled { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int DestinationServiceCenterId { get; set; }
        public bool IsBatchPickUp { get; set; }
        [MaxLength(500)]
        public string WaybillImageUrl { get; set; }
        public bool IsFromAgility { get; set; }
        public int Haulageid { get; set; }

        [MaxLength(200)]
        public string ReceiverCompanyName { get; set; }
        [MaxLength(50)]
        public string ReceiverPostalCode { get; set; }
        [MaxLength(5)]
        public string ReceiverStateOrProvinceCode { get; set; }
        [MaxLength(200)]
        public string ReceiverCountryCode { get; set; }
        public decimal InternationalShippingCost { get; set; }
        [MaxLength(5)]
        public string ManufacturerCountry { get; set; }
        [MaxLength(170)]
        public string ItemDetails { get; set; }
        public CompanyMap CompanyMap { get; set; }
        public bool IsInternationalShipment { get; set; }
        public decimal DeclarationOfValueCheck { get; set; }
        public int DepartureCountryId { get; set; }
        public int DestinationCountryId { get; set; }
        [MaxLength(300)]
        public string CustomerCancelReason { get; set; }
        public bool IsCoupon { get; set; }
        [MaxLength(50)]
        public string CouponCode { get; set; }
    }
}
