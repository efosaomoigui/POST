using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    public class RateRequestPayload
    {
        public RateRequestPayload()
        {
            RateRequest = new RateRequest();
        }
        public RateRequest RateRequest { get; set; }
    }

    public class RateRequest
    {
        public RateRequest()
        {
            RequestedShipment = new RequestedShipment();
        }
        public string ClientDetails { get; set; } = null;

        public RequestedShipment RequestedShipment { get; set; }
    }

    public class RequestedShipment
    {
        public RequestedShipment()
        {
            Ship = new Ship();
            Packages = new Packages();
        }
        public string DropOffType { get; set; }
        public DateTime ShipTimestamp { get; set; }
        public string NextBusinessDay { get; set; } = "Y";
        public string Account { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string Content { get; set; }
        public string PaymentInfo { get; set; } = "DAP";
        public Ship Ship { get; set; }
        public Packages Packages { get; set; }
    }

    public class Ship
    {
        public Ship()
        {
            Shipper = new AddressPayload();
            Recipient = new AddressPayload();
        }
        public AddressPayload Shipper { get; set; }
        public AddressPayload Recipient { get; set; }
    }
    public class Packages
    {
        public Packages()
        {
            RequestedPackages = new List<RequestedPackages>();
        }
        public List<RequestedPackages> RequestedPackages { get; set; }
    }

    public class RequestedPackages
    {
        public RequestedPackages()
        {
            Weight = new Weight();
            Dimensions = new Dimensions();
        }

        [JsonProperty("@number")]
        public int @number { get; set; }
        public Weight Weight { get; set; }
        public Dimensions Dimensions { get; set; }
    }

    public class Weight
    {
        public decimal Value { get; set; }
    }
}
