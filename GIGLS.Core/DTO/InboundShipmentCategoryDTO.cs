using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class InboundShipmentCategoryDTO : BaseDomainDTO
    {
        public string InboundShipmentCategoryId { get; set; }
        public string ShipmentCategoryName { get; set; }
        public string CountryName { get; set; }
        public bool IsGoStandard { get; set; }
        public bool IsGoFaster { get; set; }
    }
}
