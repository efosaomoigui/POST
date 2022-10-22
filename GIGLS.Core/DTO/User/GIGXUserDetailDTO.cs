using POST.Core.Domain;
using POST.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.User
{
    public class GIGXUserDetailDTO : BaseDomain
    {
        public int GIGXUserDetailId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPin { get; set; }
        public string WalletAddress { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string CustomerNewPin { get; set; }
        public string GIGXEmail { get; set; }
        public bool HasPin { get; set; }
    }


    public class GIGXUserPinDTO 
    {
        public bool HasPin { get; set; }
        public GIGXUserDetailDTO GIGXUserDetailDTO { get; set; }
    }

}
