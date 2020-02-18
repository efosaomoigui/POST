using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public  class Partnerdto
    {
        public List<MobilePickUpRequestsDTO> MonthlyDelivery { get; set; }

        public int TotalDelivery { get; set; }

        public decimal MonthlyTransactions { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
    }
}
