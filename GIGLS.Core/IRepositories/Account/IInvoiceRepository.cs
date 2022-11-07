using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.DTO.Report;
using POST.Core.Enums;
using POST.Core.View;
using POST.Core.View.AdminReportView;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync(int[] serviceCentreIds);
        Task<List<InvoiceDTO>> GetInvoicesAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds); 
        IQueryable<InvoiceView> GetAllFromInvoiceView();
        IQueryable<CustomerView> GetAllFromCustomerView();
        IQueryable<InvoiceView> GetInvoicesForReminderAsync();
        IQueryable<InvoiceView> GetAllFromInvoiceAndShipments();
        IQueryable<InvoiceView> GetAllInvoiceShipments();
        IQueryable<InvoiceView> GetCustomerTransactions();
        IQueryable<InvoiceView> GetCustomerInvoices();
        Task<List<InvoiceViewDTO>> GetInvoicesFromViewWithDeliveryTimeAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSPExpected(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGroupedx(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceMonitorDTO>> GetShipmentWaitingForCollection(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentWaitingForCollection_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<string> VerifyPayment(string waybill);

        //Admin Report 
        IQueryable<Report_AllTimeSalesByCountry> GetAllTimeSalesByCountry();
        IQueryable<Report_BusiestRoute> GetBusiestRoute();
        IQueryable<Report_CustomerRevenue> GetCustomerRevenue();
        IQueryable<Report_MostShippedItemByWeight> GetMostShippedItemByWeight();
        IQueryable<Report_RevenuePerServiceCentre> GetRevenuePerServiceCentre();
        IQueryable<Report_TotalServiceCentreByState> GetTotalServiceCentreByState();
        IQueryable<Report_TotalOrdersDelivered> GetTotalOrdersDelivered();
        IQueryable<InvoiceView> GetAllFromInvoiceAndShipments(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<object>> SalesPerServiceCenter(List<InvoiceView> invoice);
        Task<List<object>> MostShippedItemsByWeight(List<InvoiceView> invoice);
        Task<List<object>> CountOfCustomers(List<InvoiceView> invoice);
        Task<int> GetCountOfMonthlyOrDailyShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType);
    }
}