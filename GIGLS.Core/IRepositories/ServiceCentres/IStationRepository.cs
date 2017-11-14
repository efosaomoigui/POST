using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IStationRepository : IRepository<Station>
    {
        Task<List<Station>> GetStationsAsync();
    }
}
