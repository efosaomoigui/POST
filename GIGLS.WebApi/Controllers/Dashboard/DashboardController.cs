using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Dashboard;
using GIGLS.Core.IServices.User;
using GIGLS.Services.Implementation;
using System.Linq;
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
        private ICountryService _countryService;
        public DashboardController(IDashboardService dashboardService, IUserService userService,
            ICountryService countryService) : base(nameof(DashboardController))
        {
            _dashboardService = dashboardService;
            _userService = userService;
            _countryService = countryService;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<DashboardDTO>> GetDashboard()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var countries = await _userService.GetPriviledgeCountrys();
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
                CountryDTO userActiveCountry = null;

                //set user active country from PriviledgeCountrys
                var countries = await _userService.GetPriviledgeCountrys();
                if (countries.Count == 1)
                {
                    userActiveCountry = countries[0]; 
                }
                else
                {
                    //If UserActive Country is already set in the UserEntity, use that value
                    string currentUserId = await _userService.GetCurrentUserId();
                    var currentUser = await _userService.GetUserById(currentUserId);

                    if (currentUser.UserActiveCountryId > 0)
                    {
                        var userActiveCountryFromEntity = await _countryService.GetCountryById(currentUser.UserActiveCountryId);
                        if (userActiveCountryFromEntity.CurrencySymbol != null)
                        {
                            userActiveCountry = userActiveCountryFromEntity;
                        }
                    }
                }

                //filter based on UserActiveCountry
                if(dashboardFilterCriteria.ActiveCountryId == null && userActiveCountry != null)
                {
                    dashboardFilterCriteria.ActiveCountryId = userActiveCountry.CountryId;
                }

                //Actual call
                var dashboard = await _dashboardService.GetDashboard(dashboardFilterCriteria);

                //set ActiveCountries
                var allCountries = await _countryService.GetCountries();
                var activeCountries = allCountries.Where(s => s.IsActive == true).ToList();
                dashboard.ActiveCountries = activeCountries;
                dashboard.UserActiveCountry = userActiveCountry;

                //set the filtered country
                if (dashboardFilterCriteria.ActiveCountryId != null && dashboardFilterCriteria.ActiveCountryId > 0)
                {
                    var UserActiveCountryForFilter = await _countryService.GetCountryById((int)dashboardFilterCriteria.ActiveCountryId);
                    if (UserActiveCountryForFilter != null)
                    {
                        dashboard.UserActiveCountryForFilter = UserActiveCountryForFilter;
                    }
                }

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }
    }
}
