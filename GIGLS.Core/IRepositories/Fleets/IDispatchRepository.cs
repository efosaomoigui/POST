using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Fleets
{
    public interface IDispatchRepository : IRepository<Dispatch>
    {
        Task<List<DispatchDTO>> GetDispatchAsync(int[] serviceCentreIds);
        Task<List<DispatchDTO>> CheckForOutstandingDispatch(string driverId);
        Task<List<DispatchDTO>> GetDeliveryDispatchForPartner(string userId);
        Task<List<DispatchDTO>> GetPickupForDeliveryDispatchForPartner(string userId);
    }
}
