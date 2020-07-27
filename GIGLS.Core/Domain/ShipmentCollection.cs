using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.CORE.Domain
{
    public class ShipmentCollection : BaseDomain, IAuditable
    {
        [Key]
        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(500)]
        public string IndentificationUrl { get; set; }

        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public bool IsCashOnDelivery { get; set; }

        //Who processed the collection
        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(500)]
        public string DeliveryAddressImageUrl { get; set; }
    }
}