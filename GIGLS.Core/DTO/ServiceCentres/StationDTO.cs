using GIGL.GIGLS.Core.Domain;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class StationDTO : BaseDomainDTO
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string Country { get; set; }
        public CountryDTO CountryDTO { get; set; }

        public int SuperServiceCentreId { get; set; } 
        public ServiceCentreDTO SuperServiceCentreDTO { get; set; }
        public decimal StationPickupPrice { get; set; }
        public bool GIGGoActive { get; set; }
    }

    public class WebsiteStationDTO
    {
        public WebsiteStationDTO()
        {
            ServiceCentres = new List<WebsiteServiceCentreDTO>();
        }
        public int StationId { get; set; }
        public int StateId { get; set; }
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public bool GIGGoActive { get; set; }
        public List<WebsiteServiceCentreDTO> ServiceCentres { get; set; }

    }
}
