using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.View;
using GIGLS.Core.View.AdminReportView;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace GIGLS.Core.IRepositories.Account
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

        //Admin Report 
        IQueryable<Report_AllTimeSalesByCountry> GetAllTimeSalesByCountry();
        IQueryable<Report_BusiestRoute> GetBusiestRoute();
        IQueryable<Report_CustomerRevenue> GetCustomerRevenue();
        IQueryable<Report_MostShippedItemByWeight> GetMostShippedItemByWeight();
        IQueryable<Report_RevenuePerServiceCentre> GetRevenuePerServiceCentre();
        IQueryable<Report_TotalServiceCentreByState> GetTotalServiceCentreByState();
    }
}