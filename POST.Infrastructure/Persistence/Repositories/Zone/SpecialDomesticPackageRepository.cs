using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.Domain;
using POST.Core.DTO.Zone;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using POST.Core.DTO;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class SpecialDomesticPackageRepository : Repository<SpecialDomesticPackage, GIGLSContext>, ISpecialDomesticPackageRepository
    {
        public SpecialDomesticPackageRepository(GIGLSContext context) : base(context)
        {
        }


        
        public Task<List<SpecialDomesticPackage>> GetSpecialDomesticZones()
        {
            try
            {
                var zones = Context.SpecialDomesticPackage;
                //var zoneDto = from s in zones
                //              select new SpecialDomesticPackageDTO
                //              {
                //                  Name = s.Name,
                //                  SpecialDomesticPackageId = s.SpecialDomesticPackageId,
                //                  Status = s.Status
                //              };
                return Task.FromResult(zones.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
