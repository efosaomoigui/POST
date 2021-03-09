using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.CORE.Enums;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;
using System.Data.SqlClient;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentRepository : Repository<Shipment, GIGLSContext>, IShipmentRepository
    {
        private GIGLSContext _context;
        public ShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds)
        {
            var shipment = _context.Shipment.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                shipment = shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            //filter by cancelled shipments
            shipment = shipment.Where(s => s.IsCancelled == false);


            List<ShipmentDTO> shipmentDto = (from r in shipment
                                             select new ShipmentDTO()
                                             {
                                                 ShipmentId = r.ShipmentId,
                                                 Waybill = r.Waybill,
                                                 CustomerId = r.CustomerId,
                                                 CustomerType = r.CustomerType,
                                                 ActualDateOfArrival = r.ActualDateOfArrival,
                                                 //ActualReceiverName = r.ActualReceiverName,
                                                 //ActualreceiverPhone = r.ActualreceiverPhone,
                                                 //Comments = r.Comments,
                                                 DateCreated = r.DateCreated,
                                                 DateModified = r.DateModified,
                                                 DeliveryOptionId = r.DeliveryOptionId,
                                                 DeliveryOption = new DeliveryOptionDTO
                                                 {
                                                     Code = r.DeliveryOption.Code,
                                                     Description = r.DeliveryOption.Description
                                                 },
                                                 DeliveryTime = r.DeliveryTime,
                                                 DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                 DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),
                                                 DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                 DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),
                                                 ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                 //GroupWaybill = r.GroupWaybill,
                                                 //IdentificationType = r.IdentificationType,
                                                 //IndentificationUrl = r.IndentificationUrl,
                                                 //IsDomestic = r.IsDomestic,
                                                 PaymentStatus = r.PaymentStatus,
                                                 ReceiverAddress = r.ReceiverAddress,
                                                 ReceiverCity = r.ReceiverCity,
                                                 ReceiverCountry = r.ReceiverCountry,
                                                 ReceiverEmail = r.ReceiverEmail,
                                                 ReceiverName = r.ReceiverName,
                                                 ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                 ReceiverState = r.ReceiverState,
                                                 SealNumber = r.SealNumber,
                                                 UserId = r.UserId,
                                                 Value = r.Value,
                                                 GrandTotal = r.GrandTotal,
                                                 AppliedDiscount = r.AppliedDiscount,
                                                 DiscountValue = r.DiscountValue,
                                                 ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                 ShipmentPickupPrice = r.ShipmentPickupPrice,
                                                 CompanyType = r.CompanyType,
                                                 CustomerCode = r.CustomerCode,
                                                 Description = r.Description,
                                                 SenderAddress = r.SenderAddress,
                                                 SenderState = r.SenderState,
                                                 ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                 DepartureCountryId = r.DepartureCountryId,
                                                 DestinationCountryId = r.DestinationCountryId,
                                                 CurrencyRatio = r.CurrencyRatio,

                                                 ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                                 {
                                                     CancelReason = x.CancelReason
                                                 }).FirstOrDefault(),
                                                 //DepartureTerminalName = r.DepartureTerminal.Name,
                                                 //DestinationTerminalName = r.DestinationTerminal.Name       
                                                 //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()z
                                             }).ToList();


            return Task.FromResult(shipmentDto.ToList());
        }

        public async Task<Tuple<List<ShipmentDTO>, int>> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                //filter by service center
                var shipment = _context.Shipment.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    shipment = shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
                }

                //filter by country Id
                if (filterOptionsDto.CountryId > 0)
                {
                    shipment = shipment.Where(s => s.DepartureCountryId == filterOptionsDto.CountryId);
                }

                //filter by cancelled shipments
                shipment = shipment.Where(s => s.IsCancelled == false);

                //filter by Local or International Shipment
                //if (filterOptionsDto.IsInternational != null)
                //{
                //    shipment = shipment.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                //}

                var count = 0;

                List<ShipmentDTO> shipmentDto = new List<ShipmentDTO>();

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(filterValue))
                {
                    shipmentDto = (from r in shipment
                                   select new ShipmentDTO()
                                   {
                                       ShipmentId = r.ShipmentId,
                                       Waybill = r.Waybill,
                                       CustomerId = r.CustomerId,
                                       CustomerType = r.CustomerType,
                                       ActualDateOfArrival = r.ActualDateOfArrival,
                                       DateCreated = r.DateCreated,
                                       DateModified = r.DateModified,
                                       DeliveryOptionId = r.DeliveryOptionId,
                                       DeliveryOption = new DeliveryOptionDTO
                                       {
                                           Code = r.DeliveryOption.Code,
                                           Description = r.DeliveryOption.Description
                                       },
                                       DeliveryTime = r.DeliveryTime,
                                       DepartureServiceCentreId = r.DepartureServiceCentreId,
                                       DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                       {
                                           Code = x.Code,
                                           Name = x.Name
                                       }).FirstOrDefault(),
                                       DestinationServiceCentreId = r.DestinationServiceCentreId,
                                       DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                       {
                                           Code = x.Code,
                                           Name = x.Name
                                       }).FirstOrDefault(),
                                       ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                       PaymentStatus = r.PaymentStatus,
                                       ReceiverAddress = r.ReceiverAddress,
                                       ReceiverCity = r.ReceiverCity,
                                       ReceiverCountry = r.ReceiverCountry,
                                       ReceiverEmail = r.ReceiverEmail,
                                       ReceiverName = r.ReceiverName,
                                       ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                       ReceiverState = r.ReceiverState,
                                       SealNumber = r.SealNumber,
                                       UserId = r.UserId,
                                       Value = r.Value,
                                       GrandTotal = r.GrandTotal,
                                       AppliedDiscount = r.AppliedDiscount,
                                       DiscountValue = r.DiscountValue,
                                       ShipmentPackagePrice = r.ShipmentPackagePrice,
                                       ShipmentPickupPrice = r.ShipmentPickupPrice,
                                       CompanyType = r.CompanyType,
                                       CustomerCode = r.CustomerCode,
                                       Description = r.Description,
                                       SenderAddress = r.SenderAddress,
                                       SenderState = r.SenderState,
                                       ReprintCounterStatus = r.ReprintCounterStatus,
                                       ApproximateItemsWeight = r.ApproximateItemsWeight,
                                       DepartureCountryId = r.DepartureCountryId,
                                       DestinationCountryId = r.DestinationCountryId,
                                       CurrencyRatio = r.CurrencyRatio,
                                       ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                       {
                                           CancelReason = x.CancelReason
                                       }).FirstOrDefault(),
                                       Invoice = Context.Invoice.Where(c => c.Waybill == r.Waybill).Select(x => new InvoiceDTO
                                       {
                                           InvoiceId = x.InvoiceId,
                                           InvoiceNo = x.InvoiceNo,
                                           Amount = x.Amount,
                                           PaymentStatus = x.PaymentStatus,
                                           PaymentMethod = x.PaymentMethod,
                                           PaymentDate = x.PaymentDate,
                                           Waybill = x.Waybill,
                                           DueDate = x.DueDate,
                                           IsInternational = x.IsInternational
                                       }).FirstOrDefault()
                                   }).OrderByDescending(x => x.DateCreated).Take(10).ToList();

                    count = shipmentDto.Count();

                    return new Tuple<List<ShipmentDTO>, int>(shipmentDto, count);
                }

                shipmentDto = (from r in shipment
                               select new ShipmentDTO()
                               {
                                   ShipmentId = r.ShipmentId,
                                   Waybill = r.Waybill,
                                   CustomerId = r.CustomerId,
                                   CustomerType = r.CustomerType,
                                   ActualDateOfArrival = r.ActualDateOfArrival,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified,
                                   DeliveryOptionId = r.DeliveryOptionId,
                                   DeliveryOption = new DeliveryOptionDTO
                                   {
                                       Code = r.DeliveryOption.Code,
                                       Description = r.DeliveryOption.Description
                                   },
                                   DeliveryTime = r.DeliveryTime,
                                   DepartureServiceCentreId = r.DepartureServiceCentreId,
                                   DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                   {
                                       Code = x.Code,
                                       Name = x.Name
                                   }).FirstOrDefault(),
                                   DestinationServiceCentreId = r.DestinationServiceCentreId,
                                   DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                   {
                                       Code = x.Code,
                                       Name = x.Name
                                   }).FirstOrDefault(),
                                   ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                   PaymentStatus = r.PaymentStatus,
                                   ReceiverAddress = r.ReceiverAddress,
                                   ReceiverCity = r.ReceiverCity,
                                   ReceiverCountry = r.ReceiverCountry,
                                   ReceiverEmail = r.ReceiverEmail,
                                   ReceiverName = r.ReceiverName,
                                   ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                   ReceiverState = r.ReceiverState,
                                   SealNumber = r.SealNumber,
                                   UserId = r.UserId,
                                   Value = r.Value,
                                   GrandTotal = r.GrandTotal,
                                   AppliedDiscount = r.AppliedDiscount,
                                   DiscountValue = r.DiscountValue,
                                   ShipmentPackagePrice = r.ShipmentPackagePrice,
                                   ShipmentPickupPrice = r.ShipmentPickupPrice,
                                   CompanyType = r.CompanyType,
                                   CustomerCode = r.CustomerCode,
                                   Description = r.Description,
                                   SenderAddress = r.SenderAddress,
                                   SenderState = r.SenderState,
                                   ApproximateItemsWeight = r.ApproximateItemsWeight,
                                   DepartureCountryId = r.DepartureCountryId,
                                   DestinationCountryId = r.DestinationCountryId,
                                   CurrencyRatio = r.CurrencyRatio,
                                   ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                   {
                                       CancelReason = x.CancelReason
                                   }).FirstOrDefault(),
                                   Invoice = Context.Invoice.Where(c => c.Waybill == r.Waybill).Select(x => new InvoiceDTO
                                   {
                                       InvoiceId = x.InvoiceId,
                                       InvoiceNo = x.InvoiceNo,
                                       Amount = x.Amount,
                                       PaymentStatus = x.PaymentStatus,
                                       PaymentMethod = x.PaymentMethod,
                                       PaymentDate = x.PaymentDate,
                                       Waybill = x.Waybill,
                                       DueDate = x.DueDate,
                                       IsInternational = x.IsInternational
                                   }).FirstOrDefault()
                               }).Where(s => (s.Waybill == filterValue || s.GrandTotal.ToString() == filterValue || s.DateCreated.ToString() == filterValue)).ToList();

                //filter
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    shipmentDto = shipmentDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        shipmentDto = shipmentDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                    else
                    {
                        shipmentDto = shipmentDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                }

                shipmentDto = shipmentDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                count = shipmentDto.Count();
                return new Tuple<List<ShipmentDTO>, int>(shipmentDto, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<List<ShipmentDTO>, int>> GetDestinationShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                //filter by destination service center that is not cancelled
                var shipment = _context.Shipment.AsQueryable().Where(x => x.IsCancelled == false);
                if (serviceCentreIds.Length > 0)
                {
                    shipment = shipment.Where(s => serviceCentreIds.Contains(s.DestinationServiceCentreId));
                }

                //filter by Local or International Shipment
                if (filterOptionsDto.IsInternational != null)
                {
                    shipment = shipment.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                }

                var count = shipment.ToList().Count();

                List<ShipmentDTO> shipmentDto = (from r in shipment
                                                 select new ShipmentDTO()
                                                 {
                                                     ShipmentId = r.ShipmentId,
                                                     Waybill = r.Waybill,
                                                     CustomerId = r.CustomerId,
                                                     CustomerType = r.CustomerType,
                                                     ActualDateOfArrival = r.ActualDateOfArrival,
                                                     DateCreated = r.DateCreated,
                                                     DateModified = r.DateModified,
                                                     DeliveryOptionId = r.DeliveryOptionId,
                                                     DeliveryOption = new DeliveryOptionDTO
                                                     {
                                                         Code = r.DeliveryOption.Code,
                                                         Description = r.DeliveryOption.Description
                                                     },
                                                     DeliveryTime = r.DeliveryTime,
                                                     DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                     DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
                                                     DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                     DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),

                                                     ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                     PaymentStatus = r.PaymentStatus,
                                                     ReceiverAddress = r.ReceiverAddress,
                                                     ReceiverCity = r.ReceiverCity,
                                                     ReceiverCountry = r.ReceiverCountry,
                                                     ReceiverEmail = r.ReceiverEmail,
                                                     ReceiverName = r.ReceiverName,
                                                     ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                     ReceiverState = r.ReceiverState,
                                                     SealNumber = r.SealNumber,
                                                     UserId = r.UserId,
                                                     Value = r.Value,
                                                     GrandTotal = r.GrandTotal,
                                                     AppliedDiscount = r.AppliedDiscount,
                                                     DiscountValue = r.DiscountValue,
                                                     ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                     ShipmentPickupPrice = r.ShipmentPickupPrice,
                                                     CompanyType = r.CompanyType,
                                                     CustomerCode = r.CustomerCode,
                                                     Description = r.Description,
                                                     SenderAddress = r.SenderAddress,
                                                     SenderState = r.SenderState,
                                                     ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                     DepartureCountryId = r.DepartureCountryId,
                                                     DestinationCountryId = r.DestinationCountryId,
                                                     CurrencyRatio = r.CurrencyRatio,
                                                 }).ToList();

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    shipmentDto = shipmentDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString() == filterValue).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        shipmentDto = shipmentDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                    else
                    {
                        shipmentDto = shipmentDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                }

                shipmentDto = shipmentDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();

                return new Tuple<List<ShipmentDTO>, int>(shipmentDto.ToList(), count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Shipment Detail for list of waybills and filter it by service centre
        public async Task<Tuple<List<ShipmentDTO>, int>> GetShipmentDetailByWaybills(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds, List<string> waybills)
        {
            try
            {
                //filter by destination service center that is not cancelled
                var shipment = _context.Shipment.AsQueryable().Where(x => x.IsCancelled == false);

                if (serviceCentreIds.Length > 0)
                {
                    shipment = shipment.Where(s => serviceCentreIds.Contains(s.DestinationServiceCentreId));
                }

                //filter by Local or International Shipment
                if (filterOptionsDto.IsInternational != null)
                {
                    shipment = shipment.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                }

                if (waybills.Count > 0)
                {
                    shipment = shipment.Where(s => waybills.Contains(s.Waybill));
                }

                var count = shipment.ToList().Count();

                List<ShipmentDTO> shipmentDto = (from r in shipment
                                                 select new ShipmentDTO()
                                                 {
                                                     ShipmentId = r.ShipmentId,
                                                     Waybill = r.Waybill,
                                                     CustomerId = r.CustomerId,
                                                     CustomerType = r.CustomerType,
                                                     ActualDateOfArrival = r.ActualDateOfArrival,
                                                     DateCreated = r.DateCreated,
                                                     DateModified = r.DateModified,
                                                     DeliveryOptionId = r.DeliveryOptionId,
                                                     DeliveryOption = new DeliveryOptionDTO
                                                     {
                                                         Code = r.DeliveryOption.Code,
                                                         Description = r.DeliveryOption.Description
                                                     },
                                                     DeliveryTime = r.DeliveryTime,
                                                     DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                     DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
                                                     DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                     DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),

                                                     ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                     PaymentStatus = r.PaymentStatus,
                                                     ReceiverAddress = r.ReceiverAddress,
                                                     ReceiverCity = r.ReceiverCity,
                                                     ReceiverCountry = r.ReceiverCountry,
                                                     ReceiverEmail = r.ReceiverEmail,
                                                     ReceiverName = r.ReceiverName,
                                                     ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                     ReceiverState = r.ReceiverState,
                                                     SealNumber = r.SealNumber,
                                                     UserId = r.UserId,
                                                     Value = r.Value,
                                                     GrandTotal = r.GrandTotal,
                                                     AppliedDiscount = r.AppliedDiscount,
                                                     DiscountValue = r.DiscountValue,
                                                     ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                     ShipmentPickupPrice = r.ShipmentPickupPrice,
                                                     CompanyType = r.CompanyType,
                                                     CustomerCode = r.CustomerCode,
                                                     Description = r.Description,
                                                     SenderAddress = r.SenderAddress,
                                                     SenderState = r.SenderState,
                                                     ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                     DepartureCountryId = r.DepartureCountryId,
                                                     DestinationCountryId = r.DestinationCountryId,
                                                     CurrencyRatio = r.CurrencyRatio,
                                                 }).ToList();

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    shipmentDto = shipmentDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString() == filterValue).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        shipmentDto = shipmentDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                    else
                    {
                        shipmentDto = shipmentDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                }

                shipmentDto = shipmentDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();

                return new Tuple<List<ShipmentDTO>, int>(shipmentDto.ToList(), count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria f_Criteria, int[] serviceCentreIds)
        {
            DateTime StartDate = f_Criteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = f_Criteria.EndDate?.Date ?? StartDate;

            //filter by service center
            var shipments = _context.Shipment.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }
            ////

            //filter by cancelled shipments
            shipments = shipments.Where(s => s.IsCancelled == false);

            //get startDate and endDate
            var queryDate = f_Criteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            shipments = shipments.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            if (f_Criteria.ServiceCentreId > 0)
            {
                shipments = shipments.Where(x => x.DepartureServiceCentreId == f_Criteria.ServiceCentreId);
            }

            if (f_Criteria.StationId > 0)
            {
                var serviceCentre = Context.ServiceCentre.Where(y => y.StationId == f_Criteria.StationId);
                shipments = shipments.Where(y => serviceCentre.Any(x => y.DepartureServiceCentreId == x.ServiceCentreId));
            }

            if (f_Criteria.StateId > 0)
            {
                var station = Context.Station.Where(s => s.StateId == f_Criteria.StateId);
                var serviceCentre = Context.ServiceCentre.Where(w => station.Any(x => w.StationId == x.StationId));
                shipments = shipments.Where(y => serviceCentre.Any(x => y.DepartureServiceCentreId == x.ServiceCentreId));
            }

            if (!string.IsNullOrWhiteSpace(f_Criteria.UserId))
            {
                shipments = shipments.Where(x => x.UserId == f_Criteria.UserId);
            }

            if (f_Criteria.FilterCustomerType.HasValue && f_Criteria.FilterCustomerType.Equals(FilterCustomerType.IndividualCustomer))
            {
                shipments = shipments.Where(x => x.CustomerType.Equals(CustomerType.IndividualCustomer.ToString()));
            }

            if (f_Criteria.FilterCustomerType.HasValue && !f_Criteria.FilterCustomerType.Equals(FilterCustomerType.IndividualCustomer))
            {
                shipments = shipments.Where(x => x.CustomerType.Equals(CustomerType.Company.ToString()));

                if (f_Criteria.FilterCustomerType.Equals(FilterCustomerType.Corporate))
                {
                    var corporate = Context.Company.Where(x => x.CompanyType == CompanyType.Corporate);
                    shipments = shipments.Where(y => corporate.Any(x => y.CustomerId == x.CompanyId));
                }
                else
                {
                    var ecommerce = Context.Company.Where(x => x.CompanyType == CompanyType.Ecommerce);
                    shipments = shipments.Where(y => ecommerce.Any(x => y.CustomerId == x.CompanyId));
                }
            }

            List<ShipmentDTO> shipmentDto = (from r in shipments
                                             select new ShipmentDTO()
                                             {
                                                 ShipmentId = r.ShipmentId,
                                                 Waybill = r.Waybill,
                                                 CustomerId = r.CustomerId,
                                                 CustomerType = r.CustomerType,
                                                 ActualDateOfArrival = r.ActualDateOfArrival,
                                                 DateCreated = r.DateCreated,
                                                 DateModified = r.DateModified,
                                                 DeliveryOptionId = r.DeliveryOptionId,
                                                 DeliveryOption = new DeliveryOptionDTO
                                                 {
                                                     Code = r.DeliveryOption.Code,
                                                     Description = r.DeliveryOption.Description
                                                 },
                                                 DeliveryTime = r.DeliveryTime,
                                                 DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                 DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),

                                                 DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                 DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),

                                                 ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                 PaymentStatus = r.PaymentStatus,
                                                 ReceiverAddress = r.ReceiverAddress,
                                                 ReceiverCity = r.ReceiverCity,
                                                 ReceiverCountry = r.ReceiverCountry,
                                                 ReceiverEmail = r.ReceiverEmail,
                                                 ReceiverName = r.ReceiverName,
                                                 ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                 ReceiverState = r.ReceiverState,
                                                 SealNumber = r.SealNumber,
                                                 UserId = r.UserId,
                                                 Value = r.Value,
                                                 GrandTotal = r.GrandTotal,
                                                 AppliedDiscount = r.AppliedDiscount,
                                                 DiscountValue = r.DiscountValue,
                                                 ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                 ShipmentPickupPrice = r.ShipmentPickupPrice,
                                                 CompanyType = r.CompanyType,
                                                 CustomerCode = r.CustomerCode,
                                                 Description = r.Description,
                                                 SenderAddress = r.SenderAddress,
                                                 SenderState = r.SenderState,
                                                 ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                 DepartureCountryId = r.DepartureCountryId,
                                                 DestinationCountryId = r.DestinationCountryId,
                                                 CurrencyRatio = r.CurrencyRatio,
                                                 ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                                 {
                                                     CancelReason = x.CancelReason
                                                 }).FirstOrDefault()
                                             }).ToList();

            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria)
        {
            DateTime StartDate = f_Criteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = f_Criteria.EndDate?.Date ?? StartDate;

            //filter by service center
            var shipments = _context.Shipment.AsQueryable();

            //filter by cancelled shipments
            shipments = shipments.Where(s => s.IsCancelled == false);

            //If No Date Supply
            if (!f_Criteria.StartDate.HasValue && !f_Criteria.EndDate.HasValue)
            {
                var Today = DateTime.Today;
                var nextDay = DateTime.Today.AddDays(1).Date;
                shipments = shipments.Where(x => x.DateCreated >= Today && x.DateCreated < nextDay);
            }

            if (f_Criteria.StartDate.HasValue && f_Criteria.EndDate.HasValue)
            {
                if (f_Criteria.StartDate.Equals(f_Criteria.EndDate))
                {
                    var nextDay = DateTime.Today.AddDays(1).Date;
                    shipments = shipments.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
                }
                else
                {
                    var dayAfterEndDate = EndDate.AddDays(1).Date;
                    shipments = shipments.Where(x => x.DateCreated >= StartDate && x.DateCreated < dayAfterEndDate);
                }
            }

            if (f_Criteria.StartDate.HasValue && !f_Criteria.EndDate.HasValue)
            {
                var nextDay = DateTime.Today.AddDays(1).Date;
                shipments = shipments.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
            }

            if (f_Criteria.EndDate.HasValue && !f_Criteria.StartDate.HasValue)
            {
                var dayAfterEndDate = EndDate.AddDays(1).Date;
                shipments = shipments.Where(x => x.DateCreated < dayAfterEndDate);
            }

            if (f_Criteria.ServiceCentreId > 0)
            {
                shipments = shipments.Where(x => x.DepartureServiceCentreId == f_Criteria.ServiceCentreId);
            }

            if (f_Criteria.StationId > 0)
            {
                var serviceCentre = Context.ServiceCentre.Where(y => y.StationId == f_Criteria.StationId);
                shipments = shipments.Where(y => serviceCentre.Any(x => y.DepartureServiceCentreId == x.ServiceCentreId));
            }

            if (f_Criteria.StateId > 0)
            {
                var station = Context.Station.Where(s => s.StateId == f_Criteria.StateId);
                var serviceCentre = Context.ServiceCentre.Where(w => station.Any(x => w.StationId == x.StationId));
                shipments = shipments.Where(y => serviceCentre.Any(x => y.DepartureServiceCentreId == x.ServiceCentreId));
            }

            if (!string.IsNullOrWhiteSpace(f_Criteria.UserId))
            {
                shipments = shipments.Where(x => x.UserId == f_Criteria.UserId);
            }

            if (f_Criteria.FilterCustomerType.HasValue && f_Criteria.CustomerId > 0)
            {
                if (f_Criteria.FilterCustomerType.Equals(FilterCustomerType.IndividualCustomer))
                {
                    shipments = shipments.Where(x => !x.CustomerType.Equals(CustomerType.Company.ToString()) && x.CustomerId == f_Criteria.CustomerId);
                }
                else
                {
                    shipments = shipments.Where(x => x.CustomerType.Equals(CustomerType.Company.ToString()) && x.CustomerId == f_Criteria.CustomerId);
                }
            }

            List<ShipmentDTO> shipmentDto = (from r in shipments
                                             select new ShipmentDTO()
                                             {
                                                 ShipmentId = r.ShipmentId,
                                                 Waybill = r.Waybill,
                                                 CustomerId = r.CustomerId,
                                                 CustomerType = r.CustomerType,
                                                 ActualDateOfArrival = r.ActualDateOfArrival,
                                                 DateCreated = r.DateCreated,
                                                 DateModified = r.DateModified,
                                                 DeliveryOptionId = r.DeliveryOptionId,
                                                 DeliveryOption = new DeliveryOptionDTO
                                                 {
                                                     Code = r.DeliveryOption.Code,
                                                     Description = r.DeliveryOption.Description
                                                 },
                                                 DeliveryTime = r.DeliveryTime,
                                                 DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                 DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),

                                                 DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                 DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                 {
                                                     Code = x.Code,
                                                     Name = x.Name
                                                 }).FirstOrDefault(),

                                                 ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                 PaymentStatus = r.PaymentStatus,
                                                 ReceiverAddress = r.ReceiverAddress,
                                                 ReceiverCity = r.ReceiverCity,
                                                 ReceiverCountry = r.ReceiverCountry,
                                                 ReceiverEmail = r.ReceiverEmail,
                                                 ReceiverName = r.ReceiverName,
                                                 ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                 ReceiverState = r.ReceiverState,
                                                 SealNumber = r.SealNumber,
                                                 UserId = r.UserId,
                                                 Value = r.Value,
                                                 GrandTotal = r.GrandTotal,
                                                 AppliedDiscount = r.AppliedDiscount,
                                                 DiscountValue = r.DiscountValue,
                                                 ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                 ShipmentPickupPrice = r.ShipmentPickupPrice,
                                                 CompanyType = r.CompanyType,
                                                 CustomerCode = r.CustomerCode,
                                                 Description = r.Description,
                                                 SenderAddress = r.SenderAddress,
                                                 SenderState = r.SenderState,
                                                 ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                 DepartureCountryId = r.DepartureCountryId,
                                                 DestinationCountryId = r.DestinationCountryId,
                                                 CurrencyRatio = r.CurrencyRatio,
                                                 IsCashOnDelivery = r.IsCashOnDelivery,
                                                 CashOnDeliveryAmount = r.CashOnDeliveryAmount,
                                                 ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                                 {
                                                     CancelReason = x.CancelReason
                                                 }).FirstOrDefault(),
                                             }).ToList();

            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }

        public IQueryable<Shipment> ShipmentsAsQueryable()
        {
            var shipments = _context.Shipment.AsQueryable();
            return shipments;
        }

        //Basic shipment details
        public Task<ShipmentDTO> GetBasicShipmentDetail(string waybill)
        {
            var shipment = _context.Shipment.Where(x => x.Waybill == waybill);

            ShipmentDTO shipmentDto = (from r in shipment
                                       select new ShipmentDTO
                                       {
                                           ShipmentId = r.ShipmentId,
                                           Waybill = r.Waybill,
                                           CustomerId = r.CustomerId,
                                           CustomerType = r.CustomerType,
                                           DateCreated = r.DateCreated,
                                           DateModified = r.DateModified,
                                           DeliveryOptionId = r.DeliveryOptionId,
                                           DeliveryOption = new DeliveryOptionDTO
                                           {
                                               Code = r.DeliveryOption.Code,
                                               Description = r.DeliveryOption.Description
                                           },
                                           DepartureServiceCentreId = r.DepartureServiceCentreId,
                                           DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                           {
                                               Code = x.Code,
                                               Name = x.Name
                                           }).FirstOrDefault(),
                                           DestinationServiceCentreId = r.DestinationServiceCentreId,
                                           DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                           {
                                               Code = x.Code,
                                               Name = x.Name
                                           }).FirstOrDefault(),
                                           PaymentStatus = r.PaymentStatus,
                                           ReceiverAddress = r.ReceiverAddress,
                                           ReceiverCity = r.ReceiverCity,
                                           ReceiverCountry = r.ReceiverCountry,
                                           ReceiverEmail = r.ReceiverEmail,
                                           ReceiverName = r.ReceiverName,
                                           ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                           ReceiverState = r.ReceiverState,
                                           SealNumber = r.SealNumber,
                                           UserId = r.UserId,
                                           Value = r.Value,
                                           GrandTotal = r.GrandTotal,
                                           AppliedDiscount = r.AppliedDiscount,
                                           DiscountValue = r.DiscountValue,
                                           ShipmentPackagePrice = r.ShipmentPackagePrice,
                                           ShipmentPickupPrice = r.ShipmentPickupPrice,
                                           CompanyType = r.CompanyType,
                                           CustomerCode = r.CustomerCode,
                                           Description = r.Description,
                                           PickupOptions = r.PickupOptions,
                                           IsInternational = r.IsInternational,
                                           Total = r.Total,
                                           CashOnDeliveryAmount = r.CashOnDeliveryAmount,
                                           IsCashOnDelivery = r.IsCashOnDelivery,
                                           SenderAddress = r.SenderAddress,
                                           SenderState = r.SenderState,
                                           ApproximateItemsWeight = r.ApproximateItemsWeight,
                                           DepartureCountryId = r.DepartureCountryId,
                                           DestinationCountryId = r.DestinationCountryId,
                                           CurrencyRatio = r.CurrencyRatio,
                                           ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
                                           {
                                               CancelReason = x.CancelReason
                                           }).FirstOrDefault(),

                                       }).FirstOrDefault();

            return Task.FromResult(shipmentDto);
        }

        //Get Shipment sales
        public Task<List<InvoiceViewDTO>> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            // filter by cancelled shipments
            var shipments = _context.Shipment.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            //filter by service center of the login user
            if (serviceCentreIds.Length > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            //get startDate and endDate
            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            shipments = shipments.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            //filter by country Id
            if (accountFilterCriteria.CountryId > 0)
            {
                shipments = shipments.Where(s => s.DepartureCountryId == accountFilterCriteria.CountryId);
            }

            List<InvoiceViewDTO> result = (from s in shipments
                                           join i in Context.Invoice on s.Waybill equals i.Waybill
                                           join dept in Context.ServiceCentre on s.DepartureServiceCentreId equals dept.ServiceCentreId
                                           join dest in Context.ServiceCentre on s.DestinationServiceCentreId equals dest.ServiceCentreId
                                           join u in Context.Users on s.UserId equals u.Id
                                           select new InvoiceViewDTO
                                           {
                                               Waybill = s.Waybill,
                                               DepartureServiceCentreId = s.DepartureServiceCentreId,
                                               DestinationServiceCentreId = s.DestinationServiceCentreId,
                                               DepartureServiceCentreName = dept.Name,
                                               DestinationServiceCentreName = dest.Name,
                                               Amount = i.Amount,
                                               PaymentMethod = i.PaymentMethod,
                                               PaymentStatus = i.PaymentStatus,
                                               DateCreated = i.DateCreated,
                                               UserName = u.FirstName + " " + u.LastName,
                                               CompanyType = s.CompanyType,
                                               PaymentTypeReference = i.PaymentTypeReference,
                                               ApproximateItemsWeight = s.ApproximateItemsWeight,
                                               Cash = i.Cash,
                                               Transfer = i.Transfer,
                                               Pos = i.Pos
                                           }).ToList();
            var resultDto = result.OrderByDescending(x => x.DateCreated).ToList();
            return Task.FromResult(resultDto);
        }

        public async Task<List<CODShipmentDTO>> GetCODShipments(BaseFilterCriteria baseFilterCriteria)
        {
            try
            {
                var queryDate = baseFilterCriteria.getStartDateAndEndDate();
                var startDate1 = queryDate.Item1;
                var endDate1 = queryDate.Item2;

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", startDate1);
                SqlParameter endDate = new SqlParameter("@EndDate", endDate1);
                SqlParameter serviceCentreId = new SqlParameter("@ServiceCentreId", baseFilterCriteria.ServiceCentreId);

                SqlParameter[] param = new SqlParameter[]
                {
                    serviceCentreId,
                    startDate,
                    endDate
                };

                var result = _context.Database.SqlQuery<CODShipmentDTO>("CODShipments " +
                   "@ServiceCentreId,@StartDate, @EndDate",
                   param).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CargoMagayaShipmentDTO>> GetCargoMagayaShipments(BaseFilterCriteria baseFilterCriteria)
        {
            try
            {
                var queryDate = baseFilterCriteria.getStartDateAndEndDate();
                var startDate1 = queryDate.Item1;
                var endDate1 = queryDate.Item2;

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", startDate1);
                SqlParameter endDate = new SqlParameter("@EndDate", endDate1);

                SqlParameter[] param = new SqlParameter[]
                {
                    startDate,
                    endDate
                };

                var result = _context.Database.SqlQuery<CargoMagayaShipmentDTO>("MagayaShipmentForCargo_Archive " +
                   "@StartDate, @EndDate",
                   param).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get info about Shipment Waybill that belongs to Service Center
        public Task<List<InvoiceViewDTO>> GetWaybillForServiceCentre(string waybill, int[] serviceCentreIds)
        {
            // filter by cancelled shipments
            var shipments = _context.Shipment.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false && s.Waybill == waybill);

            //filter by service center of the login user
            if (serviceCentreIds.Length > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            List<InvoiceViewDTO> result = (from s in shipments
                                           join i in Context.Invoice on s.Waybill equals i.Waybill
                                           join dept in Context.ServiceCentre on s.DepartureServiceCentreId equals dept.ServiceCentreId
                                           join dest in Context.ServiceCentre on s.DestinationServiceCentreId equals dest.ServiceCentreId
                                           join u in Context.Users on s.UserId equals u.Id
                                           select new InvoiceViewDTO
                                           {
                                               Waybill = s.Waybill,
                                               DepartureServiceCentreId = s.DepartureServiceCentreId,
                                               DestinationServiceCentreId = s.DestinationServiceCentreId,
                                               DepartureServiceCentreName = dept.Name,
                                               DestinationServiceCentreName = dest.Name,
                                               Amount = i.Amount,
                                               PaymentMethod = i.PaymentMethod,
                                               PaymentStatus = i.PaymentStatus,
                                               DateCreated = i.DateCreated,
                                               UserName = u.FirstName + " " + u.LastName,
                                               CompanyType = s.CompanyType,
                                               PaymentTypeReference = i.PaymentTypeReference,
                                               ApproximateItemsWeight = s.ApproximateItemsWeight,
                                               Cash = i.Cash,
                                               Transfer = i.Transfer,
                                               Pos = i.Pos
                                           }).ToList();
            var resultDto = result.OrderByDescending(x => x.DateCreated).ToList();
            return Task.FromResult(resultDto);
        }

        //Get Sum  of Monthly 0r Daily Weight of Shipments Created
        public async Task<double> GetSumOfMonthlyOrDailyWeightOfShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType)
        {
            try
            {
                double result = 0.0D;

                DateTime dt = DateTime.Today;
                var beginningDate = dt;
                var endingDate = DateTime.Now;

                if (shipmentReportType == ShipmentReportType.Monthly)
                {
                    beginningDate = new DateTime(dt.Year, dt.Month, 1);
                }
                else if (shipmentReportType == ShipmentReportType.Daily)
                {
                    beginningDate = dt;
                }

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", beginningDate);
                SqlParameter endDate = new SqlParameter("@EndDate", endingDate);
                SqlParameter countryId = new SqlParameter("@CountryId", dashboardFilterCriteria.ActiveCountryId);

                SqlParameter[] param = new SqlParameter[]
                {
                    startDate,
                    endDate,
                    countryId
                };

                var summary = await Context.Database.SqlQuery<double?>("TotalMonthlyWeight " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                if (summary != null)
                {
                    result = (double)summary;
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ShipmentDTO> GetShipment(string waybill)
        {
            var shipment = _context.Shipment.Where(x => x.Waybill == waybill);

            ShipmentDTO shipmentDto = (from r in shipment
                                       select new ShipmentDTO
                                       {
                                           ShipmentId = r.ShipmentId,
                                           Waybill = r.Waybill,
                                           CustomerId = r.CustomerId,
                                           CustomerType = r.CustomerType,
                                           DateCreated = r.DateCreated,
                                           DateModified = r.DateModified,
                                           DeliveryOptionId = r.DeliveryOptionId,
                                           DeliveryOption = new DeliveryOptionDTO
                                           {
                                               Code = r.DeliveryOption.Code,
                                               Description = r.DeliveryOption.Description
                                           },
                                           DepartureServiceCentreId = r.DepartureServiceCentreId,
                                           DestinationServiceCentreId = r.DestinationServiceCentreId,
                                           PaymentStatus = r.PaymentStatus,
                                           ReceiverAddress = r.ReceiverAddress,
                                           ReceiverCity = r.ReceiverCity,
                                           ReceiverCountry = r.ReceiverCountry,
                                           ReceiverEmail = r.ReceiverEmail,
                                           ReceiverName = r.ReceiverName,
                                           ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                           ReceiverState = r.ReceiverState,
                                           SealNumber = r.SealNumber,
                                           UserId = r.UserId,
                                           Value = r.Value,
                                           GrandTotal = r.GrandTotal,
                                           AppliedDiscount = r.AppliedDiscount,
                                           DiscountValue = r.DiscountValue,
                                           ShipmentPackagePrice = r.ShipmentPackagePrice,
                                           ShipmentPickupPrice = r.ShipmentPickupPrice,
                                           CompanyType = r.CompanyType,
                                           CustomerCode = r.CustomerCode,
                                           Description = r.Description,
                                           PickupOptions = r.PickupOptions,
                                           IsInternational = r.IsInternational,
                                           Total = r.Total,
                                           CashOnDeliveryAmount = r.CashOnDeliveryAmount,
                                           IsCashOnDelivery = r.IsCashOnDelivery,
                                           SenderAddress = r.SenderAddress,
                                           SenderState = r.SenderState,
                                           ApproximateItemsWeight = r.ApproximateItemsWeight,
                                           DepartureCountryId = r.DepartureCountryId,
                                           DestinationCountryId = r.DestinationCountryId,
                                           CurrencyRatio = r.CurrencyRatio,
                                           ActualAmountCollected = r.ActualAmountCollected,
                                           DeclarationOfValueCheck = r.DeclarationOfValueCheck,
                                           DeliveryTime = r.DeliveryTime,
                                           DepositStatus = (int)r.DepositStatus,
                                           IsCancelled = r.IsCancelled,
                                           Insurance = r.Insurance,
                                           Vat = r.Vat,
                                           ActualDateOfArrival = r.ActualDateOfArrival,
                                           ExpectedAmountToCollect = r.ExpectedAmountToCollect,
                                           ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                           InternationalShipmentType = r.InternationalShipmentType,
                                           InvoiceDiscountValue_display = r.InvoiceDiscountValue_display,
                                           IsClassShipment = r.IsClassShipment,
                                           IsdeclaredVal = r.IsdeclaredVal,
                                           IsFromMobile = r.IsFromMobile,
                                           isInternalShipment = r.isInternalShipment,
                                           offInvoiceDiscountvalue_display = r.offInvoiceDiscountvalue_display,
                                           PaymentMethod = r.PaymentMethod,
                                           vatvalue_display = r.vatvalue_display,
                                           ReprintCounterStatus = r.ReprintCounterStatus,
                                           ShipmentItems = Context.ShipmentItem.Where(i => i.ShipmentId == r.ShipmentId).Select(x => new ShipmentItemDTO
                                           {
                                               ShipmentId = x.ShipmentId,
                                               DateCreated = x.DateCreated,
                                               DateModified = x.DateModified,
                                               Description = x.Description,
                                               Description_s = x.Description_s,
                                               Height = x.Height,
                                               IsVolumetric = x.IsVolumetric,
                                               Length = x.Length,
                                               Nature = x.Nature,
                                               PackageQuantity = x.PackageQuantity,
                                               Price = x.Price,
                                               Quantity = x.Quantity,
                                               SerialNumber = x.SerialNumber,
                                               ShipmentItemId = x.ShipmentItemId,
                                               ShipmentPackagePriceId = x.ShipmentPackagePriceId,
                                               ShipmentType = x.ShipmentType,
                                               Weight = x.Weight,
                                               Width = x.Width
                                           }).ToList(),
                                           Invoice = Context.Invoice.Where(x => x.Waybill == r.Waybill).Select(i => new InvoiceDTO
                                           {
                                               Amount = i.Amount,
                                               Waybill = i.Waybill,
                                               Cash = i.Cash,
                                               CountryId = i.CountryId,
                                               DateCreated = i.DateCreated,
                                               DateModified = i.DateModified,
                                               DueDate = i.DueDate,
                                               InvoiceId = i.InvoiceId,
                                               InvoiceNo = i.InvoiceNo,
                                               IsShipmentCollected = i.IsShipmentCollected,
                                               IsInternational = i.IsInternational,
                                               PaymentDate = i.PaymentDate,
                                               PaymentMethod = i.PaymentMethod,
                                               PaymentStatus = i.PaymentStatus,
                                               PaymentTypeReference = i.PaymentTypeReference,
                                               Pos = i.Pos,
                                               ServiceCentreId = i.ServiceCentreId,
                                               Transfer = i.Transfer
                                           }).FirstOrDefault(),
                                           WalletNumber = Context.Wallets.Where(w => w.CustomerCode == r.CustomerCode).Select(x => x.WalletNumber).FirstOrDefault()
                                       }).FirstOrDefault();

            return Task.FromResult(shipmentDto);
        }
    }

    public class IntlShipmentRequestRepository : Repository<IntlShipmentRequest, GIGLSContext>, IIntlShipmentRequestRepository
    {
        private GIGLSContext _context;
        public IntlShipmentRequestRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlTransactionShipmentRequest(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                var shipmentRequest = _context.IntlShipmentRequest
                    .Join(
                        _context.IntlShipmentRequestItem,
                        a => a.IntlShipmentRequestId,
                        b => b.IntlShipmentRequestId,
                        (a, b) => new
                        {
                            IntlShipmentRequestId = a.IntlShipmentRequestId,
                            RequestNumber = a.RequestNumber,
                            CustomerFirstName = a.CustomerFirstName,
                            CustomerLastName = a.CustomerLastName,
                            CustomerId = a.CustomerId,
                            CustomerType = a.CustomerType,
                            CustomerCountryId = a.CustomerCountryId,
                            CustomerAddress = a.CustomerAddress,
                            CustomerEmail = a.CustomerEmail,
                            CustomerPhoneNumber = a.CustomerPhoneNumber,
                            CustomerCity = a.CustomerCity,
                            CustomerState = a.CustomerState,
                            DateCreated = a.DateCreated,
                            DateModified = a.DateModified,
                            PickupOptions = a.PickupOptions,
                            DestinationServiceCentreId = a.DestinationServiceCentreId,
                            DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == a.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                            {
                                Code = x.Code,
                                Name = x.Name
                            }).FirstOrDefault(),
                            IntlShipmentRequestItemId = b.IntlShipmentRequestItemId,
                            Description = b.Description,
                            ItemName = b.ItemName,
                            TrackingId = b.TrackingId,
                            storeName = b.storeName,
                            ShipmentType = b.ShipmentType,
                            Weight = b.Weight,
                            Nature = b.Nature,
                            Price = b.Price,
                            Quantity = b.Quantity,
                            SerialNumber = b.SerialNumber,
                            IsVolumetric = b.IsVolumetric,
                            Length = b.Length,
                            Width = b.Width,
                            Height = b.Height,
                            ReceiverAddress = a.ReceiverAddress,
                            ReceiverCity = a.ReceiverCity,
                            ReceiverCountry = a.ReceiverCountry,
                            ReceiverEmail = a.ReceiverEmail,
                            ReceiverName = a.ReceiverName,
                            ReceiverPhoneNumber = a.ReceiverPhoneNumber,
                            ReceiverState = a.ReceiverState,
                            UserId = a.UserId,
                            Value = a.Value,
                            GrandTotal = a.GrandTotal,
                            SenderAddress = a.SenderAddress,
                            SenderState = a.SenderState,
                            ApproximateItemsWeight = a.ApproximateItemsWeight,
                            DestinationCountryId = a.DestinationCountryId,
                            IsProcessed = a.IsProcessed,
                            ItemSenderfullName = b.ItemSenderfullName,
                            ItemValue = b.ItemValue,
                            Consolidated = a.Consolidated,
                            Received = b.Received,
                            ReceivedBy = b.ReceivedBy,
                            ItemCount = b.ItemCount,
                            RequestProcessingCountryId = a.RequestProcessingCountryId,


                        }
                    ).Where(a => a.IsProcessed == false).ToList();


                var count = 0;
                List<IntlShipmentDTO> intlShipmentDTO = new List<IntlShipmentDTO>();

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(filterValue))
                {
                    intlShipmentDTO = (from a in shipmentRequest
                                       select new IntlShipmentDTO()
                                       {
                                           IntlShipmentRequestId = a.IntlShipmentRequestId,
                                           RequestNumber = a.RequestNumber,
                                           CustomerFirstName = a.CustomerFirstName,
                                           CustomerLastName = a.CustomerLastName,
                                           CustomerId = a.CustomerId,
                                           CustomerType = a.CustomerType,
                                           CustomerCountryId = a.CustomerCountryId,
                                           CustomerAddress = a.CustomerAddress,
                                           CustomerEmail = a.CustomerEmail,
                                           CustomerPhoneNumber = a.CustomerPhoneNumber,
                                           CustomerCity = a.CustomerCity,
                                           CustomerState = a.CustomerState,
                                           DateCreated = a.DateCreated,
                                           DateModified = a.DateModified,
                                           PickupOptions = a.PickupOptions,
                                           DestinationServiceCentreId = a.DestinationServiceCentreId,
                                           DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == a.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                           {
                                               Code = x.Code,
                                               Name = x.Name
                                           }).FirstOrDefault(),
                                           IntlShipmentRequestItemId = a.IntlShipmentRequestItemId,
                                           Description = a.Description,
                                           ItemName = a.ItemName,
                                           TrackingId = a.TrackingId,
                                           storeName = a.storeName,
                                           ShipmentType = a.ShipmentType,
                                           Weight = a.Weight,
                                           Nature = a.Nature,
                                           Price = a.Price,
                                           Quantity = a.Quantity,
                                           SerialNumber = a.SerialNumber,
                                           IsVolumetric = a.IsVolumetric,
                                           Length = a.Length,
                                           Width = a.Width,
                                           Height = a.Height,
                                           ReceiverAddress = a.ReceiverAddress,
                                           ReceiverCity = a.ReceiverCity,
                                           ReceiverCountry = a.ReceiverCountry,
                                           ReceiverEmail = a.ReceiverEmail,
                                           ReceiverName = a.ReceiverName,
                                           ReceiverPhoneNumber = a.ReceiverPhoneNumber,
                                           ReceiverState = a.ReceiverState,
                                           UserId = a.UserId,
                                           Value = a.Value,
                                           GrandTotal = a.GrandTotal,
                                           SenderAddress = a.SenderAddress,
                                           SenderState = a.SenderState,
                                           ApproximateItemsWeight = a.ApproximateItemsWeight,
                                           DestinationCountryId = a.DestinationCountryId,
                                           IsProcessed = a.IsProcessed,
                                           ItemSenderfullName = a.ItemSenderfullName,
                                           ItemValue = a.ItemValue,
                                           RequestProcessingCountryId = a.RequestProcessingCountryId,

                                       }).Where(b => b.IsProcessed == false).OrderByDescending(x => x.DateCreated).Take(10).ToList();

                    count = intlShipmentDTO.Count();
                    return new Tuple<List<IntlShipmentDTO>, int>(intlShipmentDTO, count);
                }

                intlShipmentDTO = (from a in shipmentRequest
                                   select new IntlShipmentDTO()
                                   {
                                       IntlShipmentRequestId = a.IntlShipmentRequestId,
                                       RequestNumber = a.RequestNumber,
                                       CustomerFirstName = a.CustomerFirstName,
                                       CustomerLastName = a.CustomerLastName,
                                       CustomerId = a.CustomerId,
                                       CustomerType = a.CustomerType,
                                       CustomerCountryId = a.CustomerCountryId,
                                       CustomerAddress = a.CustomerAddress,
                                       CustomerEmail = a.CustomerEmail,
                                       CustomerPhoneNumber = a.CustomerPhoneNumber,
                                       CustomerCity = a.CustomerCity,
                                       CustomerState = a.CustomerState,
                                       DateCreated = a.DateCreated,
                                       DateModified = a.DateModified,
                                       PickupOptions = a.PickupOptions,
                                       DestinationServiceCentreId = a.DestinationServiceCentreId,
                                       DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == a.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                       {
                                           Code = x.Code,
                                           Name = x.Name
                                       }).FirstOrDefault(),
                                       IntlShipmentRequestItemId = a.IntlShipmentRequestItemId,
                                       Description = a.Description,
                                       ItemName = a.ItemName,
                                       TrackingId = a.TrackingId,
                                       storeName = a.storeName,
                                       ShipmentType = a.ShipmentType,
                                       Weight = a.Weight,
                                       Nature = a.Nature,
                                       Price = a.Price,
                                       Quantity = a.Quantity,
                                       SerialNumber = a.SerialNumber,
                                       IsVolumetric = a.IsVolumetric,
                                       Length = a.Length,
                                       Width = a.Width,
                                       Height = a.Height,
                                       ReceiverAddress = a.ReceiverAddress,
                                       ReceiverCity = a.ReceiverCity,
                                       ReceiverCountry = a.ReceiverCountry,
                                       ReceiverEmail = a.ReceiverEmail,
                                       ReceiverName = a.ReceiverName,
                                       ReceiverPhoneNumber = a.ReceiverPhoneNumber,
                                       ReceiverState = a.ReceiverState,
                                       UserId = a.UserId,
                                       Value = a.Value,
                                       GrandTotal = a.GrandTotal,
                                       SenderAddress = a.SenderAddress,
                                       SenderState = a.SenderState,
                                       ApproximateItemsWeight = a.ApproximateItemsWeight,
                                       DestinationCountryId = a.DestinationCountryId,
                                       IsProcessed = a.IsProcessed,
                                       ItemSenderfullName = a.ItemSenderfullName,
                                       ItemValue = a.ItemValue,
                                       RequestProcessingCountryId = a.RequestProcessingCountryId,

                                   }).Where(b => b.IsProcessed == false).Where(s => (s.RequestNumber == filterValue || s.GrandTotal.ToString() == filterValue || s.DateCreated.ToString() == filterValue
                                   || s.CustomerFirstName == filterValue || s.CustomerLastName == filterValue || s.ItemSenderfullName == filterValue || s.storeName == filterValue )).ToList();

                //filter
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    intlShipmentDTO = intlShipmentDTO.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        intlShipmentDTO = intlShipmentDTO.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                    else
                    {
                        intlShipmentDTO = intlShipmentDTO.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                }

                intlShipmentDTO = intlShipmentDTO.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                count = intlShipmentDTO.Count();
                if (filterOptionsDto.CountryId != null && filterOptionsDto.CountryId > 0 && count > 0)
                {
                    var currentUser = _context.Users.Where(x => x.Id == filterOptionsDto.UserId).FirstOrDefault();
                    if (currentUser != null && currentUser.IsMagaya)
                    {
                        intlShipmentDTO = intlShipmentDTO.Where(x => x.RequestProcessingCountryId == filterOptionsDto.CountryId || x.RequestProcessingCountryId == 0).ToList();
                    }
                    else
                    {
                        intlShipmentDTO = intlShipmentDTO.Where(x => x.RequestProcessingCountryId == filterOptionsDto.CountryId).ToList();
                    }
                }
                return new Tuple<List<IntlShipmentDTO>, int>(intlShipmentDTO, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Tuple<List<IntlShipmentRequestDTO>, int>> GetIntlTransactionShipmentRequest2(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                //filter by service center
                var shipmentRequest = _context.IntlShipmentRequest.AsQueryable();
                //var shipment = _context.IntlShipmentRequest.Where(x => x.IntlShipmentRequestId == shipmentRequestId, "ShipmentRequestItems");

                if (serviceCentreIds.Length > 0)
                {
                    shipmentRequest = _context.IntlShipmentRequest.Where(s => s.IsProcessed == false);
                }
               
                var count = 0;
                List<IntlShipmentRequestDTO> intlShipmentRequestDTO = new List<IntlShipmentRequestDTO>();

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(filterValue))
                {
                    intlShipmentRequestDTO = (from r in shipmentRequest
                                              select new IntlShipmentRequestDTO()
                                              {
                                                  IntlShipmentRequestId = r.IntlShipmentRequestId,
                                                  RequestNumber = r.RequestNumber,
                                                  CustomerFirstName = r.CustomerFirstName,
                                                  CustomerLastName = r.CustomerLastName,
                                                  CustomerId = r.CustomerId,
                                                  CustomerType = r.CustomerType,
                                                  CustomerCountryId = r.CustomerCountryId,
                                                  CustomerAddress = r.CustomerAddress,
                                                  CustomerEmail = r.CustomerEmail,
                                                  CustomerPhoneNumber = r.CustomerPhoneNumber,
                                                  CustomerCity = r.CustomerCity,
                                                  CustomerState = r.CustomerState,
                                                  DateCreated = r.DateCreated,
                                                  DateModified = r.DateModified,
                                                  PickupOptions = r.PickupOptions,
                                                  DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                  DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                  {
                                                      Code = x.Code,
                                                      Name = x.Name
                                                  }).FirstOrDefault(),
                                                  ShipmentRequestItems = Context.IntlShipmentRequestItem.Where(s => s.IntlShipmentRequestId == r.IntlShipmentRequestId).Select(x => new IntlShipmentRequestItemDTO
                                                  {
                                                      Description = x.Description,
                                                      ItemName = x.ItemName,
                                                      TrackingId = x.TrackingId,
                                                      storeName = x.storeName,
                                                      ShipmentType = x.ShipmentType,
                                                      Weight = x.Weight,
                                                      Nature = x.Nature,
                                                      Price = x.Price,
                                                      Quantity = x.Quantity,
                                                      SerialNumber = x.SerialNumber,
                                                      IsVolumetric = x.IsVolumetric,
                                                      Length = x.Length,
                                                      Width = x.Width,
                                                      Height = x.Height,
                                                      ItemSenderfullName = x.ItemSenderfullName,
                                                      Received = x.Received,
                                                      ReceivedBy = x.ReceivedBy,
                                                      ItemCount = x.ItemCount

                                                  }).ToList(),
                                                  ReceiverAddress = r.ReceiverAddress,
                                                  ReceiverCity = r.ReceiverCity,
                                                  ReceiverCountry = r.ReceiverCountry,
                                                  ReceiverEmail = r.ReceiverEmail,
                                                  ReceiverName = r.ReceiverName,
                                                  ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                  ReceiverState = r.ReceiverState,
                                                  UserId = r.UserId,
                                                  Value = r.Value,
                                                  GrandTotal = r.GrandTotal,
                                                  SenderAddress = r.SenderAddress,
                                                  SenderState = r.SenderState,
                                                  ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                  DestinationCountryId = r.DestinationCountryId,
                                                  IsProcessed = r.IsProcessed,
                                                  ItemSenderfullName = r.ItemSenderfullName,
                                                  Consolidated = r.Consolidated,
                                                  RequestProcessingCountryId = r.RequestProcessingCountryId,

                                              }).Where(b => b.IsProcessed == false).OrderByDescending(x => x.DateCreated).Take(10).ToList();

                    count = intlShipmentRequestDTO.Count();
                    var retVal = new Tuple<List<IntlShipmentRequestDTO>, int>(intlShipmentRequestDTO, count);

                    return Task.FromResult(retVal);
                }

                intlShipmentRequestDTO = (from r in shipmentRequest
                                          select new IntlShipmentRequestDTO()
                                          {
                                              IntlShipmentRequestId = r.IntlShipmentRequestId,
                                              RequestNumber = r.RequestNumber,
                                              CustomerId = r.CustomerId,
                                              CustomerType = r.CustomerType,
                                              DateCreated = r.DateCreated,
                                              DateModified = r.DateModified,
                                              PickupOptions = r.PickupOptions,
                                              DestinationServiceCentreId = r.DestinationServiceCentreId,
                                              DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                              {
                                                  Code = x.Code,
                                                  Name = x.Name
                                              }).FirstOrDefault(),
                                              ReceiverAddress = r.ReceiverAddress,
                                              ReceiverCity = r.ReceiverCity,
                                              ReceiverCountry = r.ReceiverCountry,
                                              ReceiverEmail = r.ReceiverEmail,
                                              ReceiverName = r.ReceiverName,
                                              ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                              UserId = r.UserId,
                                              Value = r.Value,
                                              GrandTotal = r.GrandTotal,
                                              SenderAddress = r.SenderAddress,
                                              SenderState = r.SenderState,
                                              ApproximateItemsWeight = r.ApproximateItemsWeight,
                                              DestinationCountryId = r.DestinationCountryId,
                                              IsProcessed = r.IsProcessed,
                                              ItemSenderfullName = r.ItemSenderfullName,
                                              Consolidated = r.Consolidated,
                                              RequestProcessingCountryId = r.RequestProcessingCountryId,

                                          }).Where(b => b.IsProcessed == false).Where(s => (s.RequestNumber == filterValue || s.GrandTotal.ToString() == filterValue || s.DateCreated.ToString() == filterValue || s.ItemSenderfullName == filterValue)).ToList();

                //filter
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    intlShipmentRequestDTO = intlShipmentRequestDTO.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        intlShipmentRequestDTO = intlShipmentRequestDTO.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                    else
                    {
                        intlShipmentRequestDTO = intlShipmentRequestDTO.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                    }
                }

                intlShipmentRequestDTO = intlShipmentRequestDTO.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();
                count = intlShipmentRequestDTO.Count();
                if (filterOptionsDto.CountryId != null && filterOptionsDto.CountryId > 0 && count > 0)
                {
                    var currentUser = _context.Users.Where(x => x.Id == filterOptionsDto.UserId).FirstOrDefault();
                    if (currentUser != null && currentUser.IsMagaya)
                    {
                        intlShipmentRequestDTO = intlShipmentRequestDTO.Where(x => x.RequestProcessingCountryId == filterOptionsDto.CountryId || x.RequestProcessingCountryId == 0).ToList();
                    }
                    else
                    {
                        intlShipmentRequestDTO = intlShipmentRequestDTO.Where(x => x.RequestProcessingCountryId == filterOptionsDto.CountryId).ToList();
                    }
                }
                var retValue = new Tuple<List<IntlShipmentRequestDTO>, int>(intlShipmentRequestDTO, count);
                return Task.FromResult(retValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<IntlShipmentRequestDTO>> GetShipments(int[] serviceCentreIds)
        {
            //filter by service center
            var shipmentRequest = _context.IntlShipmentRequest.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                shipmentRequest = _context.IntlShipmentRequest.Where(s => s.IsProcessed == true);
            }

            List<IntlShipmentRequestDTO> IntlShipmentRequestDTO = (from r in shipmentRequest
                                                                   select new IntlShipmentRequestDTO()
                                                                   {
                                                                       IntlShipmentRequestId = r.IntlShipmentRequestId,
                                                                       RequestNumber = r.RequestNumber,
                                                                       CustomerId = r.CustomerId,
                                                                       CustomerType = r.CustomerType,
                                                                       DateCreated = r.DateCreated,
                                                                       DateModified = r.DateModified,
                                                                       PickupOptions = r.PickupOptions,
                                                                       DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                                       DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                                       {
                                                                           Code = x.Code,
                                                                           Name = x.Name
                                                                       }).FirstOrDefault(),
                                                                       ReceiverAddress = r.ReceiverAddress,
                                                                       ReceiverCity = r.ReceiverCity,
                                                                       ReceiverCountry = r.ReceiverCountry,
                                                                       ReceiverEmail = r.ReceiverEmail,
                                                                       ReceiverName = r.ReceiverName,
                                                                       ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                                       UserId = r.UserId,
                                                                       Value = r.Value,
                                                                       GrandTotal = r.GrandTotal,
                                                                       SenderAddress = r.SenderAddress,
                                                                       SenderState = r.SenderState,
                                                                       ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                                       DestinationCountryId = r.DestinationCountryId,
                                                                       RequestProcessingCountryId = r.RequestProcessingCountryId,
                                                                   }).ToList();

            return Task.FromResult(IntlShipmentRequestDTO.ToList());
        }

        public async Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlTransactionShipmentRequest(DateFilterCriteria dateFilterCriteria)
        {
            try
            {
                var count = 0;
                List<IntlShipmentDTO> intlShipmentDTO = new List<IntlShipmentDTO>();

                if (!String.IsNullOrEmpty(dateFilterCriteria.FilterValue))
                {
                    intlShipmentDTO = _context.IntlShipmentRequest
                  .Join(
                      _context.IntlShipmentRequestItem,
                      a => a.IntlShipmentRequestId,
                      b => b.IntlShipmentRequestId,
                      (a, b) => new IntlShipmentDTO
                      {
                          IntlShipmentRequestId = a.IntlShipmentRequestId,
                          RequestNumber = a.RequestNumber,
                          CustomerFirstName = a.CustomerFirstName,
                          CustomerLastName = a.CustomerLastName,
                          CustomerId = a.CustomerId,
                          CustomerType = a.CustomerType,
                          CustomerCountryId = a.CustomerCountryId,
                          CustomerAddress = a.CustomerAddress,
                          CustomerEmail = a.CustomerEmail,
                          CustomerPhoneNumber = a.CustomerPhoneNumber,
                          CustomerCity = a.CustomerCity,
                          CustomerState = a.CustomerState,
                          DateCreated = a.DateCreated,
                          DateModified = a.DateModified,
                          PickupOptions = a.PickupOptions,
                          DestinationServiceCentreId = a.DestinationServiceCentreId,
                          DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == a.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                          {
                              Code = x.Code,
                              Name = x.Name
                          }).FirstOrDefault(),
                          IntlShipmentRequestItemId = b.IntlShipmentRequestItemId,
                          Description = b.Description,
                          ItemName = b.ItemName,
                          TrackingId = b.TrackingId,
                          storeName = b.storeName,
                          ShipmentType = b.ShipmentType,
                          Weight = b.Weight,
                          Nature = b.Nature,
                          Price = b.Price,
                          Quantity = b.Quantity,
                          SerialNumber = b.SerialNumber,
                          IsVolumetric = b.IsVolumetric,
                          Length = b.Length,
                          Width = b.Width,
                          Height = b.Height,
                          ReceiverAddress = a.ReceiverAddress,
                          ReceiverCity = a.ReceiverCity,
                          ReceiverCountry = a.ReceiverCountry,
                          ReceiverEmail = a.ReceiverEmail,
                          ReceiverName = a.ReceiverName,
                          ReceiverPhoneNumber = a.ReceiverPhoneNumber,
                          ReceiverState = a.ReceiverState,
                          UserId = a.UserId,
                          Value = a.Value,
                          GrandTotal = a.GrandTotal,
                          SenderAddress = a.SenderAddress,
                          SenderState = a.SenderState,
                          ApproximateItemsWeight = a.ApproximateItemsWeight,
                          DestinationCountryId = a.DestinationCountryId,
                          IsProcessed = a.IsProcessed,
                          ItemSenderfullName = b.ItemSenderfullName,
                          ItemValue = b.ItemValue,
                          Consolidated = a.Consolidated,
                          Received = b.Received,
                          ReceivedBy = b.ReceivedBy,
                          ItemCount = b.ItemCount,
                          RequestProcessingCountryId = a.RequestProcessingCountryId,

                      }
                  ).Where(b => b.IsProcessed == false).Where(s => (s.RequestNumber == dateFilterCriteria.FilterValue
                                       || s.TrackingId == dateFilterCriteria.FilterValue || s.CustomerEmail == dateFilterCriteria.FilterValue
                                       || s.CustomerFirstName == dateFilterCriteria.FilterValue || s.CustomerLastName == dateFilterCriteria.FilterValue || s.storeName == dateFilterCriteria.FilterValue || s.ItemSenderfullName == dateFilterCriteria.FilterValue)).OrderByDescending(x => x.DateCreated).ToList();
                }
                else
                {
                    //get startDate and endDate
                    var queryDate = dateFilterCriteria.getStartDateAndEndDate();
                    var startDate = queryDate.Item1;
                    var endDate = queryDate.Item2;

                    if (dateFilterCriteria.StartDate == null && dateFilterCriteria.EndDate == null)
                    {
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                    }

                    intlShipmentDTO = _context.IntlShipmentRequest
                   .Join(
                       _context.IntlShipmentRequestItem,
                       a => a.IntlShipmentRequestId,
                       b => b.IntlShipmentRequestId,
                       (a, b) => new IntlShipmentDTO
                       {
                           IntlShipmentRequestId = a.IntlShipmentRequestId,
                           RequestNumber = a.RequestNumber,
                           CustomerFirstName = a.CustomerFirstName,
                           CustomerLastName = a.CustomerLastName,
                           CustomerId = a.CustomerId,
                           CustomerType = a.CustomerType,
                           CustomerCountryId = a.CustomerCountryId,
                           CustomerAddress = a.CustomerAddress,
                           CustomerEmail = a.CustomerEmail,
                           CustomerPhoneNumber = a.CustomerPhoneNumber,
                           CustomerCity = a.CustomerCity,
                           CustomerState = a.CustomerState,
                           DateCreated = a.DateCreated,
                           DateModified = a.DateModified,
                           PickupOptions = a.PickupOptions,
                           DestinationServiceCentreId = a.DestinationServiceCentreId,
                           DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == a.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                           {
                               Code = x.Code,
                               Name = x.Name
                           }).FirstOrDefault(),
                           IntlShipmentRequestItemId = b.IntlShipmentRequestItemId,
                           Description = b.Description,
                           ItemName = b.ItemName,
                           TrackingId = b.TrackingId,
                           storeName = b.storeName,
                           ShipmentType = b.ShipmentType,
                           Weight = b.Weight,
                           Nature = b.Nature,
                           Price = b.Price,
                           Quantity = b.Quantity,
                           SerialNumber = b.SerialNumber,
                           IsVolumetric = b.IsVolumetric,
                           Length = b.Length,
                           Width = b.Width,
                           Height = b.Height,
                           ReceiverAddress = a.ReceiverAddress,
                           ReceiverCity = a.ReceiverCity,
                           ReceiverCountry = a.ReceiverCountry,
                           ReceiverEmail = a.ReceiverEmail,
                           ReceiverName = a.ReceiverName,
                           ReceiverPhoneNumber = a.ReceiverPhoneNumber,
                           ReceiverState = a.ReceiverState,
                           UserId = a.UserId,
                           Value = a.Value,
                           GrandTotal = a.GrandTotal,
                           SenderAddress = a.SenderAddress,
                           SenderState = a.SenderState,
                           ApproximateItemsWeight = a.ApproximateItemsWeight,
                           DestinationCountryId = a.DestinationCountryId,
                           IsProcessed = a.IsProcessed,
                           ItemSenderfullName = b.ItemSenderfullName,
                           ItemValue = b.ItemValue,
                           Consolidated = a.Consolidated,
                           Received = b.Received,
                           ReceivedBy = b.ReceivedBy,
                           ItemCount = b.ItemCount,
                           RequestProcessingCountryId = a.RequestProcessingCountryId,

                       }
                   ).Where(a => a.IsProcessed == false && a.DateCreated >= startDate && a.DateCreated < endDate).OrderByDescending(x => x.DateCreated).ToList();
                }

                count = intlShipmentDTO.Count();
                if (dateFilterCriteria.CountryId != null && dateFilterCriteria.CountryId > 0 && count > 0)
                {
                    var currentUser = _context.Users.Where(x => x.Id == dateFilterCriteria.UserId).FirstOrDefault();
                    if (currentUser != null && currentUser.IsMagaya)
                    {
                        intlShipmentDTO = intlShipmentDTO.Where(x => x.RequestProcessingCountryId == dateFilterCriteria.CountryId || x.RequestProcessingCountryId == 0).ToList();
                    }
                    else
                    {
                        intlShipmentDTO = intlShipmentDTO.Where(x => x.RequestProcessingCountryId == dateFilterCriteria.CountryId).ToList();
                    }
                }
                return new Tuple<List<IntlShipmentDTO>, int>(intlShipmentDTO, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<IntlShipmentRequestDTO>> GetIntlShipmentRequestsForUser(ShipmentCollectionFilterCriteria filterCriteria, string currentUserId)
        {
            //get startDate and endDate
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var requests = _context.IntlShipmentRequest.AsQueryable().Where(x => x.UserId == currentUserId);

            if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
            {
                //Last 20 days
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-20);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            requests = requests.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate).OrderByDescending(s => s.DateCreated);

            List<IntlShipmentRequestDTO> requestsDTO = (from r in requests
                                                select new IntlShipmentRequestDTO()
                                                {
                                                    IntlShipmentRequestId = r.IntlShipmentRequestId,
                                                    RequestNumber = r.RequestNumber,
                                                    CustomerFirstName = r.CustomerFirstName,
                                                    CustomerLastName = r.CustomerLastName,
                                                    CustomerAddress = r.CustomerAddress,
                                                    CustomerEmail = r.CustomerEmail,
                                                    CustomerPhoneNumber = r.CustomerPhoneNumber,
                                                    CustomerCity = r.CustomerCity,
                                                    CustomerState = r.CustomerState,
                                                    DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                    DestinationServiceCentre = _context.ServiceCentre.Where(s => s.ServiceCentreId == r.DestinationServiceCentreId)
                                                                                .Select(x => new ServiceCentreDTO
                                                                                {
                                                                                    Name = x.Name,
                                                                                    FormattedServiceCentreName = x.FormattedServiceCentreName
                                                                                }).FirstOrDefault(),
                                                    DestinationCountryId = r.DestinationCountryId,
                                                    DateCreated = r.DateCreated,
                                                    DateModified = r.DateModified,
                                                    ReceiverAddress = r.ReceiverAddress,
                                                    ReceiverCity = r.ReceiverCity,
                                                    ReceiverName = r.ReceiverName,
                                                    ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                    ReceiverEmail = r.ReceiverEmail,
                                                    ReceiverState = r.ReceiverState,
                                                    ApproximateItemsWeight = r.ApproximateItemsWeight,
                                                    GrandTotal = r.GrandTotal,
                                                    Total = r.Total,
                                                    PaymentMethod = r.PaymentMethod,
                                                    SenderAddress = r.SenderAddress,
                                                    SenderState = r.SenderState,
                                                    Value = r.Value,
                                                    IsProcessed = r.IsProcessed,
                                                    PickupOptions = r.PickupOptions,
                                                    Consolidated = r.Consolidated,
                                                    RequestProcessingCountryId = r.RequestProcessingCountryId,
                                                    ShipmentRequestItems = _context.IntlShipmentRequestItem.Where(s => s.IntlShipmentRequestId == r.IntlShipmentRequestId)
                                                                        .Select(x => new IntlShipmentRequestItemDTO
                                                                        {
                                                                            IntlShipmentRequestItemId = x.IntlShipmentRequestItemId,
                                                                            Description = x.Description,
                                                                            ItemName = x.ItemName,
                                                                            TrackingId = x.TrackingId,
                                                                            storeName = x.storeName,
                                                                            Nature = x.Nature,
                                                                            Price = x.Price,
                                                                            ShipmentType = x.ShipmentType,
                                                                            Weight = x.Weight,
                                                                            Quantity = x.Quantity,
                                                                            IsVolumetric = x.IsVolumetric,
                                                                            Length = x.Length,
                                                                            Width = x.Width,
                                                                            Height = x.Height,
                                                                            RequiresInsurance = x.RequiresInsurance,
                                                                            IntlShipmentRequestId = x.IntlShipmentRequestId,
                                                                            SerialNumber = x.SerialNumber ,
                                                                            ItemValue = x.ItemValue,
                                                                            ItemSenderfullName = x.ItemSenderfullName,
                                                                            Received = x.Received,
                                                                            ReceivedBy = x.ReceivedBy,
                                                                            ItemCount = x.ItemCount
                                                                        }).ToList()
                                                }).ToList();

            return Task.FromResult(requestsDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

    }
}