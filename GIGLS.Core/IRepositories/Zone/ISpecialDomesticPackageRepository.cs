using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface ISpecialDomesticPackageRepository : IRepository<SpecialDomesticPackage>
    {
        Task<List<SpecialDomesticPackage>> GetSpecialDomesticZones();
        

    }
}
