using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Dashboard
{
    public interface IDashboardService : IServiceDependencyMarker
    {
        Task<DashboardDTO> GetDashboard();
        Task<DashboardDTO> GetDashboard(DashboardFilterCriteria dashboardFilterCriteria);
    }
}