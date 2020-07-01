using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO;

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
                                                   }).FirstOrDefault();

            return Task.FromResult(preShipmentDto);
        }

        public IQueryable<PreShipmentMobileDTO> GetPreShipmentForUser(string userChannelCode)
        {
            var preShipments = Context.PresShipmentMobile.AsQueryable().Where(s => s.CustomerCode == userChannelCode);

            var shipmentDto = (from r in preShipments
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.PreShipmentMobileId,
                                   Waybill = r.Waybill,
                                   ActualDateOfArrival = r.ActualDateOfArrival,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   SenderPhoneNumber = r.SenderPhoneNumber,
                                   ReceiverCountry = r.ReceiverCountry,
                                   SenderStationId = r.SenderStationId,
                                   ReceiverStationId = r.ReceiverStationId,
                                   ReceiverEmail = r.ReceiverEmail,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   ReceiverState = r.ReceiverState,
                                   SenderName = r.SenderName,
                                   UserId = r.UserId,
                                   Value = r.Value,
                                   shipmentstatus = r.shipmentstatus,
                                   GrandTotal = r.GrandTotal,
                                   DeliveryPrice = r.DeliveryPrice,
                                   CalculatedTotal = r.CalculatedTotal,
                                   CustomerCode = r.CustomerCode,
                                   VehicleType = r.VehicleType,
                                   InputtedSenderAddress = r.InputtedSenderAddress,
                                   InputtedReceiverAddress = r.InputtedReceiverAddress,
                                   SenderLocality = r.SenderLocality,
                                   ReceiverLocation = new LocationDTO
                                   {
                                       Longitude = r.ReceiverLocation.Longitude,
                                       Latitude = r.ReceiverLocation.Latitude,
                                       Name = r.ReceiverLocation.Name,
                                       FormattedAddress = r.ReceiverLocation.FormattedAddress
                                   },
                                   SenderLocation = new LocationDTO
                                   {
                                       Longitude = r.SenderLocation.Longitude,
                                       Latitude = r.SenderLocation.Latitude,
                                       Name = r.ReceiverLocation.Name,
                                       FormattedAddress = r.ReceiverLocation.FormattedAddress
                                   }
                               });

            return shipmentDto;
        }

        public IQueryable<PreShipmentMobileDTO> GetPreShipmentForEcommerce(string userChannelCode)
        {
            var preShipments = Context.Shipment.AsQueryable().Where(x => x.CustomerCode == userChannelCode && x.IsCancelled == false);

            var shipmentDto = (from r in preShipments
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.ShipmentId,
                                   Waybill = r.Waybill,
                                   ActualDateOfArrival = r.ActualDateOfArrival,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   ReceiverCountry = r.ReceiverCountry,
                                   ReceiverEmail = r.ReceiverEmail,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   ReceiverState = r.ReceiverState,
                                   UserId = r.UserId,
                                   Value = r.Value,
                                   GrandTotal = r.GrandTotal,
                                   CustomerCode = r.CustomerCode,
                                   DepartureServiceCentreId = r.DepartureServiceCentreId,
                                   shipmentstatus = "Shipment",
                                   CustomerId = r.CustomerId,
                                   CustomerType = r.CustomerType,
                                   CountryId = r.DepartureCountryId
                               });

            return shipmentDto;
        }


    }
}