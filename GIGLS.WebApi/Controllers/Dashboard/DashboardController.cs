using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.IServices.Dashboard;
using GIGLS.Core.DTO.Report;

namespace GIGLS.WebApi.Controllers.Dashboard
{
    //[Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseWebApiController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService) :base(nameof(DashboardController))
        {
            _dashboardService = dashboardService;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
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

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<DashboardDTO>> GetDashboard(DashboardFilterCriteria dashboardFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dashboard = await _dashboardService.GetDashboard(dashboardFilterCriteria);

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }
    }
}
