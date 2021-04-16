using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

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
        public string PaymentTypeReference { get; set; }

        //Shipment Information
        public int ShipmentId { get; set; }
        public string SealNumber { get; set; }
        public decimal Value { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Insurance { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal ShipmentPackagePrice { get; set; }
        public bool IsInternational { get; set; }
        public double ApproximateItemsWeight { get; set; }

        //Customer Information
        public int CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyType { get; set; }
        public string CustomerCode { get; set; }
        public CustomerDTO CustomerDetails { get; set; }

        //Receiver Information
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }
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

        public string WalletBalance { get; set; }
        public DepositStatus DepositStatus { get; set; }

        public string InvoiceDueDays { get; set; }

        public string PaymentStatusDisplay { get; set; }

        public bool IsCODPaidOut { get; set; }

        //Country info
        public int DepartureCountryId { get; set; }
        public int DestinationCountryId { get; set; }
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }

        public DateTime DeliveryTime { get; set; }
        public decimal Cash { get; set; }
        public decimal Transfer { get; set; }
        public decimal Pos { get; set; }
        public decimal? DeclarationOfValueCheck { get; set; }
    }
     
    public class InvoiceViewDTOUNGROUPED
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DateCreated { get; set; } 
        public string Waybill { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public PickupOptions PickupOptions { get; set; }
    }

    public class InvoiceViewDTOUNGROUPED2
    {
        public string DestinationServiceCentreName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Waybill { get; set; }

    }

    public class InvoiceMonitorDTO
    {
        public string Waybill { get; set; } 
        public DateTime DateCreated { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public long Rn { get; set; }
        public PickupOptions PickupOptions { get; set; }

    }

    public class LimitDates
    {
        public int StartLimit { get; set; }
        public int EndLimit { get; set; }
        public string ScName { get; set; } 
    }

    public class InvoiceMonitorDTO2
    {
        public string label { get; set; }
        public int y { get; set; }
        //public string color { get; set; }
        public DateTime ShipmentDate { get; set; }
    }

    public class InvoiceMonitorDTO3
    {
        public string label { get; set; }
        public string waybill { get; set; }
        public DateTime ShipmentDate { get; set; }
    }


    public class MulitipleInvoiceMonitorDTO
    {

        public List<InvoiceMonitorDTO> ShipmentCreated { get; set; }
        public List<InvoiceMonitorDTO> ShipmentExpected { get; set; }
        public List<InvoiceMonitorDTO> ShipmentCollection { get; set; }

    }

    public class ColoredInvoiceMonitorDTO
    {
        public object[] groupgreen_s { get; set; }
        public object[] groupblue_s { get; set; }
        public object[] groupred_s { get; set; }
        public object[] totalZones { get; set; }

        //public double totalGreen { get; set; }
        //public double totalBlue { get; set; }
        //public double totalRed { get; set; }

    }

    public class InvoiceMonitorDTO_total
    {

        public int WayBillCount { get; set; }

    }

    public class CustomerInvoiceDTO
    {
        public CustomerInvoiceDTO()
        {
            InvoiceViewDTOs = new List<InvoiceViewDTO>();
            InvoiceCharges = new List<InvoiceChargeDTO>();
        }
        public decimal Total { get; set; }
        public decimal TotalVat { get; set; }
        public string InvoiceRefNo { get; set; }
        public string UserID { get; set; }
        public string CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<InvoiceViewDTO> InvoiceViewDTOs { get; set; }
        public List<InvoiceChargeDTO> InvoiceCharges { get; set; }

    }

    public class InvoiceChargeDTO
    {

        public decimal Amount { get; set; }
        public string Description { get; set; }

    }
}
