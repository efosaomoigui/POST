using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class UnidentifiedItemsForInternationalShipping : BaseDomain, IAuditable
    {
        [Key]
        public int UnidentifiedItemsForInternationalShippingId { get; set; }
        [MaxLength(128)]
        public string TrackingNo { get; set; }
        [MaxLength(128)]
        public string CustomerName { get; set; }
        [MaxLength(128)]
        public string CustomerEmail { get; set; }

        [MaxLength(128)]
        public string CustomerPhoneNo { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        public bool IsProcessed { get; set; }
        [MaxLength(300)]
        public string ItemName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int NoOfPackageReceived { get; set; }
        public int Quantity { get; set; }
        [MaxLength(300)]
        public string ItemDescription { get; set; }
        [MaxLength(300)]
        public string StoreName { get; set; }
        [MaxLength(128)]
        public string ItemUniqueNo { get; set; }
        [MaxLength(300)]
        public string CourierService { get; set; }
        [MaxLength(300)]
        public string ItemStateDescription { get; set; }


    }
}

