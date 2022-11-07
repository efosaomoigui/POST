using POST.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.User
{
    public class IntertnationalUserProfilerDTO
    {
        public string IdentificationNumber { get; set; }
        public string IdentificationImage { get; set; }
        public IdentificationType IdentificationType { get; set; }
    }
}
