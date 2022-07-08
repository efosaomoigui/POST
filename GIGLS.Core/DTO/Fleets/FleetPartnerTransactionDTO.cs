using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetPartnerTransactionDTO
    {
        public DateTime DateOfEntry { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }
    }
}
