﻿using GIGLS.Core.Enums;
using GIGLS.Core.DTO.Customers;
using System;
using System.Collections.Generic;
using GIGLS.CORE.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO.Shipments;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentDTO : BaseDomainDTO
    {
        //Shipment Information
        public int ShipmentId { get; set; }
        public string SealNumber { get; set; }

        public string Waybill { get; set; }

        //Senders' Information
        public decimal Value { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string CustomerType { get; set; }
        public int CustomerId { get; set; }

        //Receivers Information
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }

        //Delivery Options
        public int DeliveryOptionId { get; set; }
        public DeliveryOptionDTO DeliveryOption { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //General but optional
        //public bool IsDomestic { get; set; }
        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public List<ShipmentItemDTO> ShipmentItems { get; set; }

        public decimal GrandTotal { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; } = 0;

        public decimal? ExpectedAmountToCollect { get; set; } = 0;
        public decimal? ActualAmountCollected { get; set; } = 0;

        //General Details comes with role user
        public string UserId { get; set; }

        //
        public List<CustomerDTO> Customer { get; set; }
        public CustomerDTO CustomerDetails { get; set; }

        //
        public bool IsdeclaredVal { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; } = 0;

        //discount information
        public decimal? AppliedDiscount { get; set; } = 0;
        public decimal? DiscountValue { get; set; } = 0;

        public decimal? Insurance { get; set; } = 0;
        public decimal? Vat { get; set; } = 0;
        public decimal? Total { get; set; } = 0;

        //wallet information
        public string WalletNumber { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; } = 0;
        public decimal? InvoiceDiscountValue_display { get; set; } = 0;
        public decimal? offInvoiceDiscountvalue_display { get; set; } = 0;

        //ShipmentCollection
        public ShipmentCollectionDTO ShipmentCollection { get; set; }

        //Demurrage Information
        public DemurrageDTO Demurrage { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsInternational { get; set; }
    }
}
