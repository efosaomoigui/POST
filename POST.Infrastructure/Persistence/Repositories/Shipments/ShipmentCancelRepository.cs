using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.Report;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentCancelRepository : Repository<ShipmentCancel, GIGLSContext>, IShipmentCancelRepository
    {
        private GIGLSContext _context;
        public ShipmentCancelRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentCancelDTO>> GetShipmentCancels()
        {
            var shipments = _context.ShipmentCancel.AsQueryable();

            List<ShipmentCancelDTO> shipmentDto = (from r in shipments
                                                   select new ShipmentCancelDTO()
                                                   {
                                                       Waybill = r.Waybill,
                                                       CreatedBy = Context.Users.Where(x => x.Id == r.CreatedBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                       ShipmentCreatedDate = r.ShipmentCreatedDate,
                                                       CancelledBy = Context.Users.Where(x => x.Id == r.CancelledBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                       DateCreated = r.DateCreated,
                                                       CancelReason= r.CancelReason
                                                   }).OrderByDescending(x => x.DateCreated).ToList();

            return Task.FromResult(shipmentDto);
        }

        public Task<List<ShipmentCancelDTO>> GetShipmentCancels(ShipmentCollectionFilterCriteria collectionFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = collectionFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var shipments = _context.ShipmentCancel.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            List<ShipmentCancelDTO> shipmentDto = (from r in shipments
                                                   select new ShipmentCancelDTO()
                                                   {
                                                       Waybill = r.Waybill,
                                                       CreatedBy = Context.Users.Where(x => x.Id == r.CreatedBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                       ShipmentCreatedDate = r.ShipmentCreatedDate,
                                                       CancelledBy = Context.Users.Where(x => x.Id == r.CancelledBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                       DateCreated = r.DateCreated,
                                                       CancelReason = r.CancelReason
                                                   }).OrderByDescending(x => x.DateCreated).ToList();

            return Task.FromResult(shipmentDto);
        }

        public Task<ShipmentCancelDTO> GetShipmentCancels(string waybill)
        {
            var shipments = _context.ShipmentCancel.Where(x => x.Waybill == waybill).AsQueryable();

            ShipmentCancelDTO shipmentDto = (from r in shipments
                                             select new ShipmentCancelDTO()
                                             {
                                                 Waybill = r.Waybill,
                                                 CreatedBy = Context.Users.Where(x => x.Id == r.CreatedBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                 ShipmentCreatedDate = r.ShipmentCreatedDate,
                                                 CancelledBy = Context.Users.Where(x => x.Id == r.CancelledBy).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                 DateCreated = r.DateCreated,
                                                 CancelReason = r.CancelReason
                                             }).FirstOrDefault();

            return Task.FromResult(shipmentDto);
        }
    }
}