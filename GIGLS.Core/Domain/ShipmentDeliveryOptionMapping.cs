using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class ShipmentDeliveryOptionMapping : BaseDomain, IAuditable
    {
        [Key]
        public int ShipmentDeliveryOptionMappingId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index]
        public string Waybill { get; set; }
        
        public int DeliveryOptionId { get; set; }
        public virtual DeliveryOption DeliveryOption { get; set; }
    }
}