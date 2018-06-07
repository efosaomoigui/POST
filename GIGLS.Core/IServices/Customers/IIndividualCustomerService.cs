using GIGLS.Core.DTO.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Customers
{
    public interface IIndividualCustomerService : IServiceDependencyMarker
    {
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers();
        Task<IndividualCustomerDTO> GetCustomerById(int customerId);
        Task<IndividualCustomerDTO> GetCustomerByCode(string customerCode);
        Task UpdateCustomer(int customerId, IndividualCustomerDTO customer);
        Task<IndividualCustomerDTO> AddCustomer(IndividualCustomerDTO customer);
        Task DeleteCustomer(int customerId);
        Task<IndividualCustomerDTO> GetCustomerByPhoneNumber(string phoneNumber);
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData);
    }
}
