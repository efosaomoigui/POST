using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.Enums;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Customers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/company")]
    public class CompanyController : BaseWebApiController
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service) : base(nameof(CompanyController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CompanyDTO>>> GetCompanies()
        {
            return await HandleApiOperationAsync(async () => {
                var companies = await _service.GetCompanies();
                return new ServiceResponse<IEnumerable<CompanyDTO>>
                {
                    Object = companies
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCompany(CompanyDTO companyDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var company = await _service.AddCompany(companyDto);
                return new ServiceResponse<object>
                {
                    Object = company
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{companyId:int}")]
        public async Task<IServiceResponse<CompanyDTO>> GetCompany(int companyId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var company = await _service.GetCompanyById(companyId);

                return new ServiceResponse<CompanyDTO>
                {
                    Object = company
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{companyId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCustomer(int companyId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteCompany(companyId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            }); 
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{companyId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCustomer(int companyId, CompanyDTO companyDto)
        {

            return await HandleApiOperationAsync( async () =>
            {
                await _service.UpdateCompany(companyId, companyDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });         
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{companyId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateCustomerStatus(int companyId, CompanyStatus status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateCompanyStatus(companyId, status);

                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });           
        }
    }
}
