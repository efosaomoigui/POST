using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Shipments;
using POST.Core.View;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IShipmentTrackingRepository : IRepository<ShipmentTracking>
    {
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync();
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync(string waybill);
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsForMobileAsync(string waybill);
        //Task<List<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
        IQueryable<ShipmentTrackingView> GetShipmentTrackingsFromViewAsync(ScanTrackFilterCriteria f_Criteria);
        IQueryable<ShipmentTracking> GetShipmentTrackingsAsync(ScanTrackFilterCriteria f_Criteria);
        Shipment GetShipmentByWayBill(string waybill);

    }
}
