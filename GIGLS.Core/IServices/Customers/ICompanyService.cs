using GIGLS.Core.DTO.Customers;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Customers
{
    public interface ICompanyService : IServiceDependencyMarker
    {
        Task<List<CompanyDTO>> GetCompanies();
        Task<CompanyDTO> GetCompanyById(int companyId);
        Task UpdateCompany(int companyId, CompanyDTO company);
        Task<CompanyDTO> AddCompany(CompanyDTO company);
        Task DeleteCompany(int companyId);
        Task UpdateCompanyStatus(int companyId, CompanyStatus status);
    }
}