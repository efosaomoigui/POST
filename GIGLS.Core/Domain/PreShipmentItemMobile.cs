using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.Domain
{
    public class PreShipmentItemMobile : BaseDomain, IAuditable
    {
        public int PreShipmentItemMobileId { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ItemType { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set;}
        public decimal EstimatedPrice { get; set; }

        public string Value { get; set; }

        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }

        //Foreign key information
        public int PreShipmentMobileId { get; set; }
        public virtual PreShipmentMobile PreShipment { get; set; }
         
       //Agility Calculations
        public decimal? CalculatedPrice { get; set; }
        public int? SpecialPackageId { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public bool IsCancelled { get; set; }
        public string PictureName { get; set; }

        public DateTime? PictureDate { get; set; }
        public string WeightRange { get; set; }
    }
}
