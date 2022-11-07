using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Customers
{
    public interface ICompanyContactPersonRepository : IRepository<CompanyContactPerson>
    {
        Task<List<CompanyContactPersonDTO>> GetCompanyContactPerson();
    }
}
