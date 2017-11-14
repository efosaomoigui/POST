using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class StockRequestPart : BaseDomain
    {
        public int StockRequestPartId { get; set; }
        public int Quantity { get; set; }
        public int QuantitySupplied { get; set; }
        public decimal UnitPrice { get; set; }
        public string SerialNumber { get; set; }

        public int PartId { get; set; }
        public FleetPart FleetPart { get; set; }

        public int StockRequestId { get; set; }
        public virtual StockRequest StockRequest { get; set; }
    }
}