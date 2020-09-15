using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class StateWithHomeDeliveryDTO : BaseDomainDTO
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public  List<HomeDeliveryLocationDTO> LGAs { get; set; }
    }

}
