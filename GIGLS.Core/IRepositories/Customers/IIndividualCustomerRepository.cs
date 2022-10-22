using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Customers
{
    public interface IIndividualCustomerRepository : IRepository<IndividualCustomer>
    {
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers();
        Task<List<IndividualCustomerDTO>> GetIndividualCustomers(string searchData);
        Task<IndividualCustomerDTO> GetIndividualCustomerByIdWithCountry(int customerId);
        Task<int> GetCountOfIndividualCustomers(DashboardFilterCriteria dashboardFilterCriteria);
    }
}
