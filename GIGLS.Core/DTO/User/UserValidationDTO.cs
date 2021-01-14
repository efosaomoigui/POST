using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.User
{
    public class UserValidationDTO
    {
        public Rank Rank { get; set; }
        public string UserCode { get; set; }
        public string BVN { get; set; }
        public RankType RankType { get; set; }
    }

    public class UserValidationNewDTO
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
    }
}
