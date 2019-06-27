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
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }     
        public string IndentificationUrl { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public bool IsCashOnDelivery { get; set; }

        //Who processed the collection
        public string UserId { get; set; }
        public Demurrage Demurrage { get; set; }
    }
}