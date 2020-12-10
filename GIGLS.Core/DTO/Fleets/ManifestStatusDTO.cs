using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Fleets
{
    public class ManifestStatusDTO : BaseDomainDTO
    {
        public ManifestStatus ManifestStatus { get; set; }
        public string ManifestCode { get; set; }
    }
}
