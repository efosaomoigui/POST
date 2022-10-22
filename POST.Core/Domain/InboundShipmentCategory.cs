using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.Domain
{
    public class InboundShipmentCategory :  BaseDomain
    {
        [Key]
        public string InboundShipmentCategoryId { get; set; } = Guid.NewGuid().ToString();
        public int ShipmentCategoryId { get; set; }
        public int CountryId { get; set; }
        public bool IsGoStandard { get; set; }
        public bool IsGoFaster { get; set; }

    }
}
