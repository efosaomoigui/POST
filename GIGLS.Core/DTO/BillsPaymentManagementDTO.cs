using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class BillsPaymentManagementDTO 
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal PurchasedAmount { get; set; }
        public DateTime PurchasedDate { get; set; }
        public int FraudRating { get; set; }
    }
}
