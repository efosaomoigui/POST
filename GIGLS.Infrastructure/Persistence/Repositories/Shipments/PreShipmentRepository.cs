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

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PreShipmentRepository : Repository<PreShipment, GIGLSContext>, IPreShipmentRepository
    {
        private GIGLSContext _context;
        public PreShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Tuple<Task<List<PreShipmentDTO>>, int> GetPreShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                //filter by service center
                var preShipment = _context.PreShipment.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    preShipment = _context.PreShipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
                }
                ////

                //filter by cancelled preshipments
                preShipment = preShipment.Where(s => s.IsCancelled == false);

                //filter by Local or International PreShipment
                if (filterOptionsDto.IsInternational != null)
                {
                    preShipment = preShipment.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                }


                var count = preShipment.Count();
                //shipment = shipment.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count);


                List<PreShipmentDTO> preShipmentDto = new List<PreShipmentDTO>();
                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(filterValue))
                {
                    preShipmentDto = (from r in preShipment
                                   select new PreShipmentDTO()
                                   {
                                       PreShipmentId = r.PreShipmentId,
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
                                       CompanyType = r.CompanyType,
                                       CustomerCode = r.CustomerCode,
                                       Description = r.Description,
                                       //Invoice = Context.Invoice.Where(c => c.Waybill == r.Waybill).Select(x => new InvoiceDTO
                                       //{
                                       //    InvoiceId = x.InvoiceId,
                                       //    InvoiceNo = x.InvoiceNo,
                                       //    Amount = x.Amount,
                                       //    PaymentStatus = x.PaymentStatus,
                                       //    PaymentMethod = x.PaymentMethod,
                                       //    PaymentDate = x.PaymentDate,
                                       //    Waybill = x.Waybill,
                                       //    DueDate = x.DueDate,
                                       //    IsInternational = x.IsInternational
                                       //}).FirstOrDefault()
                                       //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()
                                   }).OrderByDescending(x => x.DateCreated).Take(20).ToList();

                    return new Tuple<Task<List<PreShipmentDTO>>, int>(Task.FromResult(preShipmentDto.ToList()), count);
                }

                preShipmentDto = (from r in preShipment
                               select new PreShipmentDTO()
                               {
                                   PreShipmentId = r.PreShipmentId,
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
                                   CompanyType = r.CompanyType,
                                   CustomerCode = r.CustomerCode,
                                   Description = r.Description,
                                   //Invoice = Context.Invoice.Where(c => c.Waybill == r.Waybill).Select(x => new InvoiceDTO
                                   //{
                                   //    InvoiceId = x.InvoiceId,
                                   //    InvoiceNo = x.InvoiceNo,
                                   //    Amount = x.Amount,
                                   //    PaymentStatus = x.PaymentStatus,
                                   //    PaymentMethod = x.PaymentMethod,
                                   //    PaymentDate = x.PaymentDate,
                                   //    Waybill = x.Waybill,
                                   //    DueDate = x.DueDate,
                                   //    IsInternational = x.IsInternational
                                   //}).FirstOrDefault()
                                   //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()
                               }).Where(s => (s.Waybill == filterValue || s.GrandTotal.ToString() == filterValue || s.DateCreated.ToString() == filterValue)).ToList();


                //filter
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    preShipmentDto = preShipmentDto.Where(s => (s.GetType().GetProperty(filter).GetValue(s)).ToString().Contains(filterValue)).ToList();
                }

                //sort
                var sortorder = filterOptionsDto.sortorder;
                var sortvalue = filterOptionsDto.sortvalue;

                if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortvalue))
                {
                    System.Reflection.PropertyInfo prop = typeof(Shipment).GetProperty(sortvalue);

                    if (sortorder == "0")
                    {
                        preShipmentDto = preShipmentDto.OrderBy(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        //shipment = shipment.OrderBy(x => prop.Name); ;
                    }
                    else
                    {
                        preShipmentDto = preShipmentDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        //shipment = shipment.OrderByDescending(x => prop.Name); 
                    }

                }

                preShipmentDto = preShipmentDto.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count).ToList();

                return new Tuple<Task<List<PreShipmentDTO>>, int>(Task.FromResult(preShipmentDto.ToList()), count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<PreShipment> PreShipmentsAsQueryable()
        {
            var preShipments = _context.PreShipment.AsQueryable();
            return preShipments;
        }
    
    }
}
