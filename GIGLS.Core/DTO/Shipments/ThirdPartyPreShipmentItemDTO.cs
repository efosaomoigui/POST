using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ThirdPartyPreShipmentItemDTO
    {
        public string Description { get; set; }
        //public ShipmentType ShipmentType { get; set; }
        public double Weight { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
