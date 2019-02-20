using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Account
{
    public class InvoiceViewDTO : BaseDomainDTO
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Waybill { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsShipmentCollected { get; set; }

        //Shipment Information
        public int ShipmentId { get; set; }
        public string SealNumber { get; set; }
        public decimal Value { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Insurance { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal CashOnDeliveryAmount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal ShipmentPackagePrice { get; set; }
        public bool IsInternational { get; set; }

        //Customer Information
        public int CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyType { get; set; }
        public string CustomerCode { get; set; }
        public CustomerDTO CustomerDetails { get; set; }

        //Receiver Information
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }

        //DeliveryOption
        public int DeliveryOptionId { get; set; }
        public string DeliveryOptionCode { get; set; }
        public string DeliveryOptionDescription { get; set; }
        public DeliveryOptionDTO DeliveryOption { get; set; }

        //station
        public int StationId { get; set; }
        public string StationName { get; set; }

        public int DepartureStationId { get; set; }
        public string DepartureStationName { get; set; }

        public int DestinationStationId { get; set; }
        public string DestinationStationName { get; set; }

        //service centre
        public int DepartureServiceCentreId { get; set; }
        public string DepartureServiceCentreCode { get; set; }
        public string DepartureServiceCentreName { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public string DestinationServiceCentreCode { get; set; }
        public string DestinationServiceCentreName { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }

        //UserId
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Description { get; set; }

        //PickUp Options
        public PickupOptions PickupOptions { get; set; }

        //Customer
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public int? IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DepositStatus { get; set; }

    }
}
