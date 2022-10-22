using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.Domain
{
    public class ShipmentCategory : BaseDomain
    {
        [Key]
        public int ShipmentCategoryId { get; set; }
        public string ShipmentCategoryName { get; set; }
    }
}
