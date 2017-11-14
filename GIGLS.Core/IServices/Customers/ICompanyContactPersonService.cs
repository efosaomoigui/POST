using GIGLS.Core.DTO.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Customers
{
    public interface ICompanyContactPersonService : IServiceDependencyMarker
    {
        Task<IEnumerable<CompanyContactPersonDTO>> GetContactPersons();
        Task<CompanyContactPersonDTO> GetContactPersonById(int id);
        Task<object> AddContactPerson(CompanyContactPersonDTO contactPerson);
        Task UpdateContactPerson(int contactPersonId, CompanyContactPersonDTO contactPerson);
        Task DeleteContactPerson(int id);
    }
}
