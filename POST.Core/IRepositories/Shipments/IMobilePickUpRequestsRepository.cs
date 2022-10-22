using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IMobilePickUpRequestsRepository :IRepository<MobilePickUpRequests>
    {
        Task <List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsync(string userId);
        Task<List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsyncMonthly(string userId);
        Task<List<FleetMobilePickUpRequestsDTO>> GetPartnerMobilePickUpRequestsForFleetPartner(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode);
        Task<PartnerDTO> GetPartnerDetailsForAWaybill(string waybill);
    }
}
