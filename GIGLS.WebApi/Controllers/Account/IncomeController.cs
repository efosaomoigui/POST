using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/income")]
    public class IncomeController : BaseWebApiController
    {
        private readonly IIncomeService _incomeService;
        public IncomeController(IIncomeService incomeService) : base(nameof(IncomeController))
        {
            _incomeService = incomeService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GeneralLedgerDTO>>> GetAllIncome()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var income = await _incomeService.GetAllIncome();

                return new ServiceResponse<IEnumerable<GeneralLedgerDTO>>
                {
                    Object = income
                };
            });
        }

    }
}
