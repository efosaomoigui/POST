using System.Collections.Generic;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSShipmentResponsePayload
    {
        public UPSShipmentResponsePayload()
        {
            ShipmentResponse = new UPSShipmentResponse();
            ShipmentResults = new UPSShipmentResults();
        }
        public UPSShipmentResponse ShipmentResponse { get; set; }
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
}
