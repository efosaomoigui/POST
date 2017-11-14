using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IGroupWaybillNumberMappingRepository : IRepository<GroupWaybillNumberMapping>
    {
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings();
    }
}
