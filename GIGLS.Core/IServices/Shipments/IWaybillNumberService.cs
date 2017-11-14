using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IWaybillNumberService : IServiceDependencyMarker
    {
        Task<IEnumerable<WaybillNumberDTO>> GetAllWayBillNumbers();
        Task<List<WaybillNumberDTO>> GetActiveWayBillNumbers();
        Task<List<WaybillNumberDTO>> GetDeliverWayBillNumbers();
        Task<WaybillNumberDTO> GetWayBillNumberById(int waybillId);
        Task<WaybillNumberDTO> GetWayBillNumberById(string waybillNumber);
        Task<string> GenerateWaybillNumber(string code, string user, int servicecentre);
        Task UpdateWaybillNumber(int waybillId);
        Task UpdateWaybillNumber(string waybillNumber);
        Task RemoveWaybillNumber(int waybillId);
        Task RemoveWaybillNumber(string waybillId);
    }
}
