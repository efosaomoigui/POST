using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.User
{
    public class GIGXUserDetailDTO : BaseDomain
    {
        public int GIGXUserDetailId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPin { get; set; }
        public string WalletAddress { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }


    public class GIGXUserPinDTO 
    {
        public bool HasPin { get; set; }
        public GIGXUserDetailDTO GIGXUserDetailDTO { get; set; }
    }

}
