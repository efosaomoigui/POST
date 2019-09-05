using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Account
{
    public interface IInsuranceRepository : IRepository<Insurance>
    {
        Task<List<InsuranceDTO>> GetInsurancesAsync();
        Task<InsuranceDTO> GetInsuranceById(int insuranceId);
        Task<InsuranceDTO> GetInsuranceByCountry(int countryId);
    }
}