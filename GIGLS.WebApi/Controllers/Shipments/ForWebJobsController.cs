using POST.Core.IServices;
using POST.Core.IServices.Shipments;
using POST.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using POST.CORE.IServices.Report;

namespace POST.WebApi.Controllers.Shipments
{
    [RoutePrefix("api/webjobs")]
    public class ForWebJobsController : BaseWebApiController
    {
        private readonly IShipmentService _service;
        private readonly IShipmentReportService _reportService;

        public ForWebJobsController(IShipmentService service, IShipmentReportService reportService) : base(nameof(ShipmentController))
        {
            _service = service;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("runreprintexpirycounter")] 
        public async Task<IServiceResponse<bool>> RunReprintExpiryCounter() 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentboolean = await _service.RePrintCountUpdater();
                return new ServiceResponse<bool>
                {
                    Object = shipmentboolean
                };
            });
        }

    }
}
