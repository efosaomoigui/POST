using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.BankSettlement
{
    public class BankDTO : BaseDomainDTO
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
    }
}
