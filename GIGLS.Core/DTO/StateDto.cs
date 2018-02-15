using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO
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
}
