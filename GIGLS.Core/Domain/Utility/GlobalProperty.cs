namespace GIGLS.Core.Domain.Utility
{
    public class GlobalProperty : BaseDomain, IAuditable
    {
        public int GlobalPropertyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
