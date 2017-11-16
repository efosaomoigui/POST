using GIGLS.Core.IServices;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.IServices.Dashboard;

namespace GIGLS.WebApi.Controllers.Dashboard
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseWebApiController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService) :base(nameof(DashboardController))
        {
            _dashboardService = dashboardService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<DashboardDTO>> GetDashboard()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dashboard = await _dashboardService.GetDashboard();

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }

    }
}
