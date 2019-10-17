using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ICountryService : IServiceDependencyMarker
    {
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<CountryDTO> GetCountryById(int countryId);
        Task<object> AddCountry(CountryDTO countryDto);
        Task UpdateCountry(int countryId, CountryDTO countryDto);
        Task DeleteCountry(int countryId);
        Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries();
    }
}
