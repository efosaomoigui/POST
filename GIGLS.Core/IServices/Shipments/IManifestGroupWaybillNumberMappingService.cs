using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestGroupWaybillNumberMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings();
        Task MappingManifestToGroupWaybillNumber(int manifestId, int groupWaybillNumberId);
        Task MappingManifestToGroupWaybillNumber(string manifest, string groupWaybillNumber);
        Task<ManifestDTO> GetManifestForGroupWaybillNumber(int groupWaybillNumberId);
        Task<ManifestDTO> GetManifestForGroupWaybillNumber(string groupWaybillNumber);
        Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(int manifestId);
        Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(string manifest);
        Task RemoveGroupWaybillNumberFromManifest(string manifest, string groupWaybillNumber);
    }
}
