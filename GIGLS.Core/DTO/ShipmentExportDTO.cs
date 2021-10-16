using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
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
    }
}
