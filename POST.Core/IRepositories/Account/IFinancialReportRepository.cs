using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.DTO.Dashboard;
using POST.Core.DTO.Report;
using POST.Core.Enums;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IFinancialReportRepository : IRepository<FinancialReport>
    {
        Task<EarningsBreakdownDTO> GetEarningsBreakdown(DashboardFilterCriteria dashboardFilter);
        Task<List<FinancialReportDTO>> GetFinancialReportBreakdown(AccountFilterCriteria accountFilterCriteria);
        Task<decimal> GetTotalFinancialReportEarnings(DashboardFilterCriteria dashboardFilterCriteria);
        Task<List<FinancialReportDTO>> GetFinancialReportBreakdownForDemurrage(AccountFilterCriteria accountFilterCriteria);
        Task<decimal> GetTotalFinancialReportDemurrage(DashboardFilterCriteria dashboardFilterCriteria);
        Task<FinancialBreakdownByCustomerTypeDTO> GetFinancialSummaryByCustomerType(string procedureName, DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType);
        Task<decimal> GetTotalFinancialReportEarningsForOutboundShipments(DashboardFilterCriteria dashboardFilterCriteria, int queryType);
        Task<List<OutboundFinancialReportDTO>> GetFinancialReportOfOutboundShipmentsBreakdown(AccountFilterCriteria accountFilterCriteria, int queryType);
        Task<decimal> GetInternationalTotalEarnings(DashboardFilterCriteria dashboardFilterCriteria);
        Task<decimal> GetCorporateIncomeBreakdownSummary(DashboardFilterCriteria dashboardFilterCriteria);
        Task<IQueryable<FinancialReportDemurrageDTO>> GetTotalFinancialReportDemurrageGraph(DashboardFilterCriteria dashboardFilterCriteria);
        Task<IQueryable<TotalTerminalShipmentDTO>> GetTotalTerminalShipmentGraph(DashboardFilterCriteria dashboardFilterCriteria);
        Task<IQueryable<CorporateSalesDTO>> GetCorporateIncomeGraph(DashboardFilterCriteria dashboardFilterCriteria);
        Task<IQueryable<GiGGoIntraCityDTO>> GetTotalGIGGOIntraStateShipmentGraph(DashboardFilterCriteria dashboardFilterCriteria);
    }
}
