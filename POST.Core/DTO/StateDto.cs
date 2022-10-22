using POST.Core.DTO.ServiceCentres;
using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO
{
    public class StateDTO : BaseDomainDTO
    {
        public StateDTO()
        {
            Stations = new List<StationDTO>();
        }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }

        public List<StationDTO> Stations { get; set; }
    }

    public class WebsiteStateDTO
    {
        public WebsiteStateDTO()
        {
            Stations = new List<WebsiteStationDTO>();
        }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }
        public List<WebsiteStationDTO> Stations { get; set; }

    }
}
