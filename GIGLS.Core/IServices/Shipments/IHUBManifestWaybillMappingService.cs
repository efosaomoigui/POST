using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IHUBManifestWaybillMappingService : IServiceDependencyMarker
    {
        Task<List<HUBManifestWaybillMappingDTO>> GetAllHUBManifestWaybillMappings();
        Task<List<HUBManifestWaybillMappingDTO>> GetAllHUBManifestWaybillMappings(DateFilterCriteria dateFilterCriteria);
        Task MappingManifestToWaybills(string manifest, List<string> Waybills);
        Task MappingManifestToWaybillsMobile(string manifest, List<string> waybills);
        Task<List<HUBManifestWaybillMappingDTO>> GetWaybillsInManifest(string manifestcode);
        Task<List<HUBManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch();
        Task<List<HUBManifestWaybillMappingDTO>> GetManifestForWaybill(string waybill);
        Task<HUBManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill);
        Task RemoveWaybillFromManifest(string manifest, string waybill);
        Task ReturnWaybillsInManifest(string manifest, List<string> Waybills);
        Task<List<ShipmentDTO>> GetUnMappedWaybillsForDeliveryManifestByServiceCentre();
        Task<List<HUBManifestWaybillMappingDTO>> GetManifestWaitingForSignOff();
        Task<List<HUBManifestWaybillMappingDTO>> GetManifestHistoryForWaybill(string waybill);

        Task MappingHUBManifestToWaybills(string manifest, List<string> Waybills, int DepartureServiceCentreId, int DestinationServiceCentreId);
        Task MappingHUBManifestToWaybillsForScanner(string manifest, List<string> waybills, int DepartureServiceCentreId, int DestinationServiceCentreId);
    }
}
