using GIGLS.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IHaulageService : IServiceDependencyMarker
    {
        Task<IEnumerable<HaulageDTO>> GetHaulages();
        Task<HaulageDTO> GetHaulageById(int haulageId);
        Task<object> AddHaulage(HaulageDTO haulage);
        Task UpdateHaulage(int haulageId, HaulageDTO haulageDto);
        Task RemoveHaulage(int haulageId);
    }
}
