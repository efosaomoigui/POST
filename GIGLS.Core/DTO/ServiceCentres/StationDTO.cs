using GIGLS.CORE.DTO;

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
    }
}
