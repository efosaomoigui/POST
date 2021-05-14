using System.Collections.Generic;

namespace GIGLS.Core.DTO.DHL
{
    public class ShipmentResPayload
    {
        public ShipmentResPayload()
        {
            Packages = new List<ShipmentResPackage>();
            Documents = new List<ShipmentResDocument>();
        }
        public string ShipmentTrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
        public List<ShipmentResPackage> Packages { get; set; }
        public List<ShipmentResDocument> Documents { get; set; }
    }

    public class ShipmentResPackage
    {
        public int ReferenceNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
    }

    public class ShipmentResDocument
    {
        public string ImageFormat { get; set; }
        public string Content { get; set; }
        public string TypeCode { get; set; }
    }

}
