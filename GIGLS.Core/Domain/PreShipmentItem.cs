using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class PreShipmentItem : BaseDomain
    {
        public int PreShipmentItemId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Description_s { get; set; } 
        public ShipmentType  ShipmentType { get; set; }
        public double Weight { get; set; }

        [MaxLength(100)]
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int? SpecialPackageId { get; set; }
        public decimal ItemValue { get; set; }

        //Foreign key information
        public int PreShipmentId { get; set; }
        public virtual PreShipment PreShipment { get; set; }

        //Agility Calculations
        public decimal CalculatedPrice { get; set; }
    }
}