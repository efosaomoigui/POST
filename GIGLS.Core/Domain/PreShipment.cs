using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class PreShipment : BaseDomain, IAuditable
    {

        //Shipment Information
        [Key]
        public int PreShipmentId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string TempCode { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }

        
        [MaxLength(100)]
        public string CompanyType { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }

        //General Details comes with role user
        [MaxLength(128)]
        public string SenderUserId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderCity { get; set; }
        //Receivers Information

        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public PickupOptions PickupOptions { get; set; }

        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }

        //Shipment Items
        public virtual List<PreShipmentItem> PreShipmentItems { get; set; }
        public double ApproximateItemsWeight { get; set; }

        public decimal GrandTotal { get; set; }
        public bool IsProcessed { get; set; }
    }
}