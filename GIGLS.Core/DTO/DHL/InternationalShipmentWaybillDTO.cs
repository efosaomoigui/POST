using GIGLS.Core.Domain;

namespace GIGLS.Core.DTO.DHL
{
    public class InternationalShipmentWaybillDTO : BaseDomain
    {
        public int Id { get; set; }
        public string Waybill { get; set; }
        public string ShipmentIdentificationNumber { get; set; }
        public string PackageResult { get; set; }
        //public string ImageFormat { get; set; }
        //public string GraphicImage { get; set; }
    }
}