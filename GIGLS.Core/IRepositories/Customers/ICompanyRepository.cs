using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Customers
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<List<CompanyDTO>> GetCompanies();
        Task<List<CompanyDTO>> GetCompanies(CompanyType companyType, CustomerSearchOption searchOption);
        Task<CompanyDTO> GetCompanyById(int companyId);
        Task<CompanyDTO> GetCompanyByCode(string customerCode);
        Task<CompanyDTO> GetCompanyByIdWithCountry(int companyId);
        Task<EcommerceWalletDTO> GetWalletDetailsForCompany(int companyId);
    }
}