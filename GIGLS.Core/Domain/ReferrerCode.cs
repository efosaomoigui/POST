namespace GIGLS.Core.Domain
{
    public class ReferrerCode : BaseDomain, IAuditable
    {
        public int ReferrerCodeId { get; set; }

        public string Referrercode { get; set; }

        public string UserId { get; set; }

        public string UserCode { get; set; }
    }
}
