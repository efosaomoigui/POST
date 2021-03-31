namespace GIGLS.Core.DTO.UPS
{
    public class LabelSpecification
    {
        public LabelSpecification()
        {
            LabelImageFormat = new LabelImageFormat();
        }
        public LabelImageFormat LabelImageFormat { get; set; }
        public string HTTPUserAgent { get; set; } = "Mozilla/4.5";
    }

    public class LabelImageFormat
    {
        public string Code { get; set; } = "GIF";
        public string Description { get; set; } = "GIF";
    }
}
