using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class CustomerInvoice : BaseDomain, IAuditable
    {
        public CustomerInvoice()
        {
            InvoiceCharges = new List<InvoiceCharge>();
        }
        [Key]
        public int CustomerInvoiceId { get; set; }
        [MaxLength(128)]
        public string InvoiceRefNo { get; set; }
        [MaxLength(128)]
        public string CustomerName { get; set; }
        [MaxLength(128)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        public decimal Total { get; set; }
        public decimal TotalVat { get; set; }
        public string Waybills { get; set; }
        [MaxLength(128)]
        public string UserID { get; set; }
        [MaxLength(128)]
        public string CreatedBy { get; set; }
        [MaxLength(128)]
        public string CustomerCode { get; set; }
        public virtual List<InvoiceCharge> InvoiceCharges { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

    public class InvoiceCharge : BaseDomain, IAuditable
    {

        public int CustomerInvoiceId { get; set; }
        [Key]
        public int InvoiceChargeId { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }

    }
}
