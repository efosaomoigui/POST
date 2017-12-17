using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.Enums;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseWebApiController
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service) : base(nameof(CustomerController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<CustomerDTO>> CreateCustomer(CustomerDTO customer)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var customerObj = await _service.CreateCustomer(customer);
                return new ServiceResponse<CustomerDTO>
                {
                    Object = customerObj
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<CustomerDTO>> GetCustomer(int customerId, CustomerType customerType)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var customerObj = await _service.GetCustomer(customerId, customerType);

                return new ServiceResponse<CustomerDTO>
                {
                    Object = customerObj
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{phonenumber}/phonenumber")]
        public async Task<IServiceResponse<IndividualCustomerDTO>> GetCustomerByPhoneNumber(string phonenumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var customerObj = await _service.GetCustomerByPhoneNumber(phonenumber);

                return new ServiceResponse<IndividualCustomerDTO>
                {
                    Object = customerObj
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{customerType}")]
        public async Task<IServiceResponse<List<CustomerDTO>>> GetCustomers(CustomerType customerType)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var customerObj = await _service.GetCustomers(customerType);

                return new ServiceResponse<List<CustomerDTO>>
                {
                    Object = customerObj
                };
            });
        }


    }
}
