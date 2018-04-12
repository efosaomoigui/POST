using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface ITransitWaybillNumberService : IServiceDependencyMarker
    {
        Task<IEnumerable<TransitWaybillNumberDTO>> GetTransitWaybillNumbers();
        Task<TransitWaybillNumberDTO> GetTransitWaybillNumberById(int transitWaybillNumberId);
        Task<TransitWaybillNumberDTO> GetTransitWaybillNumberByCode(string waybillNumber);
        Task<object> AddTransitWaybillNumber(TransitWaybillNumberDTO transitWaybillNumberDto);
        Task UpdateTransitWaybillNumber(int transitWaybillNumberId, TransitWaybillNumberDTO transitWaybillNumberDto);
        Task DeleteTransitWaybillNumber(int transitWaybillNumberId);
    }
}
