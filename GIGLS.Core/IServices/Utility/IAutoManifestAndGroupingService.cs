using POST.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace POST.Core.IServices.Utility
{
    public interface IAutoManifestAndGroupingService : IServiceDependencyMarker
    {
        Task MappingWaybillNumberToGroup(string waybill);
        Task MappingWaybillNumberToGroupForBulk(string waybill);
        Task MapMoveManifest(string movemanifestNo);
    }
}
