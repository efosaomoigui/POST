using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IPreShipmentManifestMappingService : IServiceDependencyMarker
    {
        Task<List<PreShipmentManifestMappingDTO>> GetAllManifestWaybillMappings();
        Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifest(string manifestcode);
        Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifestForPickup();
        Task<PreShipmentManifestMappingDTO> GetManifestForWaybill(string waybill);
        Task<List<PreShipmentDTO>> GetUnMappedWaybillsForPickupManifest();

        Task MappingManifestToWaybills(PreShipmentManifestMappingDTO data);
        Task RemoveWaybillFromManifest(string manifest, string waybill);
    }
}
