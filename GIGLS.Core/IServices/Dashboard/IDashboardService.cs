using POST.Core.DTO.Dashboard;
using POST.Core.DTO.Report;
using System.Threading.Tasks;

namespace POST.Core.IServices.Dashboard
{
    public interface IDashboardService : IServiceDependencyMarker
    {
        Task<DashboardDTO> GetDashboard();
        Task<DashboardDTO> GetDashboard(DashboardFilterCriteria dashboardFilterCriteria);
    }
}