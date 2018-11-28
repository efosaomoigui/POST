using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Core.Domain
{
    public class MissingShipment : BaseDomain, IAuditable
    {
        public int MissingShipmentId { get; set; }
        public string Waybill { get; set; }
        public double SettlementAmount { get; set; }
        public string Comment { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public string CreatedBy { get; set; }        
        public string ResolvedBy { get; set; }
        public int ServiceCentreId { get; set; }
    }
}
