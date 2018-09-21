using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IPreShipmentService : IServiceDependencyMarker
    {
        Tuple<Task<List<PreShipmentDTO>>, int> GetPreShipments(FilterOptionsDto filterOptionsDto);
        Task<PreShipmentDTO> GetPreShipment(int preShipmentId);
        Task<PreShipmentDTO> GetPreShipment(string waybill);
        Task<PreShipmentDTO> AddPreShipment(PreShipmentDTO preShipment);
        Task UpdatePreShipment(int preShipmentId, PreShipmentDTO preShipment);
        Task UpdatePreShipment(string waybill, PreShipmentDTO preShipment);
        Task DeletePreShipment(int shipmentId);
        Task DeletePreShipment(string waybill);
        Task<bool> CancelPreShipment(string waybill);
    }
}
