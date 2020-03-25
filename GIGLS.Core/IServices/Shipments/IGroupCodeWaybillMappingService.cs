using GIGLS.Core.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IGroupCodeWaybillMappingService : IServiceDependencyMarker
    {
        Task<GroupCodeWaybillMappingDTO> GetWaybillNumbersInGroup(string groupCodeNumber);
    }
}
