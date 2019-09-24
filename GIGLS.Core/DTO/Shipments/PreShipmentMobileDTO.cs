using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class PreShipmentMobileDTO : BaseDomainDTO
    {
        public int PreShipmentMobileId { get; set; }
        public new DateTime? DateCreated { get; set; }
        public string Waybill { get; set; }

        //Senders' Information
        public string SenderName { get; set; }

        public string SenderPhoneNumber { get; set; }
        public decimal Value { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public int SenderStationId { get; set; }

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



        
        public LocationDTO SenderLocation { get; set; }

        

        public  LocationDTO ReceiverLocation { get; set; }
        //Delivery Options
        public bool IsHomeDelivery { get; set; }

        //General but optional

        public DateTime? ExpectedDateOfArrival { get; set; }
        public DateTime? ActualDateOfArrival { get; set; }

        //Shipment Items
        public List<PreShipmentItemMobileDTO> PreShipmentItems { get; set; }

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

    }
}
