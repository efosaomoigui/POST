namespace GIGLS.Core.Domain
{
    public class DispatchActivity : BaseDomain, IAuditable
    {
        public int DispatchActivityId { get; set; }
        public int DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
