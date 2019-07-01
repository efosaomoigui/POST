using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
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
