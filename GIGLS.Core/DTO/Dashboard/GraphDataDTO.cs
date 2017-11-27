namespace GIGLS.Core.DTO.Dashboard
{
    public class GraphDataDTO
    {
        public int ShipmentYear { get; set; }
        public int ShipmentMonth { get; set; }
        public int CalculationDay { get; set; }
        public int TotalShipmentByMonth { get; set; }
        public decimal TotalSalesByMonth { get; set; }
    }
}