using Newtonsoft.Json;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    public class ShipmentRequestResponse
    {
        public ShipmentRequestResponse()
        {
            ShipmentResponse = new ShipmentResponse();
        }
        public ShipmentResponse ShipmentResponse { get; set; }
        public string ResponseResult { get; set; }
    }

    public class ShipmentResponse
    {
        public ShipmentResponse()
        {
            Notification = new List<Notification>();
            PackagesResult = new PackagesResult();
            LabelImage = new List<LabelImage>();
        }
        //waybill
        public string ShipmentIdentificationNumber { get; set; }
        public List<Notification> Notification { get; set; }
        public PackagesResult PackagesResult { get; set; }
        public List<LabelImage> LabelImage { get; set; }
    }

    public class LabelImage
    {
        public string LabelImageFormat { get; set; }
        public string GraphicImage { get; set; }
    }

    public class PackagesResult
    {
        public PackagesResult()
        {
            PackageResult = new List<PackageResult>();
        }

        public List<PackageResult> PackageResult { get; set; }
    }

    public class PackageResult
    {
        [JsonProperty("@number")]
        public int @number { get; set; }
        public string TrackingNumber { get; set; }
    }
}
