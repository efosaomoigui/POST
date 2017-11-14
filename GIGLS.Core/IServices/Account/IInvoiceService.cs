using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IInvoiceService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceDTO>> GetInvoices();
        Task<InvoiceDTO> GetInvoiceById(int invoiceId);
        Task<object> AddInvoice(InvoiceDTO invoice);
        Task UpdateInvoice(int invoiceId, InvoiceDTO invoice);
        Task RemoveInvoice(int invoiceId);
    }
}
