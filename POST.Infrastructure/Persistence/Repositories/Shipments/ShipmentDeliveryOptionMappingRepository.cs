using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.Zone;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentDeliveryOptionMappingRepository : Repository<ShipmentDeliveryOptionMapping, GIGLSContext>, IShipmentDeliveryOptionMappingRepository
    {
        private GIGLSContext _context;
        public ShipmentDeliveryOptionMappingRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentDeliveryOptionMappingDTO>> GetAllShipmentDeliveryOptionMappings()
        {
            var mappings = _context.ShipmentDeliveryOptionMapping.AsQueryable();

            List<ShipmentDeliveryOptionMappingDTO> result = (from m in mappings
                                                              select new ShipmentDeliveryOptionMappingDTO
                                                              {
                                                                  ShipmentDeliveryOptionMappingId = m.ShipmentDeliveryOptionMappingId,
                                                                  Waybill = m.Waybill,
                                                                  DeliveryOptionId = m.DeliveryOptionId,
                                                                  DateCreated = m.DateCreated,
                                                                  DateModified = m.DateModified,
                                                                  DeliveryOption = _context.DeliveryOption.Where(x => x.DeliveryOptionId == m.DeliveryOptionId).
                                                                  Select(d => new DeliveryOptionDTO {
                                                                      DeliveryOptionId = d.DeliveryOptionId,
                                                                      Code = d.Code,
                                                                      Description = d.Description,
                                                                      IsActive = d.IsActive
                                                                  }).FirstOrDefault(),
                                                              }).ToList();
            return Task.FromResult(result);
        }

        public Task<List<ShipmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill)
        {
            var mappings = _context.ShipmentDeliveryOptionMapping.Where(x => x.Waybill == waybill).AsQueryable();

            List<ShipmentDeliveryOptionMappingDTO> result = (from m in mappings
                                                              select new ShipmentDeliveryOptionMappingDTO
                                                              {
                                                                  ShipmentDeliveryOptionMappingId = m.ShipmentDeliveryOptionMappingId,
                                                                  Waybill = m.Waybill,
                                                                  DeliveryOptionId = m.DeliveryOptionId,
                                                                  DateCreated = m.DateCreated,
                                                                  DateModified = m.DateModified,
                                                                  DeliveryOption = _context.DeliveryOption.Where(x => x.DeliveryOptionId == m.DeliveryOptionId).
                                                                  Select(d => new DeliveryOptionDTO
                                                                  {
                                                                      DeliveryOptionId = d.DeliveryOptionId,
                                                                      Code = d.Code,
                                                                      Description = d.Description,
                                                                      IsActive = d.IsActive
                                                                  }).FirstOrDefault(),
                                                              }).ToList();
            return Task.FromResult(result);
        }
    }
}
