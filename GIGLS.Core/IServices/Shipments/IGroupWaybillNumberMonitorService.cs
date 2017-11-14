using GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IGroupWaybillNumberMonitorService : IServiceDependencyMarker
    {
        Task<GroupWaybillNumberMonitor> GetLastGroupWaybillNumberMonitor(string serviceCode);
        Task AddGroupWaybillNumberMonitor(string centre, string code);
        Task UpdateGroupWaybillNumberMonitor(string centre, string code);
    }
}
