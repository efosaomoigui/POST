using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IPreShipmentManifestMappingRepository : IRepository<PreShipmentManifestMapping>
    {
        Task<List<PreShipmentManifestMappingDTO>> GetManifestWaybillMappings();
    }
}
