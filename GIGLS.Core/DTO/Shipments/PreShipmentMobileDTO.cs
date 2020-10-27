using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class PreShipmentMobileDTO : BaseDomainDTO
    {
        public PreShipmentMobileDTO()
        {
            SenderLocation = new LocationDTO();
            ReceiverLocation = new LocationDTO();
            PreShipmentItems = new List<PreShipmentItemMobileDTO>();
            serviceCentreLocation = new LocationDTO();
            partnerDTO = new PartnerDTO();
        }

        public int PreShipmentMobileId { get; set; }
        public new DateTime? DateCreated { get; set; }
        public string Waybill { get; set; }
        public string GroupCodeNumber { get; set; }

        //Senders' Information
        public string SenderName { get; set; }
        public string SenderStationName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public decimal Value { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public int SenderStationId { get; set; }
        public string InputtedSenderAddress { get; set; }
        public string SenderLocality { get; set; }

        public int ReceiverStationId { get; set; }

        public string CustomerType { get; set; }
        public string CompanyType { get; set; }
        public string CustomerCode { get; set; }
        public string SenderAddress { get; set; }

        //Receivers Information
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverStationName { get; set; }
        public string InputtedReceiverAddress { get; set; }

        public LocationDTO SenderLocation { get; set; }      

        public  LocationDTO ReceiverLocation { get; set; }
        //Delivery Options
        public bool IsHomeDelivery { get; set; }

        //General but optional

        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public List<PreShipmentItemMobileDTO> PreShipmentItems { get; set; } = null;

        public decimal GrandTotal { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        public bool IsCashOnDelivery { get; set; }
        public decimal? CashOnDeliveryAmount { get; set; }
        public decimal? ExpectedAmountToCollect { get; set; }
        public decimal? ActualAmountCollected { get; set; }


        //General Details comes with role user
        public string UserId { get; set; }

        public bool IsdeclaredVal { get; set; }


        //discount information
      

        public decimal? DiscountValue { get; set; }

        public decimal? Vat { get; set; }

        public decimal? InsuranceValue { get; set; }
        public decimal? DeliveryPrice { get; set; }
        public decimal? Total { get; set; }

        public decimal? ShipmentPackagePrice { get; set; }

        //from client
        public decimal? vatvalue_display { get; set; }
        public decimal? InvoiceDiscountValue_display { get; set; }
        public decimal? offInvoiceDiscountvalue_display { get; set; }

        //Cancelled shipment
        public bool IsCancelled { get; set; }
        public bool IsConfirmed { get; set; }
        public string DeclinedReason { get; set; }

        //Agility Validations
        public double? CalculatedTotal { get; set; } = 0;
        public bool? IsBalanceSufficient { get; set; }

        public string shipmentstatus { get; set; }
        public bool IsDelivered { get; set; }
        public int TrackingId { get; set; }

        public string VehicleType { get; set; }

        public int? ZoneMapping { get; set; }
        public List<int> DeletedItems { get; set; }

        public bool IsRated { get; set; }

        public string PartnerFirstName { get; set; }

        public string PartnerLastName { get; set; }

        public string PartnerImageUrl { get; set; }
        public string ActualReceiverFirstName { get; set; }
        public string ActualReceiverLastName { get; set; }
        public string ActualReceiverPhoneNumber { get; set; }

        public string CountryName { get; set; }
        public int CountryId { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyCode { get; set; }

        public int? Haulageid { get; set; }
        public ShipmentType Shipmentype { get; set; }
        public string ServiceCentreAddress { get; set; }
        public LocationDTO serviceCentreLocation { get; set; }
        public bool? IsApproved { get; set; }

        public bool? IsFromShipment { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public int CustomerId { get; set; }
        
        public bool? IsEligible { get; set; }
        public bool IsCodNeeded { get; set; }
        public decimal CurrentWalletAmount { get; set; }
        public decimal ShipmentPickupPrice { get; set; }
        public PartnerDTO partnerDTO { get; set; }

        public DateTime? TimeAssigned { get; set; }
        public DateTime? TimePickedUp { get; set; }
        public DateTime? TimeDelivered { get; set; }

        public string IndentificationUrl { get; set; }
        public string DeliveryAddressImageUrl { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? ScheduledDate { get; set; }

        public string QRCode { get; set; }
        public string SenderCode { get; set; }
        public string ReceiverCode { get; set; }
        public int DestinationServiceCenterId { get; set; }
        public string DestinationServiceCenterName { get; set; }
        public bool IsBatchPickUp { get; set; }
    }
    public class NewPreShipmentMobileDTO : BaseDomainDTO
    {
        public int PreShipmentMobileId { get; set; }

        //Senders' Information
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public int SenderStationId { get; set; }
        public string CustomerType { get; set; }
        public string CompanyType { get; set; }
        public string CustomerCode { get; set; }
        public string SenderAddress { get; set; }
        public LocationDTO SenderLocation { get; set; }

        //General Details comes with role user
        public string UserId { get; set; }

        //public string PartnerFirstName { get; set; }
        //public string PartnerLastName { get; set; }
        //public string PartnerImageUrl { get; set; }

        public decimal CurrentWalletAmount { get; set; } 

        public string CountryName { get; set; }  
        public int CountryId { get; set; } 

        public string CurrencySymbol { get; set; } 

        public string CurrencyCode { get; set; } 
        public bool? IsEligible { get; set; } 
        public bool IsCodNeeded { get; set; } 
        public ShipmentType Shipmentype { get; set; } 
        public bool? IsFromShipment { get; set; } 
        public string VehicleType { get; set; }
        public bool? IsBalanceSufficient { get; set; }  
        public int? Haulageid { get; set; }
        public decimal PickupPrice { get; set; } 

        //List of Receivers
        public List<ReceiverPreShipmentMobileDTO> Receivers { get; set; }
     
        public PartnerDTO partnerDTO { get; set; }
    }

    public class ReceiverPreShipmentMobileDTO : BaseDomainDTO
    {
        //Receivers Information
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public int ReceiverStationId { get; set; }
        public LocationDTO ReceiverLocation { get; set; }

        public string Waybill { get; set; } 
        //Delivery Options
        public bool IsHomeDelivery { get; set; }
        public int? ZoneMapping { get; set; } 
        public decimal GrandTotal { get; set; } 
        public bool IsdeclaredVal { get; set; }  
        public decimal? DeliveryPrice { get; set; }  
        public decimal? InsuranceValue { get; set; } 
        public double? CalculatedTotal { get; set; } = 0; 
        public decimal Value { get; set; } 
        public bool IsConfirmed { get; set; } 
        public bool IsDelivered { get; set; }  
        public string shipmentstatus { get; set; } 
        public decimal ReceiverPickupPrice { get; set; }

        //discount information
        public decimal? DiscountValue { get; set; }
        //Shipment Items
        //public List<NewPreShipmentItemMobileDTO> PreShipmentItems { get; set; }
        public List<PreShipmentItemMobileDTO> preShipmentItems { get; set; }

        //Invoice parameters: Helps generate invoice for ecomnerce customers  by customerType
        //public bool IsCashOnDelivery { get; set; }
        //public decimal? CashOnDeliveryAmount { get; set; }
        //public decimal? ExpectedAmountToCollect { get; set; }
        //public decimal? ActualAmountCollected { get; set; }

    }

    public class PreShipmentMobileReportDTO
    {
        public string Waybill { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string CompanyType { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal GrandTotal { get; set; }
        public double CalculatedTotal { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Vat { get; set; }
        public decimal? DeliveryPrice { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? Value { get; set; }
        public string shipmentstatus { get; set; }
        public string SenderStationName { get; set; }
        public string ReceiverStationName { get; set; }
        public DateTime DateCreated { get; set; }
        public string VehicleType { get; set; }
    }
}
