namespace GIGLS.Core.Domain
{
    public class Location : BaseDomain, IAuditable
    {
        public int LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
    }
}
