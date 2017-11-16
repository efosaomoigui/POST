using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IInsuranceService : IServiceDependencyMarker
    {
        Task<IEnumerable<InsuranceDTO>> GetInsurances();
        Task<InsuranceDTO> GetInsuranceById(int insuranceId);
        Task<object> AddInsurance(InsuranceDTO insurance);
        Task UpdateInsurance(int insuranceId, InsuranceDTO insurance);
        Task RemoveInsurance(int insuranceId);
    }
}
