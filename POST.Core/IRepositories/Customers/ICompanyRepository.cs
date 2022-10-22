using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Dashboard;
using POST.Core.DTO.Report;
using POST.Core.Enums;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Customers
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<List<CompanyDTO>> GetCompanies();
        Task<List<CompanyDTO>> GetCompanies(CompanyType companyType, CustomerSearchOption searchOption);
        Task<CompanyDTO> GetCompanyById(int companyId);
        Task<CompanyDTO> GetCompanyByCode(string customerCode);
        Task<CompanyDTO> GetCompanyByIdWithCountry(int companyId);
        Task<EcommerceWalletDTO> GetWalletDetailsForCompany(int companyId);
        Task<List<CompanyDTO>> GetCompanies(BaseFilterCriteria filterCriteria);
        Task<List<CompanyDTO>> GetCompanyByEmail(string email);
        Task<List<EcommerceAgreementDTO>> GetPendingEcommerceRequest(BaseFilterCriteria filterCriteria);
        Task<EcommerceAgreementDTO> GetPendingEcommerceRequestById(int companyId);
        Task<List<CompanyDTO>> GetCompaniesByCodes(List<string> codes);
        Task<List<CompanyDTO>> GetCompanies(Rank rank, ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<CompanyDTO>> GetCompanyByEmail(string email, Rank? rank);
        Task<CustomerBreakdownDTO> GetNoOfBasicAndClassCustomers(DashboardFilterCriteria dashboardFilterCriteria);
        Task<decimal> GetBasicOrClassCustomersIncome(string procedureName, DashboardFilterCriteria dashboardFilterCriteria);
        Task<int> GetClassSubscriptions(DashboardFilterCriteria dashboardFilterCriteria);
        Task<List<CompanyDTO>> GetAssignedCustomers(BaseFilterCriteria filterCriteria);
        Task<List<CompanyDTO>> GetAssignedCustomersByCustomerRep(BaseFilterCriteria filterCriteria);
        Task<List<CompanyDTO>> GetCompaniesByEmailOrCode(string searchParams);
        Task<CompanyDTO> GetCompanyDetailsByEmail(string email);
    }
}