using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<CountryDTO> GetCountryByServiceCentreId(int serviceCentreId);

        Task<CountryDTO> GetCountryByStationId(int stationId);
        Task<List<CountryByStationDTO>> GetCountryByStationId(int[] stationId);
        Task<List<CountryDTO>> GetCountries(int[] countryId);
    }
}
