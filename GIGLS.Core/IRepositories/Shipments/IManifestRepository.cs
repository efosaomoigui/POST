using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IManifestRepository : IRepository<Manifest>
    {
        Task<List<ManifestDTO>> GetManifests();
        Task<List<ManifestDTO>> GetManifest(List<Manifest> manifests);
        Task<List<AllManifestAndGroupWaybillDTO>> GetManifestsInSuperManifests(string code);
    }
}
