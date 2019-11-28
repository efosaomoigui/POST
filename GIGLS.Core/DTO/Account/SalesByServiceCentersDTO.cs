using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Account
{
    public class SalesByServiceCentersDTO : BaseDomainDTO
    {
        public int ServiceCenterId { get; set; }
        public ServiceCentreDTO Name { get; set; }
        public decimal Revenue { get; set; }
    }

}
