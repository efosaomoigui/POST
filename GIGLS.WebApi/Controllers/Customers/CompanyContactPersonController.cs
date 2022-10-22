using POST.Core.IServices;
using POST.Core.DTO.Customers;
using POST.Core.IServices.Customers;
using POST.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using POST.WebApi.Filters;

namespace POST.WebApi.Controllers.Customers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/companycontactperson")]
    public class CompanyContactPersonController : BaseWebApiController 
    {
        private readonly ICompanyContactPersonService _service;

        public CompanyContactPersonController(ICompanyContactPersonService service):base (nameof(CompanyContactPersonController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CompanyContactPersonDTO>>> GetCompanyContactPersons()
        {

            return await HandleApiOperationAsync(async () =>
            {
                var persons = await _service.GetContactPersons();
                return new ServiceResponse<IEnumerable<CompanyContactPersonDTO>>
                {
                    Object = persons
                };

            });

           
        }
        
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCompanyContactPerson(CompanyContactPersonDTO personDto)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var person = await _service.AddContactPerson(personDto);

                return new ServiceResponse<object>
                {
                    Object = person
                };
            });
            
            
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{personId:int}")]
        public async Task<IServiceResponse<CompanyContactPersonDTO>> GetCompanyContactPerson(int personId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var person = await _service.GetContactPersonById(personId);

                return new ServiceResponse<CompanyContactPersonDTO>
                {
                    Object = person
                };

            });
           
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{personId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCompanyContactPerson(int personId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteContactPerson(personId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{personId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCompanyContactPerson(int personId, CompanyContactPersonDTO personDto)
        {
            return await HandleApiOperationAsync(async () =>
           {
               await _service.UpdateContactPerson(personId, personDto);
               return new ServiceResponse<bool>
               {
                   Object = true
               };

           });
        }
    }
}
