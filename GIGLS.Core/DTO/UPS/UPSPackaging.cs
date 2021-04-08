namespace GIGLS.Core.DTO.UPS
{
    public class UPSPackaging
    {
        public UPSPackaging()
        {
            UnitOfMeasurement = new LabelDescription();
        }
        public LabelDescription UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }
}
