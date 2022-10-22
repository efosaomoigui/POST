using POST.Core.DTO;
using POST.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface ICountryService : IServiceDependencyMarker
    {
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<IEnumerable<CountryDTO>> GetActiveCountries();
        Task<CountryDTO> GetCountryById(int countryId);
        Task<object> AddCountry(CountryDTO countryDto);
        Task UpdateCountry(int countryId, CountryDTO countryDto);
        Task DeleteCountry(int countryId);
        Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries();
        Task<IEnumerable<CountryDTO>> GetIntlShipingCountries();
        Task<UserActiveCountryDTO> UpdateUserActiveCountry(UpdateUserActiveCountryDTO userActiveCountry);
    }
}
