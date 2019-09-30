using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
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
