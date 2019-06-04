using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IMobilePickUpRequestsRepository :IRepository<MobilePickUpRequests>
    {
        Task <List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsync(string userId);
    }
}
