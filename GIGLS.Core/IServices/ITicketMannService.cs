using POST.Core.DTO;
using POST.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface ITicketMannService : IServiceDependencyMarker
    {
        Task<string> GetToken();
        Task<MerchantSalesDTO> GetMerchantSalesSummary(DateFilterCriteria filter);
        Task<CustomerTransactionsDTO> GetCustomerTransactionsSummary(DateFilterCriteria filter);
        Task<string> BillTransactionRefund(string emailOrCode, decimal amount);
    }
}
