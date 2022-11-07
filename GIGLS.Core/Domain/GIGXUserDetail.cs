using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POST.Core.Domain
{
    public class GIGXUserDetail : BaseDomain, IAuditable
    {
        public int GIGXUserDetailId { get; set; }
        [MaxLength(128)]
        public string CustomerCode { get; set; }
        [MaxLength(7)]
        public string CustomerPin { get; set; }
        public string WalletAddress { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        [MaxLength(128)]
        public string GIGXEmail { get; set; }

    }
}

