using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentTrackingRepository : IRepository<ShipmentTracking>
    {
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync();
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync(string waybill);
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsForMobileAsync(string waybill);
        //Task<List<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
        IQueryable<ShipmentTrackingView> GetShipmentTrackingsFromViewAsync(ScanTrackFilterCriteria f_Criteria);
        IQueryable<ShipmentTracking> GetShipmentTrackingsAsync(ScanTrackFilterCriteria f_Criteria);
    }
}
