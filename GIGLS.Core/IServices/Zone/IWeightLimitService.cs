using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Zone
{
    public interface IWeightLimitService : IServiceDependencyMarker
    {
        Task<IEnumerable<WeightLimitDTO>> GetWeightLimits();
        Task<WeightLimitDTO> GetActiveWeightLimits();
        Task<WeightLimitDTO> GetWeightLimitById(int weightLimitId);
        Task<object> AddWeightLimit(WeightLimitDTO weightLimit);
        Task UpdateWeightLimit(int weightLimitId, WeightLimitDTO weightLimit);
        Task RemoveWeightLimit(int weightLimitId);
    }
}
