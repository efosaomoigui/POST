using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface ISpecialDomesticPackageService : IServiceDependencyMarker
    {
        Task<List<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages();
        Task<SpecialDomesticPackageDTO> GetSpecialDomesticPackageById(int SpecialDomesticId);
        Task<object> AddSpecialDomesticPackage(SpecialDomesticPackageDTO SpecialDomestic);
        Task UpdateSpecialDomesticPackage(int SpecialDomesticZoneId, SpecialDomesticPackageDTO SpecialDomestic);
        Task UpdateSpecialDomesticPackage(int SpecialDomesticZoneId, bool status);
        Task DeleteSpecialDomesticPackage(int SpecialDomesticZoneId);
    }
}
