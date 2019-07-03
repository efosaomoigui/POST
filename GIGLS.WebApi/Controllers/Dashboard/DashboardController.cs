using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Dashboard;
using GIGLS.Core.IServices.User;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Dashboard
{
    //[Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseWebApiController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IUserService _userService;
        public DashboardController(IDashboardService dashboardService, IUserService userService) :base(nameof(DashboardController))
        {
            _dashboardService = dashboardService;
            _userService = userService;
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

                //set user active country
                var countries = await _userService.GetPriviledgeCountrys();
                if (countries.Count == 1)
                {
                    dashboard.UserActiveCountry = countries[0];
                }

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }
    }
}
