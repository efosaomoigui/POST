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
        //public string ImageFormat { get; set; }
        //public string GraphicImage { get; set; }
    }
}