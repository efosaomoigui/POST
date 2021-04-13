using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class PreShipment : BaseDomain, IAuditable
    {
        [Key]
        public int PreShipmentId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string TempCode { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        
        [MaxLength(100)]
        public string CompanyType { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }

        //General Details comes with role user
        [MaxLength(128)]
        public string SenderUserId { get; set; }

        [MaxLength(100)]
        public string SenderCity { get; set; }

        [MaxLength(100)]
        public string SenderName { get; set; }

        [MaxLength(100)]
        public string SenderPhoneNumber { get; set; }

        //Receivers Information
        [MaxLength(100)]
        public string ReceiverName { get; set; }

        [MaxLength(100)]
        public string ReceiverPhoneNumber { get; set; }

        [MaxLength(500)]
        public string ReceiverAddress { get; set; }

        [MaxLength(100)]
        public string ReceiverCity { get; set; }

        [MaxLength(100)]
        public string LGA { get; set; }

        public PickupOptions PickupOptions { get; set; }

        public decimal Value { get; set; }

        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }
        public int DestinationServiceCenterId { get; set; }

        //Shipment Items
        public virtual List<PreShipmentItem> PreShipmentItems { get; set; }
        public double ApproximateItemsWeight { get; set; }

        public decimal GrandTotal { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsAgent { get; set; }
        public bool IsActive { get; set; }
    }
}