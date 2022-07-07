using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Utility
{
    public interface IAutoManifestAndGroupingService : IServiceDependencyMarker
    {
        Task MappingWaybillNumberToGroup(string waybill);
        Task MappingWaybillNumberToGroupForBulk(string waybill);
        Task MapMoveManifest(string movemanifestNo);
    }
}
