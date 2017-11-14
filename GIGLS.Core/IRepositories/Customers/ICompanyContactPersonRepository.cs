using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Customers
{
    public interface ICompanyContactPersonRepository : IRepository<CompanyContactPerson>
    {
        Task<List<CompanyContactPersonDTO>> GetCompanyContactPerson();
    }
}
