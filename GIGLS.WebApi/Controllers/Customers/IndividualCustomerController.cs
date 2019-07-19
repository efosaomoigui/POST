using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using System.Linq;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/IndividualCustomer")]
    public class IndividualCustomerController : BaseWebApiController
    {
        private readonly IIndividualCustomerService _service;
        private ICountryService _countryService;

        public IndividualCustomerController(IIndividualCustomerService service, ICountryService countryService) : base(nameof(IndividualCustomerController))
        {
            _service = service;
            _countryService = countryService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<IndividualCustomerDTO>>> GetCustomers()
        {
            return await HandleApiOperationAsync(async () =>
           {
               var customers = await _service.GetIndividualCustomers();

               //get all countries and set the country name
               var allCountries = await _countryService.GetCountries();

               foreach (var customer in customers)
               {
                   var country = allCountries.FirstOrDefault(s => s.CountryId == customer.UserActiveCountryId);
                   customer.UserActiveCountryName = country?.CountryName;
               }

               return new ServiceResponse<List<IndividualCustomerDTO>>
               {
                   Object = customers
               };
           });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddIndividualCustomer(IndividualCustomerDTO customer)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var individual = await _service.AddCustomer(customer);
                return new ServiceResponse<object>
                {
                    Object = individual
                };
            });
           
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{customerId:int}")]
        public async Task<IServiceResponse<IndividualCustomerDTO>> GetCustomer(int customerId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var customer = await _service.GetCustomerById(customerId);

               return new ServiceResponse<IndividualCustomerDTO>
               {
                   Object = customer
               };

           });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{customerId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCustomer(int customerId)
        {
           return await HandleApiOperationAsync(async () =>
           {
               await _service.DeleteCustomer(customerId);

               return new ServiceResponse<bool>
               {
                   Object = true
               };

           });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{customerId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCustomer(int customerId, IndividualCustomerDTO customerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateCustomer(customerId, customerDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }
    }
}
