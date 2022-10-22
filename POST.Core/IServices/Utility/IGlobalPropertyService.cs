using POST.Core.DTO.Utility;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Utility
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
        Task<decimal> GetDropOffDiscountInGlobalProperty(int countryId);
        Task<string> GenerateDeliveryCode();
        Task<decimal> GetGoFasterPercentageInGlobalProperty(int countryId);
    }
}
