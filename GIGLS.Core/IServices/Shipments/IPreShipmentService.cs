using GIGLS.Core.DTO.ServiceCentres;
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

        //Management API
        Task<List<PreShipmentDTO>> GetNewPreShipments(FilterOptionsDto filterOptionsDto);
        Task<List<PreShipmentDTO>> GetValidPreShipments(FilterOptionsDto filterOptionsDto);
        Task<bool> ValidatePreShipment(string waybill, bool valid);
        Task<bool> CreateShipmentFromPreShipment(string waybill);
        Task<List<StationDTO>> GetUnmappedManifestStations();
    }
}
