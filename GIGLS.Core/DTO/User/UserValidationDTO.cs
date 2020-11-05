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
        public string PhoneNumber { get; set; }
        public Rank Rank { get; set; }
        public string UserCode { get; set; }
    }
}
