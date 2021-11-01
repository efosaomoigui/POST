using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class PlaceLocation : BaseDomain, IAuditable
    {
        public int PlaceLocationId { get; set; }
        [MaxLength(100)]
        public string PlaceLocationName { get; set; }
        [MaxLength(100)]
        public string StateName { get; set; }
        public int StateId { get; set; }
        [MaxLength(100)]
        public string BaseStation { get; set; }
        public int BaseStationId { get; set; }
        public bool IsHomeDelivery { get; set; }
        public bool IsNormalHomeDelivery { get; set; }
        public bool IsExpressHomeDelivery { get; set; }
        public bool IsExtraMileDelivery { get; set; }
        public bool IsGIGGO { get; set; }
    }
}
