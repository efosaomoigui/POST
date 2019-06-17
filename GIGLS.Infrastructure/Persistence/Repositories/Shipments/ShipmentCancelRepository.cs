using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
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
    }
}
