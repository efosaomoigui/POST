using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
 
namespace GIGL.GIGLS.Core.Domain
{
    public class MagayaShipmentItem : BaseDomain
    {
        public int MagayaShipmentItemId { get; set; } 
        public string Description { get; set; }
        public string Description_s { get; set; }
        public double Weight { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }

        //To handle volumetric weight
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double VolumeWeight { get; set; }

        //Foreign key information
        public int MagayaShipmentId { get; set; } 
        public virtual MagayaShipmentAgility MagayaShipment { get; set; } 
    }
}