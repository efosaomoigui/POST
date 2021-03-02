using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class ReferrerCode : BaseDomain, IAuditable
    {
        public int ReferrerCodeId { get; set; }

        [MaxLength(50)]
        public string Referrercode { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(100)]
        public string UserCode { get; set; }
    }
}
