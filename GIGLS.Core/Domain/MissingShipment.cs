namespace GIGLS.Core.Domain
{
    public class MissingShipment : BaseDomain, IAuditable
    {
        public int MissingShipmentId { get; set; }
        public string Waybill { get; set; }
        public double SettlementAmount { get; set; }
        public string Comment { get; set; }
    }
}
