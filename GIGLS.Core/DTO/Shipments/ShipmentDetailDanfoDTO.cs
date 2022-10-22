using System;

namespace POST.Core.DTO.Shipments
{
    public class ShipmentDetailDanfoDTO
    {
        public string Waybill { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
