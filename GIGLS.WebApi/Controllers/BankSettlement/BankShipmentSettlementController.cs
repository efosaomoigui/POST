using POST.Core.DTO.Account;
using POST.Core.IServices;
using POST.Core.IServices.BankSettlement;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.BankSettlement
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/bankshipmentsettlement")]
    public class BankShipmentSettlementController : BaseWebApiController
    {
        private readonly IBankShipmentSettlementService _bankShipmentSettlementService;

        public BankShipmentSettlementController(IBankShipmentSettlementService bankShipmentSettlementService) :base(nameof(BankShipmentSettlementController))
        {
            _bankShipmentSettlementService = bankShipmentSettlementService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("cashshipmentsettlement")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetCashShipmentSettlement()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var cashShipmentSettlements = await _bankShipmentSettlementService.GetCashShipmentSettlement();
                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = cashShipmentSettlements
                };
            });
        }

    }
}
