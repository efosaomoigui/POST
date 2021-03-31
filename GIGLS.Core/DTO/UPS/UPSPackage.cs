using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSPackage
    {
        public UPSPackage()
        {
            Packaging = new UPSPackaging();
            Dimensions = new UPSDimensions();
            PackageWeight = new UPSPackageWeight();
        }

        [MaxLength(50, ErrorMessage = "Item Description cannot be more than 50 characters")]
        public string Description { get; set; }
        public UPSPackaging Packaging { get; set; }
        public UPSDimensions Dimensions { get; set; }
        public UPSPackageWeight PackageWeight { get; set; }
    }

    public class UPSPackageWeight
    {
        public string Code { get; set; } = "02";
        public string Description { get; set; }
    }

    public class UPSDimensions
    {
        public UPSUnitOfMeasurement UnitOfMeasurement { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class UPSPackaging
    {
        public UPSUnitOfMeasurement UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }

    public class UPSUnitOfMeasurement
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
