using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class CountryRateConversionDTO : BaseDomainDTO
    {
        public int CountryRateConversionId { get; set; }

        public int DepartureCountryId { get; set; }
        public CountryDTO DepartureCountry { get; set; }

        public int DestinationCountryId { get; set; }
        public CountryDTO DestinationCountry { get; set; }

        public decimal Rate { get; set; }
    }
}