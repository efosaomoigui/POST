namespace GIGLS.Core.Domain
{
    public class InvoiceShipment : BaseDomain, IAuditable
    {
        public int InvoiceShipmentId { get; set; }
        public int InvoiceId { get; set; }
        public int ShipmentId { get; set; }
    }
}
