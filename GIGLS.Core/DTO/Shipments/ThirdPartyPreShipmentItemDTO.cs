using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.Shipments
{
    public class ThirdPartyPreShipmentItemDTO
    {
        public int PreShipmentItemId { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public string Nature { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        public string Description_s { get; set; }
        public ShipmentType ShipmentType { get; set; }

        public int SerialNumber { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        //Foreign key information
        public int PreShipmentId { get; set; }

        //Agility Calculations
        public decimal CalculatedPrice { get; set; }

    }
}
