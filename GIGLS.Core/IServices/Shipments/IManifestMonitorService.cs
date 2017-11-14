using GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestMonitorService : IServiceDependencyMarker
    {
        Task<ManifestMonitor> GetLastManifestMonitor(string serviceCode);
        Task AddManifestMonitor(string centre, string code);
        Task UpdateManifestMonitor(string centre, string code);
    }
}
