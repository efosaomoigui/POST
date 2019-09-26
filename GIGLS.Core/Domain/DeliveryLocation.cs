using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class DeliveryLocation : BaseDomain, IAuditable
    {
        [Key]
        public int DeliveryLocationId { get; set; }
        public string Location { get; set; }         
        public decimal Tariff { get; set; }
    }
}
