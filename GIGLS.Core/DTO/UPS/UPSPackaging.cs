namespace GIGLS.Core.DTO.UPS
{
    public class UPSPackaging
    {
        public UPSPackaging()
        {
            UnitOfMeasurement = new UPSUnitOfMeasurement();
        }
        public UPSUnitOfMeasurement UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }
}
