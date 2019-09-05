using GIGL.GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain
{
    public class DeliveryOptionPrice : BaseDomain, IAuditable
    {
        public int DeliveryOptionPriceId { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        public int DeliveryOptionId { get; set; }
        public virtual DeliveryOption DeliveryOption { get; set; }

        public decimal Price { get; set; }

        public int CountryId { get; set; }
    }
}
