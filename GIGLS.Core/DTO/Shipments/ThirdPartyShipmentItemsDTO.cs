using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
   public  class ThirdPartyShipmentItemsDTO
    {
        public double Weight { get; set; }
        public string Value { get; set; }
        public bool IsVolumetric { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public decimal? CalculatedPrice { get; set; }
        public int Quantity { get; set; }
    }
}
