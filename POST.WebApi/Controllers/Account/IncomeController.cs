using POST.Core.DTO.Account;
using POST.Core.IServices;
using POST.Core.IServices.Account;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Account
{
    [Authorize(Roles = "Account, ViewAdmin")]
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
