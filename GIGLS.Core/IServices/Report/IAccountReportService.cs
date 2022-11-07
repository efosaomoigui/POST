using POST.Core.DTO.Account;
using POST.Core.DTO.Dashboard;
using POST.Core.DTO.Report;
using POST.Core.IServices;
using POST.Core.View;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Report
{
    public interface IAccountReportService : IServiceDependencyMarker
    {
        Task<List<GeneralLedgerDTO>> GetIncomeReports(AccountFilterCriteria accountFilterCriteria);
        Task<List<GeneralLedgerDTO>> GetDemurageReports(AccountFilterCriteria accountFilterCriteria);
        Task<List<GeneralLedgerDTO>> GetExpenditureReports(AccountFilterCriteria accountFilterCriteria);
        Task<List<InvoiceDTO>> GetInvoiceReports(AccountFilterCriteria accountFilterCriteria);
        Task<List<InvoiceViewDTO>> GetInvoiceReportsFromView(AccountFilterCriteria accountFilterCriteria);
        Task<List<InvoiceViewDTO>> GetInvoiceReportsFromViewPlusDeliveryTime(AccountFilterCriteria accountFilterCriteria);
        Task<EarningsBreakdownDTO> GetEarningsBreakdown(DashboardFilterCriteria dashboardFilter);
        Task<List<FinancialReportDTO>> GetFinancialBreakdownByType(AccountFilterCriteria accountFilter);
        Task<List<WalletPaymentLogView>> GetWalletPaymentLogBreakdown(DashboardFilterCriteria dashboardFilter);
        Task<List<OutboundFinancialReportDTO>> GetFinancialBreakdownOfOutboundShipments(AccountFilterCriteria accountFilter, int queryType);
    }
}
