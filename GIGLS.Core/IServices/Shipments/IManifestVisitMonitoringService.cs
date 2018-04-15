using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestVisitMonitoringService : IServiceDependencyMarker
    {
        Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitorings();
        Task<ManifestVisitMonitoringDTO> GetManifestVisitMonitoringById(int manifestVisitMonitoringId);
        Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitoringByWaybill(string waybill);
        Task<object> AddManifestVisitMonitoring(ManifestVisitMonitoringDTO manifestVisitMonitoringDto);
        Task UpdateManifestVisitMonitoring(int manifestVisitMonitoringId, ManifestVisitMonitoringDTO manifest);
        Task DeleteManifestVisitMonitoring(int manifestVisitMonitoringId);
    }
}
