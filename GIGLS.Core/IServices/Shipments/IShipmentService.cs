using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentService : IServiceDependencyMarker
    {
        Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto);
        Task<ShipmentDTO> GetShipment(int shipmentId);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<ShipmentDTO> AddShipment(ShipmentDTO shipment);
        Task UpdateShipment(int shipmentId, ShipmentDTO shipment);
        Task UpdateShipment(string waybill, ShipmentDTO shipment);
        Task DeleteShipment(int shipmentId);
        Task DeleteShipment(string waybill);
        Task<Shipment> GetShipmentForScan(string waybill);
    }
}
