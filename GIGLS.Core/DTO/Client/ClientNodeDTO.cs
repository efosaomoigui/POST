using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Client
{
    public class ClientNodeDTO : BaseDomainDTO
    {
        public string ClientNodeId { get; set; }
        public string Base64Secret { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
