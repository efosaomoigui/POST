namespace GIGLS.Core.Domain
{
    public class Haulage : BaseDomain, IAuditable
    {
        public int HaulageId { get; set; }
        public decimal Tonne { get; set; }
        public bool Status { get; set; }
    }
}
