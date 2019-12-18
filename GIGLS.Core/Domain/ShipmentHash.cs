using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class ShipmentHash : BaseDomain
    {
        [Key]
        public int ShipmentHashId { get; set; }

        [MaxLength(500)]
        public string HashedShipment { get; set; }
    }
}
