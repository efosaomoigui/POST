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
        public int FleetPartnerTransactionId { get; set; }

        public int FleetId { get; set; }
        public string FleetRegistrationNumber { get; set; }
        public DateTime DateOfEntry { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }

        public string MovementManifestNumber { get; set; }
        public PaymentType PaymentType { get; set; }

        public string PaymentTypeReference { get; set; }

        public int TransactionCountryId { get; set; }
        public bool IsSettled { get; set; }
    }
}
