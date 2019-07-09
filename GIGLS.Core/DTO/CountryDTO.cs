using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO
{
    public class CountryDTO : BaseDomainDTO
    {
        public CountryDTO()
        {
            States = new List<StateDTO>();
        }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public List<StateDTO> States { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRatio { get; set; }
    }
}
