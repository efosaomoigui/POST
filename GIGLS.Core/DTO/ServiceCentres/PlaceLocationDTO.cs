using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class PlaceLocationDTO : BaseDomainDTO
    {
        public int PlaceLocationId { get; set; }
        public string PlaceLocationName { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string BaseStation { get; set; }
        public int BaseStationId { get; set; }
        public bool IsHomeDelivery { get; set; }
        public bool IsNormalHomeDelivery { get; set; }
        public bool IsExpressHomeDelivery { get; set; }
        public bool IsExtraMileDelivery { get; set; }
        public bool IsGIGGO { get; set; }
    }

    public class UpdatePlaceLocationsDTO : BaseDomainDTO
    {
        public List<int> PlaceLocations { get; set; }
        public List<PlaceLocationDTO> LocationItems { get; set; }
        public string BaseStationName { get; set; }
        public int BaseStationId { get; set; }
        public bool IsHomeDelivery { get; set; }
        public bool IsNormalHomeDelivery { get; set; }
        public bool IsExpressHomeDelivery { get; set; }
        public bool IsExtraMileDelivery { get; set; }
        public bool IsGIGGO { get; set; }
    }
}
