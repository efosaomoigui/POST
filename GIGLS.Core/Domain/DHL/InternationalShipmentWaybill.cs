using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.DHL
{
    public class InternationalShipmentWaybill
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        [MaxLength(100)]
        public string ShipmentIdentificationNumber { get; set; }
        public string PackageResult { get; set; }

        [MaxLength(10)]
        public string ImageFormat { get; set; }
        public string GraphicImage { get; set; }
    }
}
