using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.View;
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
        IQueryable<InvoiceView> GetAllFromInvoiceView();
        IQueryable<CustomerView> GetAllFromCustomerView();
        Task<List<InvoiceView>> GetInvoicesForReminderAsync(double rangeofdays);
        IQueryable<InvoiceView> GetAllFromInvoiceAndShipments();
        IQueryable<InvoiceView> GetAllInvoiceShipments();
    }
}
