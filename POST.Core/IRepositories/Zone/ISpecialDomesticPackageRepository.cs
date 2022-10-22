using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Zone
{
    public interface ISpecialDomesticPackageRepository : IRepository<SpecialDomesticPackage>
    {
        Task<List<SpecialDomesticPackage>> GetSpecialDomesticZones();
        

    }
}
