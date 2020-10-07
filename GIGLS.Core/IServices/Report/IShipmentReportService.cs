using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IServices;
using GIGLS.Core.View;
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
        Task<List<ShipmentTrackingView>> GetShipmentTrackingFromView(ScanTrackFilterCriteria f_Criteria);
        Task<List<ScanStatusReportDTO>> GetShipmentTrackingFromViewReport(ScanTrackFilterCriteria f_Criteria);
        Task<DashboardDTO> GetShipmentProgressSummary(ShipmentProgressSummaryFilterCriteria baseFilterCriteria);
        Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryBreakDown(ShipmentProgressSummaryFilterCriteria baseFilterCriteria);
        Task<List<PreShipmentMobileReportDTO>> GetPreShipmentMobile(MobileShipmentFilterCriteria accountFilterCriteria);
    }
}
