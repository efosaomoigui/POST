using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestWaybillMappingService : IServiceDependencyMarker
    {
        Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings();
        Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings(DateFilterCriteria dateFilterCriteria);
        Task MappingManifestToWaybills(string manifest, List<string> Waybills);
        Task MappingManifestToWaybillsMobile(string manifest, List<string> waybills);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifest(string manifestcode);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch();
        Task<List<ManifestWaybillMappingDTO>> GetManifestForWaybill(string waybill);
        Task<ManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill);
        Task RemoveWaybillFromManifest(string manifest, string waybill);
        Task ReturnWaybillsInManifest(string manifest, List<string> Waybills);
        Task<List<ShipmentDTO>> GetUnMappedWaybillsForDeliveryManifestByServiceCentre();
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaitingForSignOff();
        Task<List<ManifestWaybillMappingDTO>> GetManifestHistoryForWaybill(string waybill);

        Task MappingHUBManifestToWaybills(string manifest, List<string> Waybills, int DepartureServiceCentreId, int DestinationServiceCentreId);
    }
}
