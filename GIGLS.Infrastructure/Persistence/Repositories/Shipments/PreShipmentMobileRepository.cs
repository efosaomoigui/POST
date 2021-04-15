using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Report;
using System.Data.SqlClient;
using GIGLS.Core.Enums;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class PreShipmentMobileRepository : Repository<PreShipmentMobile, GIGLSContext>, IPreShipmentMobileRepository
    {
        private GIGLSContext _context;
        public PreShipmentMobileRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<PreShipmentMobileDTO>> GetPreShipmentsForMobile()
        {
            try
            {
                var preShipmentMobile = _context.PresShipmentMobile.AsQueryable();

                List<PreShipmentMobileDTO> preShipmentDto = new List<PreShipmentMobileDTO>();

                preShipmentDto = (from r in preShipmentMobile
                                  select new PreShipmentMobileDTO()
                                  {
                                      PreShipmentMobileId = r.PreShipmentMobileId,
                                      Waybill = r.Waybill,
                                      CustomerType = r.CustomerType,
                                      ActualDateOfArrival = r.ActualDateOfArrival,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified,
                                      ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                      ReceiverAddress = r.ReceiverAddress,
                                      SenderAddress = r.SenderAddress,
                                      ReceiverCity = r.ReceiverCity,
                                      ReceiverCountry = r.ReceiverCountry,
                                      ReceiverEmail = r.ReceiverEmail,
                                      ReceiverName = r.ReceiverName,
                                      ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                      ReceiverState = r.ReceiverState,
                                      SenderName = r.SenderName,
                                      UserId = r.UserId,
                                      Value = r.Value,
                                      GrandTotal = r.GrandTotal,
                                      DeliveryPrice = r.DeliveryPrice,
                                      CalculatedTotal = r.CalculatedTotal,
                                      InsuranceValue = r.InsuranceValue,
                                      DiscountValue = r.DiscountValue,
                                      CompanyType = r.CompanyType,
                                      CustomerCode = r.CustomerCode
                                  }).OrderByDescending(x => x.DateCreated).Take(20).ToList();

                return Task.FromResult(preShipmentDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PreShipmentMobileDTO> GetBasicPreShipmentMobileDetail(string waybill)
        {
            var preShipment = _context.PresShipmentMobile.Where(x => x.Waybill == waybill);

            PreShipmentMobileDTO preShipmentDto = (from r in preShipment
                                                   select new PreShipmentMobileDTO
                                                   {
                                                       PreShipmentMobileId = r.PreShipmentMobileId,
                                                       Waybill = r.Waybill,
                                                       CustomerType = r.CustomerType,
                                                       ActualDateOfArrival = r.ActualDateOfArrival,
                                                       DateCreated = r.DateCreated,
                                                       DateModified = r.DateModified,
                                                       ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                       ReceiverAddress = r.ReceiverAddress,
                                                       SenderAddress = r.SenderAddress,
                                                       ReceiverCity = r.ReceiverCity,
                                                       ReceiverCountry = r.ReceiverCountry,
                                                       ReceiverEmail = r.ReceiverEmail,
                                                       ReceiverName = r.ReceiverName,
                                                       ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                       ReceiverState = r.ReceiverState,
                                                       SenderName = r.SenderName,
                                                       UserId = r.UserId,
                                                       Value = r.Value,
                                                       GrandTotal = r.GrandTotal,
                                                       DeliveryPrice = r.DeliveryPrice,
                                                       CalculatedTotal = r.CalculatedTotal,
                                                       InsuranceValue = r.InsuranceValue,
                                                       DiscountValue = r.DiscountValue,
                                                       CompanyType = r.CompanyType,
                                                       CustomerCode = r.CustomerCode,
                                                       ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                       Total = r.Total,
                                                       CashOnDeliveryAmount = r.CashOnDeliveryAmount,
                                                       IsCashOnDelivery = r.IsCashOnDelivery,
                                                       WaybillImageUrl = r.WaybillImageUrl,
                                                   }).FirstOrDefault();

            return Task.FromResult(preShipmentDto);
        }

        public IQueryable<PreShipmentMobileDTO> GetPreShipmentForUser(string userChannelCode)
        {
            var preShipments =  Context.PresShipmentMobile.AsQueryable().Where(s => s.CustomerCode == userChannelCode);

            var shipmentDto = (from r in preShipments
                               join co in _context.Country on r.CountryId equals co.CountryId
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.PreShipmentMobileId,
                                   Waybill = r.Waybill,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   shipmentstatus = r.shipmentstatus,
                                   GrandTotal = r.GrandTotal,
                                   CurrencySymbol = co.CurrencySymbol,
                                   IsDelivered = r.IsDelivered,
                                   IsBatchPickUp = r.IsBatchPickUp,
                                   IsCancelled = r.IsCancelled,
                                   PreShipmentItems = _context.PresShipmentItemMobile.Where(d => d.PreShipmentMobileId == r.PreShipmentMobileId)
                                      .Select(y => new PreShipmentItemMobileDTO
                                      {
                                          PreShipmentItemMobileId = y.PreShipmentItemMobileId,
                                          Description = y.Description
                                      }).ToList()
                               });

            return shipmentDto;
        }

        public IQueryable<PreShipmentMobileDTO> GetShipments(string userChannelCode)
        {
            var preShipments = Context.Shipment.AsQueryable().Where(x => x.CustomerCode == userChannelCode && x.IsCancelled == false);

            var shipmentDto = (from r in preShipments
                               join co in _context.Country on r.DepartureCountryId equals co.CountryId
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.ShipmentId,
                                   Waybill = r.Waybill,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ReceiverAddress = r.ReceiverAddress,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   SenderAddress = r.SenderAddress,
                                   GrandTotal = r.GrandTotal,
                                   shipmentstatus = "Shipment",
                                   CurrencySymbol = co.CurrencySymbol,
                                   CustomerType = r.CustomerType,
                                   CustomerId = r.CustomerId,
                                   PreShipmentItems = _context.ShipmentItem.Where(d => d.ShipmentId == r.ShipmentId)
                                      .Select(y => new PreShipmentItemMobileDTO
                                      {
                                          PreShipmentItemMobileId = y.ShipmentItemId,
                                          Description = y.Description
                                      }).ToList()
                               });

            return shipmentDto;
        }

        //Get Shipments that have not been paid for by user
        public Task<List<OutstandingPaymentsDTO>> GetAllOutstandingShipmentsForUser(string userChannelCode)
        {
            var shipments = _context.Shipment.AsQueryable().Where(s => s.CustomerCode == userChannelCode);

            var result = (from s in shipments
                          join i in Context.Invoice on s.Waybill equals i.Waybill
                          join dept in Context.ServiceCentre on s.DepartureServiceCentreId equals dept.ServiceCentreId
                          join dest in Context.ServiceCentre on s.DestinationServiceCentreId equals dest.ServiceCentreId
                          where i.PaymentStatus == Core.Enums.PaymentStatus.Pending
                          select new OutstandingPaymentsDTO()
                          {
                              Waybill = s.Waybill,
                              Departure = dept.Name,
                              Destination = dest.Name,
                              Amount = i.Amount,
                              CurrencySymbol = Context.Country.Where(c => c.CountryId == i.CountryId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                              DateCreated = s.DateCreated
                          }).ToList();

            return Task.FromResult(result.OrderByDescending(x => x.DateCreated).ToList()); ;
        }

        public async Task<List<PreShipmentMobileReportDTO>> GetPreShipments(MobileShipmentFilterCriteria accountFilterCriteria)
        {
            try
            {
                //DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
                //DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

                //get startDate and endDate
                var queryDate = accountFilterCriteria.getStartDateAndEndDate();
                var StartDate = queryDate.Item1;
                var EndDate = queryDate.Item2;

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
                SqlParameter endDate = new SqlParameter("@EndDate", EndDate);
                SqlParameter departureStationId = new SqlParameter("@DepartureStationId", accountFilterCriteria.DepartureStationId);
                SqlParameter destinationStationId = new SqlParameter("@DestinationStationId", accountFilterCriteria.DestinationStationId);
                SqlParameter countryId = new SqlParameter("@CountryId", accountFilterCriteria.CountryId);
                SqlParameter vehicleType = new SqlParameter("@VehicleType", (object)accountFilterCriteria.VehicleType ?? DBNull.Value);
                SqlParameter companyType = new SqlParameter("@CompanyType", (object)accountFilterCriteria.CompanyType ?? DBNull.Value);
                SqlParameter shipmentstatus = new SqlParameter("@Shipmentstatus", (object)accountFilterCriteria.Shipmentstatus ?? DBNull.Value);

                SqlParameter[] param = new SqlParameter[]
                {
                startDate,
                endDate,
                departureStationId,
                destinationStationId,
                countryId,
                vehicleType,
                companyType,
                shipmentstatus
                };

                //var listCreated = new List<PreShipmentMobileReportDTO>();

               var listCreated = await _context.Database.SqlQuery<PreShipmentMobileReportDTO>("GIGGOReporting " +
                  "@StartDate, @EndDate, @DepartureStationId, @DestinationStationId, @CountryId, @VehicleType, @CompanyType, @Shipmentstatus",
                  param).ToListAsync();

                return await Task.FromResult(listCreated);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<PreShipmentMobileDTO> GetBatchedPreShipmentForUser(string userChannelCode)
        {
            var preShipments = Context.PresShipmentMobile.AsQueryable().Where(s => s.CustomerCode == userChannelCode && s.IsBatchPickUp == true);

            var shipmentDto = (from r in preShipments
                               join co in _context.Country on r.CountryId equals co.CountryId
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.PreShipmentMobileId,
                                   Waybill = r.Waybill,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   shipmentstatus = r.shipmentstatus,
                                   GrandTotal = r.GrandTotal,
                                   CurrencySymbol = co.CurrencySymbol,
                                   IsDelivered = r.IsDelivered,
                                   IsBatchPickUp = r.IsBatchPickUp,
                                   IsCancelled = r.IsCancelled
                               });

            return shipmentDto;
        }

        public IQueryable<PreShipmentMobileDTO> GetAllBatchedPreShipment()
        {
            var preShipments = Context.PresShipmentMobile.AsQueryable().Where(s => s.IsBatchPickUp == true);

            var shipmentDto = (from r in preShipments
                               join co in _context.Country on r.CountryId equals co.CountryId
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.PreShipmentMobileId,
                                   Waybill = r.Waybill,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   shipmentstatus = r.shipmentstatus,
                                   CustomerCode = r.CustomerCode
                               });

            return shipmentDto;
        }



        public async Task<List<AddressDTO>> GetTopFiveUserAddresses(string userID)
        {
            var preShipments = Context.PresShipmentMobile.AsQueryable().Where(s => s.UserId == userID);

            var address = (from r in preShipments
                               select new AddressDTO()
                               {
                                   ReceiverAddress = r.ReceiverAddress,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverStationName = Context.Station.FirstOrDefault(x => x.StationId == r.ReceiverStationId).StationName,
                                   ReceiverLat = Context.Location.FirstOrDefault(x => x.LocationId == r.ReceiverLocation.LocationId).Latitude,
                                   ReceiverLng = Context.Location.FirstOrDefault(x => x.LocationId == r.ReceiverLocation.LocationId).Longitude,
                                   ReceiverLGA = Context.Location.FirstOrDefault(x => x.LocationId == r.ReceiverLocation.LocationId).LGA,
                                   SenderAddress = r.SenderAddress,
                                   SenderName = r.SenderName,
                                   DateCreated = r.DateCreated,
                                   SenderStationName = Context.Station.FirstOrDefault(x => x.StationId == r.SenderStationId).StationName,
                                   SenderLat = Context.Location.FirstOrDefault(x => x.LocationId == r.SenderLocation.LocationId).Latitude,
                                   SenderLng = Context.Location.FirstOrDefault(x => x.LocationId == r.SenderLocation.LocationId).Longitude,
                                   SenderLGA = Context.Location.FirstOrDefault(x => x.LocationId == r.SenderLocation.LocationId).LGA,
                               }).Distinct().OrderByDescending(x => x.DateCreated).Take(5).ToList();

            return address;
        }

    }
}