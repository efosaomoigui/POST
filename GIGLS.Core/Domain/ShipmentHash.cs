using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
