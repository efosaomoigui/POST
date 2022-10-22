using GIGL.POST.Core.Domain;
using POST.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.Domain
{
    public class FleetPartnerTransaction : BaseDomain
    {
        public int FleetPartnerTransactionId { get; set; }

        public int FleetId { get; set; }
        [ForeignKey("FleetId")]
        public virtual Fleet Fleet { get; set; }

        public string FleetRegistrationNumber { get; set; }

        public DateTime DateOfEntry { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }

        [MaxLength(100)]
        public string MovementManifestNumber { get; set; }
        public PaymentType PaymentType { get; set; }

        [MaxLength(100)]
        public string PaymentTypeReference { get; set; }
        
        public int TransactionCountryId { get; set; }
        public bool IsSettled { get; set; }
    }
}
