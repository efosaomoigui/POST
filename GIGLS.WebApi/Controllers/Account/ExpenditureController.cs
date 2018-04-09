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
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/expenditure")]
    public class ExpenditureController : BaseWebApiController
    {
        private readonly IExpenditureService _expenditureService;
        public ExpenditureController(IExpenditureService expenditureService) : base(nameof(ExpenditureController))
        {
            _expenditureService = expenditureService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GeneralLedgerDTO>>> GetAllExpenditure()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var expenditure = await _expenditureService.GetExpenditures();

                return new ServiceResponse<IEnumerable<GeneralLedgerDTO>>
                {
                    Object = expenditure
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddExpenditure(GeneralLedgerDTO generalledgerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var generalledger = await _expenditureService.AddExpenditure(generalledgerDto);

                return new ServiceResponse<object>
                {
                    Object = generalledger
                };
            });
        }
    }
}
