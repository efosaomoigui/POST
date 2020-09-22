using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Stores
{
    public class ServiceCenterPackageRepository : Repository<ServiceCenterPackage, GIGLSContext>, IServiceCenterPackageRepository
    {
        private GIGLSContext _context;

        public ServiceCenterPackageRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ServiceCenterPackageDTO>> GetShipmentPackageForServiceCenter(int[] serviceCenterIds)
        {
            try
            {
                var packages = _context.ServiceCenterPackage.Where(c => serviceCenterIds.Contains(c.ServiceCenterId));

                var packageDto = from p in packages
                                 join d in Context.ServiceCentre on p.ServiceCenterId equals d.ServiceCentreId
                                 join t in Context.ShipmentPackagePrice on p.ShipmentPackageId equals t.ShipmentPackagePriceId
                                 select new ServiceCenterPackageDTO
                                 {
                                     InventoryOnHand = p.InventoryOnHand,
                                     MinimunRequired = p.MinimunRequired,
                                     ShipmentPackageName = t.Description,
                                     DateCreated = p.DateCreated,
                                     DateModified = p.DateModified
                                 };

                return Task.FromResult(packageDto.OrderByDescending(x => x.DateCreated).ToList());
                //return Task.FromResult(packageDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
