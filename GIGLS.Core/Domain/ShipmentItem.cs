using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class ShipmentItem : BaseDomain
    {
        public int ShipmentItemId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Description_s { get; set; } 
        public ShipmentType  ShipmentType { get; set; }
        public double Weight { get; set; }

        [MaxLength(100)]
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }
        public int ShipmentPackagePriceId { get; set; }
        public int PackageQuantity { get; set; }

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        //Foreign key information
        public int ShipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }
    }

    public class IntlShipmentRequestItem : BaseDomain 
    {
        public int IntlShipmentRequestItemId { get; set; } 
        public string Description { get; set; }
        public string ItemName { get; set; } 
        public string TrackingId{ get; set; }  
        public string storeName{ get; set; }  
        public ShipmentType ShipmentType { get; set; }
        public double Weight { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; } 

        //To handle volumetric weight
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public string ItemSenderfullName { get; set; }

        //Foreign key information
        public int IntlShipmentRequestId { get; set; } 
        public virtual IntlShipmentRequest Shipment { get; set; }

        public bool RequiresInsurance { get; set; } 
        public decimal ItemValue { get; set; }
        [MaxLength(128)]
        public string ItemCount { get; set; }
        public bool Received { get; set; }
        [MaxLength (128)]
        public string ReceivedBy { get; set; }
        [MaxLength(300)]
        public string CourierService { get; set; }
        [MaxLength(300)]
        public string ItemUniqueNo { get; set; }
        public  DateTime? ReceivedDate { get; set; }
        public ItemState ItemState { get; set; }
        [MaxLength(128)]
        public string ItemRequestCode { get; set; }
        [MaxLength(500)]
        public string ItemStateDescription { get; set; }
        public int NoOfPackageReceived { get; set; }

    }
}