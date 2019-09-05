using GIGLS.Core.DTO.Utility;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Utility
{
    public interface IGlobalPropertyService : IServiceDependencyMarker
    {

        Task<IEnumerable<GlobalPropertyDTO>> GetGlobalProperties();
        Task<GlobalPropertyDTO> GetGlobalPropertyById(int globalPropertyId);
        Task<object> AddGlobalProperty(GlobalPropertyDTO globalProperty);
        Task UpdateGlobalProperty(int globalPropertyId, GlobalPropertyDTO globalProperty);
        Task UpdateGlobalProperty(int globalPropertyId, bool status);
        Task RemoveGlobalProperty(int globalPropertyId);
        Task<GlobalPropertyDTO> GetGlobalProperty(GlobalPropertyType globalPropertyType, int countryId);
    }
}
