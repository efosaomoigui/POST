using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.DHL
{
    public class InternationalShipmentWaybill : BaseDomain
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        [MaxLength(100)]
        public string ShipmentIdentificationNumber { get; set; }
        public string PackageResult { get; set; }
        public InternationalShipmentStatus InternationalShipmentStatus { get; set; }
        public string ResponseResult { get; set; }
        public CompanyMap OutBoundChannel { get; set; }
        public bool IsFromMobile { get; set; }

        //[MaxLength(10)]
        //public string ImageFormat { get; set; }
        //public string GraphicImage { get; set; }
    }
}
