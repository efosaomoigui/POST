using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IMobilePickUpRequestsService : IServiceDependencyMarker
    {
        Task AddMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest);
        Task UpdateMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest, string userId);
        Task<List<MobilePickUpRequestsDTO>> GetAllMobilePickUpRequests();
        Task<Partnerdto> GetMonthlyTransactions();
        Task AddOrUpdateMobilePickUpRequests(MobilePickUpRequestsDTO PickUpRequest);
        Task AddOrUpdateMobilePickUpRequests2(MobilePickUpRequestsDTO PickUpRequest, List<string> waybillList);
        Task UpdatePreShipmentMobileStatus(List<string> waybillList, string status);
        Task UpdateMobilePickUpRequestsForWaybillList(List<string> waybills, string userId, string status);
    }
}
