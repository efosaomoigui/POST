using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class PreShipmentItemMobile : BaseDomain, IAuditable
    {
        public int PreShipmentItemMobileId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public double Weight { get; set; }

        [MaxLength(100)]
        public string ItemType { get; set; }

        [MaxLength(100)]
        public string ItemCode { get; set; }

        [MaxLength(100)]
        public string ItemName { get; set;}
        public decimal EstimatedPrice { get; set; }

        [MaxLength(100)]
        public string Value { get; set; }

        [MaxLength(500)]
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

        [MaxLength(100)]
        public string PictureName { get; set; }

        public DateTime? PictureDate { get; set; }

        [MaxLength(100)]
        public string WeightRange { get; set; }
        public InternationalShipmentItemCategory InternationalShipmentItemCategory { get; set; }

    }
}
