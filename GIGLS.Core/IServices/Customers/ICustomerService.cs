using GIGLS.Core.DTO.Customers;
using System.Threading.Tasks;
using GIGLS.Core.Enums;

namespace GIGLS.Core.IServices.Customers
{
    public interface ICustomerService : IServiceDependencyMarker
    {
        Task<CustomerDTO> CreateCustomer(CustomerDTO customer);
        Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType);
    }
}
