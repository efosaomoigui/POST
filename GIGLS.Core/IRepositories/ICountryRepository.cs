using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<CountryDTO> GetCountryByServiceCentreId(int serviceCentreId);

        Task<CountryDTO> GetCountryByStationId(int stationId);
    }
}
