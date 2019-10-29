namespace GIGLS.Core.Domain
{
    public class LGA : BaseDomain , IAuditable
    {
        public int LGAId { get; set; }
        public string LGAName { get; set; }
        public string LGAState { get; set; }
        public bool Status { get; set; }
       
    }
}
