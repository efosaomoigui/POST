using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface ICountryRateConversionService : IServiceDependencyMarker
    {
        Task<IEnumerable<CountryRateConversionDTO>> GetCountryRateConversion();
        Task<CountryRateConversionDTO> GetCountryRateConversionById(int countryRateConversionId);
        Task<decimal> GetCountryRateConversionRate(int departureCountryId, int destinationCountryId);
        Task<object> AddCountryRateConversion(CountryRateConversionDTO countryRateConversionId);
        Task UpdateCountryRateConversion(int countryRateConversionId, CountryRateConversionDTO countryRateConversion);
        Task DeleteCountryRateConversion(int countryRateConversionId);
    }
}