using POST.Core.Domain;
using POST.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class ShipmentExportDTO: BaseDomain
    {
        public int ShipmentExportId { get; set; }
        public string RequestNumber { get; set; }
        public string Waybill { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public string ItemUniqueNo { get; set; }
        public string CourierService { get; set; }
        public bool IsExported { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ItemState ItemState { get; set; }
        public string ItemName { get; set; }
        public string ItemRequestCode { get; set; }
        public int NoOfPackageReceived { get; set; }
        public string Description { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DeclaredValue { get; set; }
        public string CustomerName { get; set; }
        public decimal ItemValue { get; set; }
    }
}
