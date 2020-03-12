using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IMobilePickUpRequestsRepository :IRepository<MobilePickUpRequests>
    {
        Task <List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsync(string userId);
        Task<List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsyncMonthly(string userId);
        Task<List<FleetMobilePickUpRequestsDTO>> GetPartnerMobilePickUpRequestsForFleetPartner(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
    }
}
