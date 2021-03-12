using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Customers
{
    public interface IIndividualCustomerRepository : IRepository<IndividualCustomer>
    {
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers();
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData);
        Task<IndividualCustomerDTO> GetIndividualCustomerByIdWithCountry(int customerId);
        Task<int> GetCountOfIndividualCustomers(DashboardFilterCriteria dashboardFilterCriteria);
    }
}
