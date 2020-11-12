using Newtonsoft.Json;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    public class RequestedShipmentPayload
    {
        public RequestedShipmentPayload()
        {
            ShipmentRequest = new ShipmentRequest();
        }
        public ShipmentRequest ShipmentRequest { get; set; }
    }

    public class ShipmentRequest
    {
        public ShipmentRequest()
        {
            RequestedShipment = new RequestedShipmentDTO();
        }
        public RequestedShipmentDTO RequestedShipment { get; set; }
    }

    public class RequestedShipmentDTO
    {
        public RequestedShipmentDTO()
        {
            ShipmentInfo = new ShipmentInfoDTO();
        }
        public string ShipTimestamp { get; set; }
        public string PaymentInfo { get; set; } = "DAP";

        public ShipmentInfoDTO ShipmentInfo { get; set; }
        public InternationalDetailDTO InternationalDetail { get; set; }
        public ShipDTO Ship { get; set; }
        public PackagesDTO Packages { get; set; }
    }

    public class ShipmentInfoDTO
    {
        public string DropOffType { get; set; }
        public string ServiceType { get; set; } = "P";
        public string Account { get; set; }
        public string Currency { get; set; } = "NGN";
        public string UnitOfMeasurement { get; set; }
    }

    public class InternationalDetailDTO
    {
        public InternationalDetailDTO()
        {
            Commodities = new Commodity();
        }
        public string Content { get; set; }
        public Commodity Commodities { get; set; }
    }

    public class Commodity
    {
        public int NumberOfPieces { get; set; }
        public string Description { get; set; }
        public string CountryOfManufacture { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int CustomsValue { get; set; }
    }

    public class ShipDTO
    {
        public ShipDTO()
        {
            Shipper = new Details();
            Recipient = new Details();
        }
        public Details Shipper { get; set; }
        public Details Recipient { get; set; }
    }

    public class Details
    {
        public Details()
        {
            Contact = new Contact();
            Address = new AddressPayload();
        }
        public Contact Contact { get; set; }
        public AddressPayload Address { get; set; }
    }

    public class Contact
    {
        public string PersonName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }

    public class PackagesDTO
    {
        public PackagesDTO()
        {
            RequestedPackages = new List<RequestedPackagesDTO>();
        }
        public List<RequestedPackagesDTO> RequestedPackages { get; set; }

    }

    public class RequestedPackagesDTO
    {
        public RequestedPackagesDTO()
        {
            Dimensions = new Dimensions();
        }

        [JsonProperty("@number")]
        public int @number { get; set; }
        public decimal Weight { get; set; }
        public Dimensions Dimensions { get; set; }
        public string CustomerReferences { get; set; }
    }
}
