using GIGLS.Core.Domain;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IWaybillNumberMonitorService : IServiceDependencyMarker
    {
        Task<WaybillNumberMonitor> GetLastWaybillNumberMonitor(string serviceCode);
        Task AddWaybillNumberMonitor(string centre, string code);
        Task UpdateWaybillNumberMonitor(string centre, string code);
    }
}
