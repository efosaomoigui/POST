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
using AutoMapper;
using GIGLS.CORE.DTO.Shipments;

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
                shipment = _context.Shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

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
                                                 DepartureServiceCentre = new ServiceCentreDTO
                                                 {
                                                     Code = r.DepartureServiceCentre.Code,
                                                     Name = r.DepartureServiceCentre.Name
                                                 },
                                                 DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                 DestinationServiceCentre = new ServiceCentreDTO
                                                 {
                                                     Code = r.DestinationServiceCentre.Code,
                                                     Name = r.DestinationServiceCentre.Name
                                                 },
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
                                                 GrandTotal = r.GrandTotal
                                                 //DepartureTerminalName = r.DepartureTerminal.Name,
                                                 //DestinationTerminalName = r.DestinationTerminal.Name       
                                                 //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()
                                             }).ToList();


            return Task.FromResult(shipmentDto.ToList());
        }

        public Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds)
        {
            try
            {
                //filter by service center
                var shipment = _context.Shipment.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    shipment = _context.Shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
                }
                ////

                var count = shipment.ToList().Count();
                shipment = shipment.OrderByDescending(x => x.DateCreated).Skip(filterOptionsDto.count * (filterOptionsDto.page - 1)).Take(filterOptionsDto.count);

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
                                                     DepartureServiceCentre = new ServiceCentreDTO
                                                     {
                                                         Code = r.DepartureServiceCentre.Code,
                                                         Name = r.DepartureServiceCentre.Name
                                                     },
                                                     DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                     DestinationServiceCentre = new ServiceCentreDTO
                                                     {
                                                         Code = r.DestinationServiceCentre.Code,
                                                         Name = r.DestinationServiceCentre.Name
                                                     },
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
                                                     GrandTotal = r.GrandTotal
                                                     //DepartureTerminalName = r.DepartureTerminal.Name,
                                                     //DestinationTerminalName = r.DestinationTerminal.Name       
                                                     //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()
                                                 }).ToList();
                //return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());

                //filter
                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterValue))
                {
                    shipmentDto = shipmentDto.Where(s => s.GetType().GetProperty(filter).GetValue(s) as string == filterValue).ToList();
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
                        //shipment = shipment.OrderBy(x => prop.Name); ;
                    }
                    else
                    {
                        shipmentDto = shipmentDto.OrderByDescending(x => x.GetType().GetProperty(prop.Name).GetValue(x)).ToList();
                        //shipment = shipment.OrderByDescending(x => prop.Name); 
                    }

                }

                return new Tuple<Task<List<ShipmentDTO>>, int>(Task.FromResult(shipmentDto.ToList()), count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetShipmentTotal()
        {
            var count = Context.State.ToList().Count();
            return count;
        }

        public int GetStatesTotal()
        {
            var count = Context.State.ToList().Count();
            return count;
        }

        public Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria f_Criteria)
        {
            DateTime StartDate = f_Criteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = f_Criteria.EndDate?.Date ?? StartDate;

            IQueryable<Shipment> shipments = Context.Shipment;

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

            var result = shipments.ToList();
            var shipmentDto = Mapper.Map<IEnumerable<ShipmentDTO>>(result);
            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
