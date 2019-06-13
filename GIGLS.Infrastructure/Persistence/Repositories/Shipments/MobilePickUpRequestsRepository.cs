using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class MobilePickUpRequestsRepository : Repository<MobilePickUpRequests, GIGLSContext>, IMobilePickUpRequestsRepository
    {
        private GIGLSContext _context;
        public MobilePickUpRequestsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }


        public Task<List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsync(string userId)
        {
            try
            {

                var MobilePickUpRequests = _context.MobilePickUpRequests.Where(x => x.UserId == userId);
           
                var MobilePickUpRequestsDto = from mobilepickuprequest in MobilePickUpRequests
                                              select new MobilePickUpRequestsDTO
                                              {
                                                  DateCreated = mobilepickuprequest.DateCreated,
                                                  Waybill = mobilepickuprequest.Waybill,
                                                  Status = mobilepickuprequest.Status,
                                                  PreShipment = _context.PresShipmentMobile.Where(c => c.Waybill == mobilepickuprequest.Waybill).Select(x => new PreShipmentMobileDTO
                                                  {
                                                      GrandTotal = x.GrandTotal,
                                                      ReceiverAddress = x.ReceiverAddress,
                                                      SenderAddress = x.SenderAddress,
                                                      SenderName = x.SenderName,
                                                      DateCreated = x.DateCreated,
                                                      ReceiverName = x.ReceiverName,
                                                      ReceiverPhoneNumber = x.ReceiverPhoneNumber,
                                                      SenderPhoneNumber = x.SenderPhoneNumber,
                                                      ReceiverLocation = new LocationDTO
                                                       {
                                                           Longitude = x.ReceiverLocation.Longitude,
                                                           Latitude = x.ReceiverLocation.Latitude
                                                       },
                                                      SenderLocation = new LocationDTO
                                                      {
                                                          Longitude = x.SenderLocation.Longitude,
                                                          Latitude = x.SenderLocation.Latitude
                                                      }
                                                  }).FirstOrDefault()
                                              };

                return Task.FromResult(MobilePickUpRequestsDto.ToList().OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
