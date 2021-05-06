using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSPackage
    {
        public UPSPackage()
        {
            Packaging = new LabelDescription();
            Dimensions = new UPSDimensions();
            PackageWeight = new UPSPackageWeight();
        }

        [MaxLength(50, ErrorMessage = "Item Description cannot be more than 50 characters")]
        public string Description { get; set; }
        public LabelDescription Packaging { get; set; }
        public UPSDimensions Dimensions { get; set; }
        public UPSPackageWeight PackageWeight { get; set; }
    }

    public class UPSPackageWeight
    {
        public UPSPackageWeight()
        {
            UnitOfMeasurement = new LabelDescription();
        }

        public LabelDescription UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }

    public class UPSDimensions
    {
        public UPSDimensions()
        {
            UnitOfMeasurement = new LabelDescription();
        }
        public LabelDescription UnitOfMeasurement { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }    
}
