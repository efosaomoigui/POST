using POST.Core.IServices;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Shipments
{
    public interface IOverdueShipmentService : IServiceDependencyMarker
    {
        Task<IEnumerable<OverdueShipmentDTO>> GetOverdueShipments();
        Task<OverdueShipmentDTO> GetOverdueShipmentById(string waybill);
        Task AddOverdueShipment(OverdueShipmentDTO overdueShipment);
        Task UpdateOverdueShipment(OverdueShipmentDTO overdueShipment);
        Task RemoveOverdueShipment(string waybill);    }
}
