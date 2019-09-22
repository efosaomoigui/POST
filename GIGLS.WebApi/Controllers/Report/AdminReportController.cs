using GIGLS.Core.DTO.Admin;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Report;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Report
{
    //[Authorize(Roles = "Report")]
    [RoutePrefix("api/specialreport")]
    public class AdminReportController : BaseWebApiController
    {
        private readonly IAdminReportService _report;

        public AdminReportController(IAdminReportService report) : base(nameof(ReportsController))
        {
            _report = report;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("report")]
        public async Task<IServiceResponse<AdminReportDTO>> GetAdminReport()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _report.GetAdminReport();

                return new ServiceResponse<AdminReportDTO>
                {
                    Object = result
                };
            });
        }
    }
}