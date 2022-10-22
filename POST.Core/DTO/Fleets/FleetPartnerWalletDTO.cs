using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Fleets
{
    public class FleetPartnerWalletDTO
    {
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal CurrentDayIncome { get; set; }
    }
}
