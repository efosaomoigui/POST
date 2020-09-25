using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdParty.WebServices.Business
{
    public class IntlShipmentRequestDTO 
    {
        //Shipment Information 
        public int IntlShipmentRequestId { get; set; }

        public string RequestNumber { get; set; }

        //General Details comes with role user 
        public string UserId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerType { get; set; }
        public int CustomerId { get; set; }
        public int CustomerCountryId { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }

        //Senders' Information
        public decimal Value { get; set; }

        //public PaymentStatus PaymentStatus { get; set; }

        //Receivers Information
        public int DestinationServiceCentreId { get; set; }
        public virtual ServiceCentreDTO DestinationServiceCentre { get; set; }
        public int DestinationCountryId { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhoneNumber { get; set; }

        public string ReceiverEmail { get; set; }

        public string ReceiverAddress { get; set; }

        public string ReceiverCity { get; set; }

        public string ReceiverState { get; set; }

        public string ReceiverCountry { get; set; }

        //Delivery Options 
        //public int DeliveryOptionId { get; set; }

        //public DeliveryOption DeliveryOption { get; set; }

        //PickUp Options
        public Object PickupOptions { get; set; }

        //Shipment Items
        public virtual List<Object> ShipmentRequestItems { get; set; }
        public double ApproximateItemsWeight { get; set; }

        public decimal GrandTotal { get; set; }

        //discount information
        public decimal? Total { get; set; }

        //payment method 
        public string PaymentMethod { get; set; }

        //Sender's Address - added for the special case of corporate customers
        public string SenderAddress { get; set; }

        public string SenderState { get; set; }

        public int StationId { get; set; }

        //public bool IsProcessed { get; set; } 
    }

    public class ServiceCentreDTO 
    {
        public ServiceCentreDTO()
        {
            Users = new List<Object>();
            Shipments = new List<Object>();
        }
        public int ServiceCentreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int SupperServiceCentreId { get; set; }
        public string StationCode { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public Object Station { get; set; }
        public List<Object> Users { get; set; }
        public List<Object> Shipments { get; set; }
        public decimal TargetAmount { get; set; }
        public int TargetOrder { get; set; }
        public bool IsDefault { get; set; }
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
        public bool IsHUB { get; set; }
        public Object CountryDTO { get; set; }
    }
}
