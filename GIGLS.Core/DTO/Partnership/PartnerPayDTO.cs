using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerPayDTO
    {
        public string Distance { get; set; }
        public string Time { get; set; }

        public decimal ShipmentPrice { get; set; }

        public decimal PickUprice { get; set; }

        public int ZoneMapping { get; set; }
    }
}
