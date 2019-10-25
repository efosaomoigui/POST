using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class DeliveryLocation : BaseDomain, IAuditable
    {
        [Key]
        public int DeliveryLocationId { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }         
        public decimal Tariff { get; set; }
    }
}
