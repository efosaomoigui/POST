using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Archived;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.View;
using GIGLS.Core.View.AdminReportView;
using GIGLS.Core.View.Archived;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Archived
{
    public interface IInvoiceArchiveRepository : IRepository<Invoice_Archive>
    {
        Task<List<InvoiceDTO>> GetInvoicesAsync(int[] serviceCentreIds);
        Task<List<InvoiceDTO>> GetInvoicesAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        IQueryable<InvoiceArchiveView> GetAllFromInvoiceArchiveView();
        
        IQueryable<InvoiceArchiveView> GetInvoicesForReminderAsync();
        IQueryable<InvoiceArchiveView> GetAllFromInvoiceAndShipments();
        IQueryable<InvoiceArchiveView> GetAllInvoiceShipments();
        IQueryable<InvoiceArchiveView> GetCustomerTransactions();
        IQueryable<InvoiceArchiveView> GetCustomerInvoices();
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewWithDeliveryTimeAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSPExpected(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGroupedx(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentWaitingForCollection(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentWaitingForCollection_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);

        //Admin Report 
        IQueryable<Report_AllTimeSalesByCountry> GetAllTimeSalesByCountry();
        IQueryable<Report_BusiestRoute> GetBusiestRoute();
        IQueryable<Report_CustomerRevenue> GetCustomerRevenue();
        IQueryable<Report_MostShippedItemByWeight> GetMostShippedItemByWeight();
        IQueryable<Report_RevenuePerServiceCentre> GetRevenuePerServiceCentre();
        IQueryable<Report_TotalServiceCentreByState> GetTotalServiceCentreByState();
        IQueryable<Report_TotalOrdersDelivered> GetTotalOrdersDelivered();
        IQueryable<InvoiceArchiveView> GetAllFromInvoiceAndShipments(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<object>> SalesPerServiceCenter(List<InvoiceArchiveView> invoice);
        Task<List<object>> MostShippedItemsByWeight(List<InvoiceArchiveView> invoice);
        Task<List<object>> CountOfCustomers(List<InvoiceArchiveView> invoice);
        Task<int> GetCountOfMonthlyOrDailyShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType);
    }
}
