using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
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
