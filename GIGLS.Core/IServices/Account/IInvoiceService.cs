using GIGLS.Core.DTO.Account;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IInvoiceService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceDTO>> GetInvoices();
        Task<Tuple<List<InvoiceDTO>, int>> GetInvoices(FilterOptionsDto filterOptionsDto);
        Task<InvoiceDTO> GetInvoiceById(int invoiceId);
        Task<InvoiceDTO> GetInvoiceByWaybill(string waybill); 
        Task<object> AddInvoice(InvoiceDTO invoice);
        Task UpdateInvoice(int invoiceId, InvoiceDTO invoice);
        Task RemoveInvoice(int invoiceId);
        Task<string> SendEmailForDueInvoices(int daystoduedate);
        Task<string> SendEmailForWalletBalanceCheck(decimal daystoduedate);
    }
}
