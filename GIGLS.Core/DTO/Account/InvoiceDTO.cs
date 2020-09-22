using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.PaymentTransactions;
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
        public bool IsInternational { get; set; }
        public int ServiceCentreId { get; set; }
        public bool IsShipmentCollected { get; set; }
        public int CountryId { get; set; }

        public ServiceCenreDTO ServiceCentre { get; set; }

        //public virtual List<Shipment> Shipments { get; set; }
        public ShipmentDTO Shipment { get; set; }
        public CustomerDTO Customer { get; set; }

        public PaymentPartialTransactionProcessDTO PaymentPartialTransaction { get; set; }
        public string PaymentTypeReference { get; set; }

        public CountryDTO Country { get; set; }
        public decimal Cash { get; set; }
        public decimal Transfer { get; set; }
        public decimal Pos { get; set; }
        public decimal HighValueAmount { get; set; }
    }
}
