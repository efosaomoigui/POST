using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.User
{
    public class GIGXUserDetailDTO
    {
        public int GIGXUserDetailId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPin { get; set; }
        public string WalletAddress { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

}
