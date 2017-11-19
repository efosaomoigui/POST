using System;

namespace GIGLS.Core.DTO.Dashboard
{
    public class ShipmentOrderDTO
    {
        public string Waybill { get; set; }
        public string Customer { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}