using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IGroupWaybillNumberRepository : IRepository<GroupWaybillNumber>
    {
        Task<List<GroupWaybillNumberDTO>> GetGroupWaybills();
    }

    public interface IMovementManifestNumberRepository : IRepository<MovementManifestNumber>  
    {
        Task<List<MovementManifestNumberDTO>> GetMovementManifests(); 
    }
}
