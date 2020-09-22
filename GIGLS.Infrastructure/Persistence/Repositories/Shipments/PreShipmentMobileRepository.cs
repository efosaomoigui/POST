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
                               join co in _context.Country on r.CountryId equals co.CountryId
                               select new PreShipmentMobileDTO()
                               {
                                   PreShipmentMobileId = r.PreShipmentMobileId,
                                   Waybill = r.Waybill,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   ReceiverAddress = r.ReceiverAddress,
                                   SenderAddress = r.SenderAddress,
                                   shipmentstatus = r.shipmentstatus,
                                   GrandTotal = r.GrandTotal,
                                   CurrencySymbol = co.CurrencySymbol,
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


    }
}