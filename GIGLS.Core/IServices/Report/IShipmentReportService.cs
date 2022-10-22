using POST.Core.DTO.Account;
using POST.Core.DTO.Dashboard;
using POST.Core.DTO.OnlinePayment;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.ShipmentScan;
using POST.Core.IServices;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Report
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
        Task<CustomerInvoiceDTO> GetCoporateTransactionsByCode(DateFilterForDropOff filter);
        Task<bool> GenerateCustomerInvoice(CustomerInvoiceDTO customerInvoiceDTO);
        Task<List<CustomerInvoiceDTO>> GetMonthlyCoporateTransactions();
        Task<string> GeneratePDF(CustomerInvoiceDTO customerInvoice);
        Task<bool> AddCustomerInvoice(CustomerInvoiceDTO customerInvoiceDTO);
        Task<bool> CreateNUBAN(CustomerInvoiceDTO customerInvoice);
        Task<bool> CheckIfInvoiceAlreadyExist(CustomerInvoiceDTO customerInvoice);
        Task<List<CustomerInvoiceDTO>> GetCustomerInvoiceList(DateFilterForDropOff filter);
        Task<bool> MarkInvoiceasPaid(List<CustomerInvoiceDTO> customerInvoices);
        Task<List<InvoiceViewDTO>> GetGoFasterReport(NewFilterOptionsDto filter);
        Task<List<InvoiceViewDTO>> GetGoFasterShipmentsByServiceCentre(NewFilterOptionsDto filter);
    }
}
