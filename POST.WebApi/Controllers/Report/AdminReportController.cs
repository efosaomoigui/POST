using POST.Core.DTO.Admin;
using POST.Core.DTO.Report;
using POST.Core.IServices;
using POST.Core.IServices.Report;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Report
{
    [Authorize(Roles = "Report")]
    [RoutePrefix("api/specialreport")]
    public class AdminReportController : BaseWebApiController
    {
        private readonly IAdminReportService _report;

        public AdminReportController(IAdminReportService report) : base(nameof(ReportsController))
        {
            _report = report;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("report")]
        public async Task<IServiceResponse<AdminReportDTO>> GetAdminReport(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _report.GetAdminReport(filterCriteria);

                return new ServiceResponse<AdminReportDTO>
                {
                    Object = result
                };
            });
        }
    }
}