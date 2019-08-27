using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Fleets;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IPickupManifestWaybillMappingRepository : IRepository<PickupManifestWaybillMapping>
    {
        Task<List<PickupManifestWaybillMappingDTO>> GetPickupManifestWaybillMapping(DateFilterCriteria dateFilterCriteria);
    }
}