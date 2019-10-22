using GIGL.GIGLS.Core.Domain;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class LGADTO : BaseDomainDTO
    {
        public int LGAId { get; set; }
        public string LGAName { get; set; }
        public string LGAState { get; set; }
        public bool Status { get; set; }
    }
}
