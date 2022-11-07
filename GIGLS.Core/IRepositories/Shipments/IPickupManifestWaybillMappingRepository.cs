using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.Fleets;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IPickupManifestWaybillMappingRepository : IRepository<PickupManifestWaybillMapping>
    {
        Task<List<PickupManifestWaybillMappingDTO>> GetPickupManifestWaybillMapping(DateFilterCriteria dateFilterCriteria);
    }
}