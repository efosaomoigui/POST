using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{

    public class RateRequestResponse
    {
        public RateRequestResponse()
        {
            RateResponse = new RateResponse();
        }
        public RateResponse RateResponse { get; set; }
    }

    public class RateResponse
    {
        public RateResponse()
        {
            Provider = new List<Provider>();
        }

        public List<Provider> Provider { get; set; }
    }

    public class Provider
    {
        public Provider()
        {
            Notification = new List<Notification>();
            Service = new List<Service>();
        }

        [JsonProperty("@code")]
        public string Code { get; set; }
        public List<Notification> Notification { get; set; }
        public List<Service> Service { get; set; }
    }

    public class Notification
    {
        [JsonProperty("@code")]
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Service
    {
        public Service()
        {
            TotalNet = new TotalNet();
            //Charges = new Charges();
        }

        [JsonProperty("@type")]
        public string Type { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime CutoffTime { get; set; }
        public string NextBusinessDayInd { get; set; }
        public TotalNet TotalNet { get; set; }
        //public Charges Charges { get; set; }
    }

    public class TotalNet
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class Charges
    {
        public Charges()
        {
            Charge = new List<ChargeDTO>();
        }
        public string Currency { get; set; }
        public List<ChargeDTO> Charge { get; set; }
    }

    public class Charge
    {
        public string ChargeCode { get; set; }
        public string ChargeType { get; set; }
        public decimal ChargeAmount { get; set; }
    }

    public class ChargeDTO
    {
        public string ChargeCode { get; set; }
        public string ChargeType { get; set; }
        public decimal ChargeAmount { get; set; }
    }

}
