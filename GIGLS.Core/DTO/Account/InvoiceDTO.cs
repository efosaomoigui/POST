using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Account
{
    public class InvoiceDTO : BaseDomainDTO
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Waybill { get; set; }
        public DateTime DueDate { get; set; }

        //public virtual List<Shipment> Shipments { get; set; }
        public ShipmentDTO Shipment { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
