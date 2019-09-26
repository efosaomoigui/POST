using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<RiderDeliveryDTO>> GetRiderDelivery (string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var riderDelivery = Context.RiderDelivery.Where(s => s.DriverId == riderId && s.DateCreated >= startDate && s.DateCreated < endDate ).AsQueryable();

            var riderDeliveryDTO = from s in riderDelivery
                                   select new RiderDeliveryDTO
                                   {
                                       RiderDeliveryId = s.RiderDeliveryId,
                                       Address = s.Address,
                                       CostOfDelivery = s.CostOfDelivery,
                                       DeliveryDate = s.DeliveryDate,
                                       DriverId = s.DriverId,
                                       Waybill = s.Waybill,
                                      
                                       //UserDetail = Context.Users.Where(x => x.Id == s.DriverId)
                                       //.Select(p => new UserDTO
                                       //{
                                       //    FirstName = p.FirstName,
                                       //    LastName = p.LastName
                                       //}).FirstOrDefault()
                                   };

            return await Task.FromResult(riderDeliveryDTO.ToList());


        }
    }
}
