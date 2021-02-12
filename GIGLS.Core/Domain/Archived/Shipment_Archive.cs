using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Archived
{
    public class Shipment_Archive : BaseDomain_Archive
    {
        [Key]
        public int ShipmentId { get; set; }

        [MaxLength(20)]
        public string SealNumber { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
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
        public int DeliveryOptionId { get; set; }
        public PickupOptions PickupOptions { get; set; }
        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }
        public double ApproximateItemsWeight { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public decimal? ExpectedAmountToCollect { get; set; }
        public decimal? ActualAmountCollected { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; }
        public decimal? AppliedDiscount { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Insurance { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Total { get; set; }
        public decimal ShipmentPackagePrice { get; set; }
        public decimal ShipmentPickupPrice { get; set; }
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        [MaxLength(20)]
        public string PaymentMethod { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsInternational { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public DepositStatus DepositStatus { get; set; }
        public bool ReprintCounterStatus { get; set; }

        [MaxLength(500)]
        public string SenderAddress { get; set; }

        [MaxLength(50)]
        public string SenderState { get; set; }

        [MaxLength(100)]
        public string ShipmentReroute_WaybillNew { get; set; }
        public bool IsCODPaidOut { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public bool IsGrouped { get; set; }
        public int DepartureCountryId { get; set; }
        public int DestinationCountryId { get; set; }
        public decimal CurrencyRatio { get; set; }

        [MaxLength(20)]
        public string DeliveryNumber { get; set; }
        public bool IsFromMobile { get; set; }
        public bool isInternalShipment { get; set; }
        public bool IsCargoed { get; set; }
        public InternationalShipmentType InternationalShipmentType { get; set; }
        public bool IsClassShipment { get; set; }
    }

}
