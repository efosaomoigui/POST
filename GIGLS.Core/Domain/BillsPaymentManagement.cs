using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class BillsPaymentManagement : BaseDomain, IAuditable
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string UserId { get; set; }
        public decimal PurchasedAmount { get; set; }
        public DateTime PurchasedDate { get; set; }
    }
}
