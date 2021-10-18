using GIGLS.Core.Domain;
using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO.DHL
{
    public class InternationalShipmentWaybillDTO : BaseDomain
    {
        public int Id { get; set; }
        public string Waybill { get; set; }
        public string ShipmentIdentificationNumber { get; set; }
        public string PackageResult { get; set; }
        public InternationalShipmentStatus InternationalShipmentStatus { get; set; }
        public string ResponseResult { get; set; }
        public CompanyMap OutBoundChannel { get; set; }
        public string PdfFormat { get; set; }
        public bool IsFromMobile { get; set; }

        //public string ImageFormat { get; set; }
        //public string GraphicImage { get; set; }
    }

    public class ShipmentReference
    {
        public string ShipmentTrackingNumber { get; set; }
        public string BaseTrackingUrl { get; set; }
        public int ReferenceNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
        public string ImageFormat { get; set; }
        public string Content { get; set; }
        public string TypeCode { get; set; }
        public CompanyMap OutBoundChannel { get; set; }
        public InternationalShipmentStatus InternationalShipmentStatus { get; set; }
    }
}