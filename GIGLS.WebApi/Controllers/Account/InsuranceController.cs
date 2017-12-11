using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using System.Linq;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/insurance")]
    public class InsuranceController : BaseWebApiController
    {
        private readonly IInsuranceService _insuranceService;
        public InsuranceController(IInsuranceService insuranceService) : base(nameof(InsuranceController))
        {
            _insuranceService = insuranceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<InsuranceDTO>> GetInsurances()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var insurance = await _insuranceService.GetInsurances();

                return new ServiceResponse<InsuranceDTO>
                {
                    Object = insurance.FirstOrDefault()
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddInsurance(InsuranceDTO insuranceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var insurance = await _insuranceService.AddInsurance(insuranceDto);

                return new ServiceResponse<object>
                {
                    Object = insurance
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{insuranceId:int}")]
        public async Task<IServiceResponse<InsuranceDTO>> GetInsurance(int insuranceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var insurance = await _insuranceService.GetInsuranceById(insuranceId);

                return new ServiceResponse<InsuranceDTO>
                {
                    Object = insurance
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{insuranceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteInsurance(int insuranceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _insuranceService.RemoveInsurance(insuranceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{insuranceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateInsurance(int insuranceId, InsuranceDTO insuranceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _insuranceService.UpdateInsurance(insuranceId, insuranceDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
