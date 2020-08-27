using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Stores
{
    public class ShipmentPackagingTransactionsRepository : Repository<ShipmentPackagingTransactions, GIGLSContext>, IShipmentPackagingTransactionsRepository
    {
        private GIGLSContext _context;

        public ShipmentPackagingTransactionsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BaseFilterCriteria filterCriteria, int[] serviceCenterIds)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                var packages = _context.ShipmentPackagingTransactions.Where(c => serviceCenterIds.Contains(c.ServiceCenterId) && c.DateCreated >= startDate && c.DateCreated < endDate);

                var packageDto = from p in packages
                                 join d in Context.ServiceCentre on p.ServiceCenterId equals d.ServiceCentreId
                                 join t in Context.ShipmentPackagePrice on p.ShipmentPackageId equals t.ShipmentPackagePriceId
                                 select new ShipmentPackagingTransactionsDTO
                                 {
                                     Waybill = p.Waybill,
                                     Quantity = p.Quantity,
                                     PackageTransactionType = p.PackageTransactionType,
                                     ServiceCenterName = d.Name,
                                     ShipmentPackageName = t.Description,
                                     User = p.User.FirstName + " " + p.User.LastName,
                                     ReceiverServiceCenterName = _context.ServiceCentre.Where(x => x.ServiceCentreId == p.ReceiverServiceCenterId)
                                     .Select(c => c.Name).FirstOrDefault(),
                                     DateCreated = p.DateCreated,
                                     DateModified = p.DateModified
                                 };

                return Task.FromResult(packageDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
