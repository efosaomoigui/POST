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

    public class PartnerUpdateDTO
    {
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string OldPhoneNumber { get; set; }
        public string NewPhoneNumber { get; set; }
    }
}
