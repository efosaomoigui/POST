using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Report
{
    public interface IShipmentReportService : IServiceDependencyMarker
    {
        Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria filterCriteria);
        Task<List<ShipmentDTO>> GetTodayShipments();
        Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria);
        Task<object> GetDailySalesByServiceCentreReport(DailySalesDTO dailySalesDTO);
    }
}
