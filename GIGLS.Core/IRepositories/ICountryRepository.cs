using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<CountryDTO> GetCountryByServiceCentreId(int serviceCentreId);

        Task<CountryDTO> GetCountryByStationId(int stationId);
        Task<List<CountryByStationDTO>> GetCountryByStationId(int[] stationId);
        Task<List<CountryDTO>> GetCountries(int[] countryId);
    }
}
