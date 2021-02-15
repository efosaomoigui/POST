using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Archived
{
    public class Invoice_Archive : BaseDomain_Archive
    {
        [Key]
        public int InvoiceId { get; set; }

        [MaxLength(100)]
        public string InvoiceNo { get; set; }

        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        [MaxLength(100)]
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public DateTime DueDate { get; set; }
        public int ServiceCentreId { get; set; }
        public bool IsInternational { get; set; }
        public bool IsShipmentCollected { get; set; }

        [MaxLength(100)]
        public string PaymentTypeReference { get; set; }
        public int CountryId { get; set; }
        public decimal Cash { get; set; }
        public decimal Transfer { get; set; }
        public decimal Pos { get; set; }
    }
}
