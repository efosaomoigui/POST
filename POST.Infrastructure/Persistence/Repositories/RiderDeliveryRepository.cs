using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.DTO.User;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class RiderDeliveryRepository : Repository<RiderDelivery, GIGLSContext>, IRiderDeliveryRepository
    {
        private GIGLSContext _context;

        public RiderDeliveryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<RiderDeliveryDTO>> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var riderDelivery = _context.RiderDelivery.Where(s => s.DriverId == riderId && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            List<RiderDeliveryDTO> riderDeliveryDTO = (from s in riderDelivery
                                                       select new RiderDeliveryDTO
                                                       {
                                                           RiderDeliveryId = s.RiderDeliveryId,
                                                           Address = s.Address,
                                                           CostOfDelivery = s.CostOfDelivery,
                                                           DeliveryDate = s.DeliveryDate,
                                                           DriverId = s.DriverId,
                                                           Waybill = s.Waybill,
                                                           Area = s.Area,
                                                           UserDetail = _context.Users.Where(x => x.Id == s.DriverId)
                                                           .Select(p => new UserDTO
                                                           {
                                                               FirstName = p.FirstName,
                                                               LastName = p.LastName
                                                           }).FirstOrDefault()
                                                       }).ToList();

            var resultDto = riderDeliveryDTO.OrderByDescending(x => x.DeliveryDate).ToList();
            return await Task.FromResult(resultDto);
        }
    }
}
