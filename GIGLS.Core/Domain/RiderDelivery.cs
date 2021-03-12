using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class RiderDelivery : BaseDomain, IAuditable
    {
        [Key]
        public int RiderDeliveryId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        [MaxLength(128)]
        public string DriverId { get; set; }

        public decimal CostOfDelivery { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }

        [MaxLength(100)]
        public string Area { get; set; }
    }
}