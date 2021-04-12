using Newtonsoft.Json;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSShipmentResponseFinalPayload
    {
        public UPSShipmentResponsePayload ShipmentResponse { get; set; }
        public FaultPayload Fault { get; set; }
    }

    //success response
    public class UPSShipmentResponsePayload
    {
        public UPSShipmentResponsePayload()
        {
            Response = new UPSShipmentResponse();
            ShipmentResults = new UPSShipmentResults();
        }
        public UPSShipmentResponse Response { get; set; }
        public UPSShipmentResults ShipmentResults { get; set; }
    }

    public class UPSShipmentResponse
    {
        public UPSShipmentResponse()
        {
            Response = new UPSResponse();
        }
        public UPSResponse Response { get; set; }
    }

    public class UPSResponse
    {
        public UPSResponse()
        {
            ResponseStatus = new UPSResponseStatus();
        }

        public UPSResponseStatus ResponseStatus { get; set; }
    }

    public class UPSResponseStatus
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class UPSShipmentResults
    {
        public UPSShipmentResults()
        {
            BillingWeight = new UPSPackaging();
            ShipmentCharges = new UPSShipmentCharges();
            PackageResults = new List<UPSPackageResults>();
        }
        public UPSShipmentCharges ShipmentCharges { get; set; }
        public UPSPackaging BillingWeight { get; set; }
        public string ShipmentIdentificationNumber { get; set; }

        //the result can be an object and array too
        public List<UPSPackageResults> PackageResults { get; set; }
    }

    public class UPSShipmentCharges
    {
        public UPSShipmentCharges()
        {
            TransportationCharges = new UPSCharges();
            ServiceOptionsCharges = new UPSCharges();
            TotalCharges = new UPSCharges();
        }
        public UPSCharges TransportationCharges { get; set; }
        public UPSCharges ServiceOptionsCharges { get; set; }
        public UPSCharges TotalCharges { get; set; }
    }

    public class UPSCharges
    {
        public string CurrencyCode { get; set; }
        public string MonetaryValue { get; set; }
    }

    public class UPSPackageResults
    {
        public UPSPackageResults()
        {
            ServiceOptionsCharges = new UPSCharges();
            ShippingLabel = new UPSShippingLabel();
        }
        public string TrackingNumber { get; set; }
        public UPSCharges ServiceOptionsCharges { get; set; }
        public UPSShippingLabel ShippingLabel { get; set; }
    }

    public class UPSShippingLabel
    {
        public UPSShippingLabel()
        {
            ImageFormat = new LabelImageFormat();
        }
        public LabelImageFormat ImageFormat { get; set; }
        public string GraphicImage { get; set; }
        public string HTMLImage { get; set; }
    }

    //failed Response for UPS

    public class FaultPayload
    {
        public FaultPayload()
        {
            Detail = new FaultDetail();
        }

        [JsonProperty("faultcode")]
        public string FaultCode { get; set; }

        [JsonProperty("faultstring")]
        public string FaultString { get; set; }

        public FaultDetail Detail { get; set; }
    }

    public class FaultDetail
    {
        public FaultDetail()
        {
            Errors = new UPSErrors();
        }

        public UPSErrors Errors { get; set; }
    }

    public class UPSErrors
    {
        public UPSErrors()
        {
            ErrorDetail = new DetailErrors();
        }
        public DetailErrors ErrorDetail { get; set; }
    }

    public class DetailErrors
    {
        public DetailErrors()
        {
            PrimaryErrorCode = new UPSPrimaryErrorCode();
        }

        [JsonProperty("Severity")]
        public string Severity { get; set; }

        public UPSPrimaryErrorCode PrimaryErrorCode { get; set; }
    }

    public class UPSPrimaryErrorCode
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

}
