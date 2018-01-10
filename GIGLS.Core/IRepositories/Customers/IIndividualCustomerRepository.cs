using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Customers
{
    public interface IIndividualCustomerRepository : IRepository<IndividualCustomer>
    {
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers();
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData);
    }
}
