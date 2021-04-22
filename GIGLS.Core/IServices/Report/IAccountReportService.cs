using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Report
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
        Task<List<OutboundFinancialReportDTO>> GetFinancialBreakdownOfOutboundShipments(AccountFilterCriteria accountFilter);
    }
}
