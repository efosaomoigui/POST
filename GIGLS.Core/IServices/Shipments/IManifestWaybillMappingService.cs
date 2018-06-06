using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestWaybillMappingService : IServiceDependencyMarker
    {
        Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings();
        Task MappingManifestToWaybills(string manifest, List<string> Waybills);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifest(string manifestcode);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch();
        Task<List<ManifestWaybillMappingDTO>> GetManifestForWaybill(string waybill);
        Task<ManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill);
        Task RemoveWaybillFromManifest(string manifest, string waybill);
        Task ReturnWaybillsInManifest(string manifest, List<string> Waybills);
        Task<List<ShipmentDTO>> GetUnMappedWaybillsForDeliveryManifestByServiceCentre();
        Task SignOffDeliveryManifest(string manifest);
    }
}
