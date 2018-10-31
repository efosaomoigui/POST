using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Shipments
{
    public interface IOverdueShipmentService : IServiceDependencyMarker
    {
        Task<IEnumerable<OverdueShipmentDTO>> GetOverdueShipments();
        Task<OverdueShipmentDTO> GetOverdueShipmentById(string waybill);
        Task AddOverdueShipment(OverdueShipmentDTO overdueShipment);
        Task UpdateOverdueShipment(OverdueShipmentDTO overdueShipment);
        Task RemoveOverdueShipment(string waybill);    }
}
