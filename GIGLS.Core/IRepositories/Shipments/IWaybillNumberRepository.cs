using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IWaybillNumberRepository : IRepository<WaybillNumber>
    {
        Task<List<WaybillNumberDTO>> GetWaybills();
    }
}
