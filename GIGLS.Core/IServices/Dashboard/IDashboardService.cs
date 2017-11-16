using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Dashboard
{
    public interface IDashboardService : IServiceDependencyMarker
    {
        Task<DashboardDTO> GetDashboard();
    }
}
