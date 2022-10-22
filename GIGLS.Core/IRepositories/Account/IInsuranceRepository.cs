using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IInsuranceRepository : IRepository<Insurance>
    {
        Task<List<InsuranceDTO>> GetInsurancesAsync();
        Task<InsuranceDTO> GetInsuranceById(int insuranceId);
        Task<InsuranceDTO> GetInsuranceByCountry(int countryId);
        Task<decimal> GetInsuranceValueByCountry(int countryId);
    }
}