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
        IQueryable<InvoiceArchiveView> GetAllFromInvoiceView();
        
        IQueryable<InvoiceArchiveView> GetInvoicesForReminderAsync();
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

        //Admin Report 
        IQueryable<InvoiceView> GetAllFromInvoiceAndShipments(ShipmentCollectionFilterCriteria filterCriteria);
        
    }
}
