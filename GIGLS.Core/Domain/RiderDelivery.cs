using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class RiderDelivery : BaseDomain, IAuditable
    {
        [Key]
        public int RiderDeliveryId { get; set; }
        public string Waybill { get; set; }
        public string DriverId { get; set; }
        public decimal CostOfDelivery { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Area { get; set; }
    }
}
