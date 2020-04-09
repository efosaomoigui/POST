using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
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
                                                  GroupCodeNumber = _context.MobileGroupCodeWaybillMapping.Where(x => x.WaybillNumber == mobilepickuprequest.Waybill)
                                                                    .Select(d => d.GroupCodeNumber).FirstOrDefault(),
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
                                                      ServiceCentreAddress = x.ServiceCentreAddress,
                                                      ReceiverLocation = new LocationDTO
                                                       {
                                                           Longitude = x.ReceiverLocation.Longitude,
                                                           Latitude = x.ReceiverLocation.Latitude
                                                       },
                                                      serviceCentreLocation = new LocationDTO
                                                      {
                                                          Longitude = x.serviceCentreLocation.Longitude,
                                                          Latitude = x.serviceCentreLocation.Latitude
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


        public Task<List<MobilePickUpRequestsDTO>> GetMobilePickUpRequestsAsyncMonthly(string userId)
        {
            try
            {
                var MobilePickUpRequests = _context.MobilePickUpRequests.AsQueryable().Where(x => x.UserId == userId).ToList();

                var MobilePickUpRequestsDto = (from mobilepickuprequest in MobilePickUpRequests
                                              where mobilepickuprequest.DateCreated.Month == DateTime.Now.Month && mobilepickuprequest.DateCreated.Year== DateTime.Now.Year
                                              select new MobilePickUpRequestsDTO
                                              {
                                                  DateCreated = mobilepickuprequest.DateCreated,
                                                  Waybill = mobilepickuprequest.Waybill,
                                                  GroupCodeNumber = _context.MobileGroupCodeWaybillMapping.Where(x => x.WaybillNumber == mobilepickuprequest.Waybill)
                                                                .Select(x => x.GroupCodeNumber).FirstOrDefault(),
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
                                                      ServiceCentreAddress = x.ServiceCentreAddress,
                                                      ReceiverLocation = new LocationDTO
                                                      {
                                                          Longitude = x.ReceiverLocation.Longitude,
                                                          Latitude = x.ReceiverLocation.Latitude
                                                      },
                                                      serviceCentreLocation = new LocationDTO
                                                      {
                                                          Longitude = x.serviceCentreLocation.Longitude,
                                                          Latitude = x.serviceCentreLocation.Latitude
                                                      },
                                                      SenderLocation = new LocationDTO
                                                      {
                                                          Longitude = x.SenderLocation.Longitude,
                                                          Latitude = x.SenderLocation.Latitude
                                                      }
                                                  }).FirstOrDefault()
                                              }).ToList();

                return Task.FromResult(MobilePickUpRequestsDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<List<FleetMobilePickUpRequestsDTO>> GetPartnerMobilePickUpRequestsForFleetPartner(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode)
        {
            try
            {
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var partners = _context.Partners.AsQueryable().Where(s => s.FleetPartnerCode == fleetPartnerCode);

                var MobilePickUpRequestsDto = (from partner in partners
                                              join mobilepickuprequest in _context.MobilePickUpRequests on partner.UserId equals mobilepickuprequest.UserId
                                              where mobilepickuprequest.DateCreated >= startDate && mobilepickuprequest.DateCreated < endDate
                                              select new FleetMobilePickUpRequestsDTO
                                              {
                                                  DateCreated = mobilepickuprequest.DateCreated,
                                                  Waybill = mobilepickuprequest.Waybill,
                                                  Status = mobilepickuprequest.Status,
                                                  PartnerName = partner.FirstName + " " + partner.LastName,
                                                  PhoneNumber = partner.PhoneNumber,
                                              }).ToList();

                return Task.FromResult(MobilePickUpRequestsDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PartnerDTO> GetPartnerDetailsForAWaybill(string waybill)
        {
            try
            {
                var mobileRequests = _context.MobilePickUpRequests.AsQueryable().Where(x => x.Waybill == waybill && x.Status != "TimedOut");

                var partnerDTO =   (from n in mobileRequests
                                              join partner in _context.Partners on n.UserId equals partner.UserId
                                              select new PartnerDTO
                                              {
                                                  PartnerName = partner.FirstName + " " + partner.LastName,
                                                  PhoneNumber = partner.PhoneNumber,
                                                  PartnerCode = partner.PartnerCode,
                                                  PartnerType = partner.PartnerType
                                              }).FirstOrDefault();

                return Task.FromResult(partnerDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
