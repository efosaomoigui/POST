using GIGLS.Core;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.IServices.Dashboard;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Customers;
using System;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using GIGLS.Core.View;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private IServiceCentreService _serviceCenterService;
        private IStationService _stationService;
        private ICustomerService _customerService;
        private IRegionServiceCentreMappingService _regionServiceCentreMappingService;
        private IRegionService _regionService;

        public DashboardService(IUnitOfWork uow,
            IUserService userService,
            IServiceCentreService serviceCenterService,
            IStationService stationService,
            ICustomerService customerService,
            IRegionServiceCentreMappingService regionServiceCentreMappingService,
            IRegionService regionService
            )
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            _stationService = stationService;
            _customerService = customerService;
            _regionServiceCentreMappingService = regionServiceCentreMappingService;
            _regionService = regionService;
        }

        public async Task<DashboardDTO> GetDashboard()
        {
            var dashboardDTO = new DashboardDTO();

            // get current user
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }
                if (claimValue == null)
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }


                if (claimValue[0] == "Public")
                {
                    dashboardDTO = new DashboardDTO()
                    {
                        Public = "Public"
                    };
                }
                else if (claimValue[0] == "Global")
                {
                    dashboardDTO = await GetDashboardForGlobal();
                }
                else if (claimValue[0] == "Region")
                {
                    dashboardDTO = await GetDashboardForRegion(int.Parse(claimValue[1]));
                }
                else if (claimValue[0] == "Station")
                {
                    dashboardDTO = await GetDashboardForStation(int.Parse(claimValue[1]));
                }
                else if (claimValue[0] == "ServiceCentre")
                {
                    dashboardDTO = await GetDashboardForServiceCentre(int.Parse(claimValue[1]));
                }
                else
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForServiceCentreOld(int serviceCenterId)
        {
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = new int[] { serviceCenterId };
            // get the service centre
            var serviceCentre = await _serviceCenterService.GetServiceCentreById(serviceCenterId);
            var allShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceView();
            var serviceCentreShipments = allShipmentsQueryable.Where(s => serviceCenterId == s.DepartureServiceCentreId);

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentre.TargetOrder;
            dashboardDTO.TargetAmount = serviceCentre.TargetAmount;


            // get shipment delivered - global
            //var shipmentTrackings = _uow.ShipmentTracking.GetAllAsQueryable();
            //var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.ARF.ToString());
            //var shipmentsDeliveredByServiceCenter =
            //    serviceCentreShipments.Where(s => shipmentsDelivered.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;
            //dashboardDTO.TotalShipmentDelivered = shipmentsDeliveredByServiceCenter.Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                shipmentsOrderedByServiceCenter.
                OrderByDescending(s => s.DateCreated).Take(5).ToList();

            dashboardDTO.MostRecentOrder = (from s in mostRecentOrder
                                            select new ShipmentOrderDTO()
                                            {
                                                //// customer
                                                Customer = string.Format($"{s.CustomerId}:{s.CustomerType}"),
                                                //Customer = _customerService.GetCustomer(
                                                //    s.CustomerId,
                                                //    (CustomerType)Enum.Parse(typeof(CustomerType), s.CustomerType)).
                                                //    Result.FirstName,
                                                //price
                                                Price = s.GrandTotal,
                                                //waybill
                                                Waybill = s.Waybill,
                                                //status
                                                //Status = shipmentTrackings.
                                                //    Where(a => a.Waybill == s.Waybill).
                                                //    OrderByDescending(b => b.DateCreated).
                                                //    First().Status,
                                                //date
                                                Date = s.DateCreated
                                            }).ToList();

            // populate customer
            await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForServiceCentre(int serviceCenterId)
        {
            int currentYear = DateTime.Now.Year;
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = new int[] { serviceCenterId };
            // get the service centre
            var serviceCentre = await _serviceCenterService.GetServiceCentreById(serviceCenterId);

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentre.TargetOrder;
            dashboardDTO.TargetAmount = serviceCentre.TargetAmount;

            var allShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated.Year == currentYear);
            var serviceCentreShipments = allShipmentsQueryable.Where(s => serviceCenterId == s.DepartureServiceCentreId);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.DateCreated.Year == currentYear && serviceCenterId == s.DepartureServiceCentreId).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //get customer 
            int accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated.Year == currentYear).Count();
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated.Year == currentYear).Count();
            dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForStationOld(int stationId)
        {
            var dashboardDTO = new DashboardDTO();

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            var serviceCenterIds = serviceCentres.Where(s => s.StationId == stationId).Select(s => s.ServiceCentreId).ToArray();
            var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            var serviceCentreShipments = allShipments.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId));


            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetAmount);

            // get shipment delivered - global
            var shipmentTrackings = _uow.ShipmentTracking.GetAllAsQueryable();
            //var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.ARF.ToString());
            //var shipmentsDeliveredByServiceCenter =
            //    serviceCentreShipments.Where(s => shipmentsDelivered.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // get all customers - individual and company
            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);

            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            var stationDTO = await _stationService.GetStationById(stationId);
            dashboardDTO.Station = stationDTO;
            //dashboardDTO.TotalShipmentDelivered = shipmentsDeliveredByServiceCenter.Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                shipmentsOrderedByServiceCenter.
                OrderByDescending(s => s.DateCreated).Take(5).ToList();

            dashboardDTO.MostRecentOrder = (from s in mostRecentOrder
                                            select new ShipmentOrderDTO()
                                            {
                                                //// customer
                                                Customer = string.Format($"{s.CustomerId}:{s.CustomerType}"),
                                                //Customer = _customerService.GetCustomer(
                                                //    s.CustomerId,
                                                //    (CustomerType)Enum.Parse(typeof(CustomerType), s.CustomerType)).
                                                //    Result.FirstName,
                                                //price
                                                Price = s.GrandTotal,
                                                //waybill
                                                Waybill = s.Waybill,
                                                //status
                                                //Status = shipmentTrackings.
                                                //    Where(a => a.Waybill == s.Waybill).
                                                //    OrderByDescending(b => b.DateCreated).
                                                //    First().Status,
                                                //date
                                                Date = s.DateCreated
                                            }).ToList();

            // populate customer
            await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForStation(int stationId)
        {
            int currentYear = DateTime.Now.Year;
            var dashboardDTO = new DashboardDTO();

            //get the station
            var stationDTO = await _stationService.GetStationById(stationId);
            dashboardDTO.Station = stationDTO;

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentresByStationId(stationId);
            int[] serviceCenterIds = serviceCentres.Select(s => s.ServiceCentreId).ToArray();

            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated.Year == currentYear);
            var serviceCentreShipments = allShipments.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId));

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.DateCreated.Year == currentYear && serviceCenterIds.Contains(s.DepartureServiceCentreId)).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //customers
            int accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated.Year == currentYear).Count();
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated.Year == currentYear).Count();
            dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForRegion(int regionId)
        {
            int currentYear = DateTime.Now.Year;
            var dashboardDTO = new DashboardDTO();

            //get the region
            var regionDTO = await _regionService.GetRegionById(regionId);
            dashboardDTO.Region = regionDTO;

            // get the service centre
            var regionServiceCentreMappingDTOList = await _regionServiceCentreMappingService.GetServiceCentresInRegion(regionId);
            var serviceCentres = regionServiceCentreMappingDTOList.Select(s => s.ServiceCentre).ToArray();
            int[] serviceCenterIds = serviceCentres.Select(s => s.ServiceCentreId).ToArray();

            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated.Year == currentYear);
            var serviceCentreShipments = allShipments.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId));

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.DateCreated.Year == currentYear && serviceCenterIds.Contains(s.DepartureServiceCentreId)).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //customers
            int accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated.Year == currentYear).Count();
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated.Year == currentYear).Count();
            dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForGlobalOld()
        {
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = { };   // empty array
            var serviceCentreShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceView();

            //set for TargetAmount and TargetOrder
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);


            // get shipment delivered
            var shipmentTrackings = _uow.ShipmentTracking.GetAllAsQueryable();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.ARF.ToString());

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipmentsQueryable.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // get all customers - individual and company
            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);


            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                serviceCentreShipmentsQueryable.
                OrderByDescending(s => s.DateCreated).Take(5).ToList();

            dashboardDTO.MostRecentOrder = (from s in mostRecentOrder
                                            select new ShipmentOrderDTO()
                                            {
                                                //// customer
                                                Customer = string.Format($"{s.CustomerId}:{s.CustomerType}"),
                                                //Customer = _customerService.GetCustomer(
                                                //    s.CustomerId,
                                                //    (CustomerType)Enum.Parse(typeof(CustomerType), s.CustomerType)).
                                                //    Result.FirstName,
                                                //price
                                                Price = s.GrandTotal,
                                                //waybill
                                                Waybill = s.Waybill,
                                                //status
                                                //Status = shipmentTrackings.
                                                //    Where(a => a.Waybill == s.Waybill).
                                                //    OrderByDescending(b => b.DateCreated).
                                                //    First().Status,
                                                //date
                                                Date = s.DateCreated
                                            }).ToList();

            // populate customer
            await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForGlobal()
        {
            int currentYear = DateTime.Now.Year;
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = { };   // empty array
            var serviceCentreShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated.Year == currentYear);

            //set for TargetAmount and TargetOrder
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipmentsQueryable.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.DateCreated.Year == currentYear).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //customers
            int accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated.Year == currentYear).Count();
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated.Year == currentYear).Count();
            dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphData(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task PopulateGraphData(DashboardDTO dashboardDTO)
        {
            var graphDataList = new List<GraphDataDTO>();
            var shipmentsOrderedByServiceCenter = dashboardDTO.ShipmentsOrderedByServiceCenter;
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            ////Only to solve last year report 
            //int currentYear = DateTime.Now.Year - 1;
            //int currentMonth = DateTime.Now.AddMonths(-1).Month;

            // filter shipments by current year
            var thisYearShipments = shipmentsOrderedByServiceCenter.Where(
                s => s.DateCreated.Year == currentYear);

            // fill GraphDataDTO by month
            for (int month = 1; month <= 12; month++)
            {
                var thisMonthShipments = thisYearShipments.Where(
                    s => s.DateCreated.Month == month);

                var graphData = new GraphDataDTO
                {
                    CalculationDay = 1,
                    ShipmentMonth = month,
                    ShipmentYear = currentYear,
                    TotalShipmentByMonth = thisMonthShipments.Count(),
                    TotalSalesByMonth = (from a in thisMonthShipments
                                         select a.GrandTotal).DefaultIfEmpty(0).Sum()
                };
                graphDataList.Add(graphData);

                // set the current month graphData
                if (currentMonth == month)
                {
                    dashboardDTO.CurrentMonthGraphData = graphData;
                }
            }

            dashboardDTO.GraphData = graphDataList;

            await Task.FromResult(0);
        }

        private int GetTotalCutomersCount(IQueryable<InvoiceView> shipmentsOrderedByServiceCenter)
        {
            var count = (from shipment in shipmentsOrderedByServiceCenter select shipment).
                GroupBy(g => new { g.CustomerType, g.CustomerId }).Count();

            return count;
        }

        private async Task PopulateCustomer(DashboardDTO dashboardDTO)
        {
            foreach (var order in dashboardDTO.MostRecentOrder)
            {
                string[] custArray = order.Customer.Split(':');

                if (string.IsNullOrEmpty(custArray[0]) || string.IsNullOrEmpty(custArray[1]))
                {
                    order.Customer = "Anonymous";
                }
                else
                {
                    var customerId = int.Parse(custArray[0]);
                    var customerType = CustomerType.Company;

                    if (CustomerType.IndividualCustomer.ToString().Contains(custArray[1]))
                    {
                        customerType = CustomerType.IndividualCustomer;
                    }

                    try
                    {
                        var customer = await _customerService.GetCustomer(
                            customerId, customerType);

                        if (customerType == CustomerType.IndividualCustomer)
                        {
                            order.Customer = string.Format($"{customer.FirstName} {customer.LastName}"); ;
                        }
                        else
                        {
                            order.Customer = customer.Name;
                        }
                    }
                    catch (Exception)
                    {
                        order.Customer = "Anonymous";
                    }
                }
            }
        }

        public async Task<int[]> GetCurrentUserServiceCenters()
        {
            int[] serviceCenterIds = { };   //empty array
            // get current user
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }
                if (claimValue == null)
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }

                if (claimValue[0] == "Global")
                {
                    serviceCenterIds = new int[] { };
                }
                else if (claimValue[0] == "Region")
                {
                    var regionId = int.Parse(claimValue[1]);
                    var regionServiceCentreMappingDTOList = await _regionServiceCentreMappingService.GetServiceCentresInRegion(regionId);
                    serviceCenterIds = regionServiceCentreMappingDTOList.Select(s => s.ServiceCentre.ServiceCentreId).ToArray();
                }
                else if (claimValue[0] == "Station")
                {
                    var stationId = int.Parse(claimValue[1]);
                    var serviceCentres = await _serviceCenterService.GetServiceCentres();
                    serviceCenterIds = serviceCentres.Where(s => s.StationId == stationId).Select(s => s.ServiceCentreId).ToArray();
                }
                else if (claimValue[0] == "ServiceCentre")
                {
                    int serviceCenterId = int.Parse(claimValue[1]);
                    serviceCenterIds = new int[] { serviceCenterId };
                }
                else
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return serviceCenterIds;
        }

        public async Task<DashboardDTO> GetDashboard(DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            // get current user
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }

                if (claimValue == null)
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }

                //if (!currentUser.DashboardAccess)
                //{
                //    dashboardDTO = await GetClaimDetails(claimValue);
                //    return dashboardDTO;
                //}


                if (claimValue[0] == "Public")
                {
                    dashboardDTO = new DashboardDTO()
                    {
                        Public = "Public"
                    };
                }
                else if (claimValue[0] == "Global")
                {
                    dashboardDTO = await GetDashboardForGlobal(dashboardFilterCriteria, currentUser.DashboardAccess);
                }
                else if (claimValue[0] == "Region")
                {
                    dashboardDTO = await GetDashboardForRegion(int.Parse(claimValue[1]), dashboardFilterCriteria);
                }
                else if (claimValue[0] == "Station")
                {
                    dashboardDTO = await GetDashboardForStation(int.Parse(claimValue[1]), dashboardFilterCriteria);
                }
                else if (claimValue[0] == "ServiceCentre")
                {
                    dashboardDTO = await GetDashboardForServiceCentre(int.Parse(claimValue[1]), dashboardFilterCriteria);
                }
                else
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
                }
                dashboardDTO.DashboardAccess = currentUser.DashboardAccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForServiceCentreSC(int serviceCenterId, DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = new int[] { serviceCenterId };
            // get the service centre
            var serviceCentre = await _serviceCenterService.GetServiceCentreById(serviceCenterId);

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentre.TargetOrder;
            dashboardDTO.TargetAmount = serviceCentre.TargetAmount;

            //get startDate and endDate
            var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var allShipmentsQueryable = _uow.MagayaShipment.GetAll().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false);
            var serviceCentreShipments = allShipmentsQueryable.Where(s => serviceCenterId == s.ServiceCenterId);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.MagayaShipmentOrdered = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;

            dashboardDTO.TotalShipmentDelivered = _uow.MagayaShipment.GetAll().
                Where(s => s.IsShipmentCollected == true && serviceCenterId == s.ServiceCenterId &&
                s.DateCreated >= startDate && s.DateCreated < endDate).Count();

            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //get customer 
            int accountCustomer = 0;
            int individualCustomer = 0;
            dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate graph data
            await PopulateGraphDataByDateInMagaya(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForServiceCentre(int serviceCenterId, DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = new int[] { serviceCenterId };

            // get the service centre
            var serviceCentre = await _serviceCenterService.GetServiceCentreById(serviceCenterId);

            ///////// do getDash if USA
            //if(serviceCentre.CountryId == 207)
            //{
            //    return await GetDashboardForServiceCentreSC(serviceCenterId, dashboardFilterCriteria); 
            //}

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentre.TargetOrder;
            dashboardDTO.TargetAmount = serviceCentre.TargetAmount;

            //get startDate and endDate
            var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var allShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false && s.PaymentStatus == PaymentStatus.Paid);
            var serviceCentreShipments = allShipmentsQueryable.Where(s => serviceCenterId == s.DepartureServiceCentreId);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && serviceCenterId == s.DepartureServiceCentreId && s.PaymentStatus == PaymentStatus.Paid && s.DateCreated >= startDate && s.DateCreated < endDate).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //get customer 
            var accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated >= startDate && c.DateCreated < endDate);
            int ecommerceBasic = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Basic).Count();
            int ecommerceClass = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Class).Count();

            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated >= startDate && i.DateCreated < endDate).Count();
            dashboardDTO.TotalCustomers = accountCustomer.Count() + individualCustomer;

            dashboardDTO.CustomerBreakdownDTO = new CustomerBreakdownDTO
            {
                Individual = individualCustomer,
                EcommerceBasic = ecommerceBasic,
                EcommerceClass = ecommerceClass,
            };

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate graph data
            await PopulateGraphDataByDate(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForStation(int stationId, DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            //get the station
            var stationDTO = await _stationService.GetStationById(stationId);
            dashboardDTO.Station = stationDTO;

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentresByStationId(stationId);
            int[] serviceCenterIds = serviceCentres.Select(s => s.ServiceCentreId).ToArray();
            //int[] countryIds = serviceCentres.Select(s => s.CountryId).ToArray();

            ///////// do getDash if USA
            //if (countryIds.Contains("United States Of America"))
            //{
            //    return await GetDashboardForMagaya(dashboardFilterCriteria);
            //}

            //get startDate and endDate
            var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false && s.PaymentStatus == PaymentStatus.Paid);
            var serviceCentreShipments = allShipments.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId));

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.PaymentStatus == PaymentStatus.Paid && s.DateCreated >= startDate && s.DateCreated < endDate && serviceCenterIds.Contains(s.DepartureServiceCentreId)).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //customers
            var accountCustomer = _uow.Company.GetAllAsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated >= startDate && i.DateCreated < endDate).Count();
            dashboardDTO.TotalCustomers = accountCustomer.Count() + individualCustomer;

            int ecommerceBasic = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Basic).Count();
            int ecommerceClass = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Class).Count();

            dashboardDTO.CustomerBreakdownDTO = new CustomerBreakdownDTO
            {
                Individual = individualCustomer,
                EcommerceBasic = ecommerceBasic,
                EcommerceClass = ecommerceClass,
            };

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphDataByDate(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForRegion(int regionId, DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            //get the region
            var regionDTO = await _regionService.GetRegionById(regionId);
            dashboardDTO.Region = regionDTO;

            // get the service centre
            var regionServiceCentreMappingDTOList = await _regionServiceCentreMappingService.GetServiceCentresInRegion(regionId);
            var serviceCentres = regionServiceCentreMappingDTOList.Select(s => s.ServiceCentre).ToArray();
            int[] serviceCenterIds = serviceCentres.Select(s => s.ServiceCentreId).ToArray();

            //get startDate and endDate
            var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false && s.PaymentStatus == PaymentStatus.Paid);
            var serviceCentreShipments = allShipments.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId));

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipments.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.IsShipmentCollected == true && s.PaymentStatus == PaymentStatus.Paid && s.DateCreated >= startDate && s.DateCreated < endDate && serviceCenterIds.Contains(s.DepartureServiceCentreId)).Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            //customers
            var accountCustomer = _uow.Company.GetAllAsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);
            int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated >= startDate && i.DateCreated < endDate).Count();
            dashboardDTO.TotalCustomers = accountCustomer.Count() + individualCustomer;

            int ecommerceBasic = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Basic).Count();
            int ecommerceClass = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Class).Count();
            dashboardDTO.TotalCustomers = accountCustomer.Count() + individualCustomer;

            dashboardDTO.CustomerBreakdownDTO = new CustomerBreakdownDTO
            {
                Individual = individualCustomer,
                EcommerceBasic = ecommerceBasic,
                EcommerceClass = ecommerceClass,
            };

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            await PopulateGraphDataByDate(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForMagaya(DashboardFilterCriteria dashboardFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            //get startDate and endDate
            var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var magayaShipmentsQueryable = _uow.MagayaShipment.GetAll().Where(s =>
                    s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false);

            var TotalShipmentDeliveredQueryable = _uow.MagayaShipment.GetAll().Where(s =>
                    s.IsShipmentCollected == true && s.DateCreated >= startDate && s.DateCreated < endDate);

            if (dashboardFilterCriteria.ActiveCountryId != null && dashboardFilterCriteria.ActiveCountryId > 0)
            {
                magayaShipmentsQueryable = magayaShipmentsQueryable.Where(s =>
                    s.ServiceCenterCountryId == dashboardFilterCriteria.ActiveCountryId);

                TotalShipmentDeliveredQueryable = TotalShipmentDeliveredQueryable.Where(s =>
                    s.ServiceCenterCountryId == dashboardFilterCriteria.ActiveCountryId);

                //customers
                int accountCustomer = 0;
                int individualCustomer = 0;
                dashboardDTO.TotalCustomers = accountCustomer + individualCustomer;

                //Update the ActiveCountryId in the User entity
                string currentUserId = await _userService.GetCurrentUserId();
                var userEntity = await _uow.User.GetUserById(currentUserId);
                userEntity.UserActiveCountryId = (int)dashboardFilterCriteria.ActiveCountryId;
                _uow.Complete();
            }

            //set for TargetAmount and TargetOrder
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = magayaShipmentsQueryable.ToList().AsQueryable();
            dashboardDTO.MagayaShipmentOrdered = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = TotalShipmentDeliveredQueryable.Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate graph data
            await PopulateGraphDataByDateInMagaya(dashboardDTO);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;

        }

        private async Task<DashboardDTO> GetDashboardForGlobal(DashboardFilterCriteria dashboardFilterCriteria, bool dashboardAccess)
        {
            var dashboardDTO = new DashboardDTO();
            dashboardDTO.DashboardAccess = dashboardAccess;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!dashboardFilterCriteria.StartDate.HasValue && !dashboardFilterCriteria.EndDate.HasValue)
            {
                var threeMonthsAgo = DateTime.Now.AddMonths(-2);  //One (1) Months ago
                startDate = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                //get startDate and endDate
                var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                startDate = queryDate.Item1;
                endDate = queryDate.Item2;
            }

            int[] serviceCenterIds = { };   // empty array
            var serviceCentreShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s =>
            s.DateCreated >= startDate && s.DateCreated < endDate && s.IsFromMobile == false && s.PaymentStatus == PaymentStatus.Paid);

            //filter by country
            var TotalShipmentDeliveredQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s =>
                s.IsShipmentCollected == true && s.PaymentStatus == PaymentStatus.Paid
                && s.DateCreated >= startDate
                && s.DateCreated < endDate);

            if (dashboardFilterCriteria.ActiveCountryId != null && dashboardFilterCriteria.ActiveCountryId > 0)
            {
                serviceCentreShipmentsQueryable = serviceCentreShipmentsQueryable.Where(s =>
                    s.DepartureCountryId == dashboardFilterCriteria.ActiveCountryId);

                TotalShipmentDeliveredQueryable = TotalShipmentDeliveredQueryable.Where(s =>
                    s.DepartureCountryId == dashboardFilterCriteria.ActiveCountryId);

                //customers
                var accountCustomer = _uow.Company.GetAllAsQueryable().Where(c => c.DateCreated >= startDate && c.DateCreated < endDate && c.UserActiveCountryId == dashboardFilterCriteria.ActiveCountryId);
                int individualCustomer = _uow.IndividualCustomer.GetAllAsQueryable().Where(i => i.DateCreated >= startDate && i.DateCreated < endDate && i.UserActiveCountryId == dashboardFilterCriteria.ActiveCountryId).Count();
                dashboardDTO.TotalCustomers = accountCustomer.Count() + individualCustomer;

                //get customer 
                int ecommerceBasic = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Basic).Count();
                int ecommerceClass = accountCustomer.Where(c => c.CompanyType == CompanyType.Ecommerce && c.Rank == Rank.Class).Count();

                dashboardDTO.CustomerBreakdownDTO = new CustomerBreakdownDTO
                {
                    Individual = individualCustomer,
                    EcommerceBasic = ecommerceBasic,
                    EcommerceClass = ecommerceClass,
                };

                //Update the ActiveCountryId in the User entity
                string currentUserId = await _userService.GetCurrentUserId();
                var userEntity = await _uow.User.GetUserById(currentUserId);
                userEntity.UserActiveCountryId = (int)dashboardFilterCriteria.ActiveCountryId;

                if (dashboardAccess)
                {
                    dashboardDTO.WalletBalance = await GetWalletBalanceForAllCustomers(userEntity.UserActiveCountryId);
                    dashboardDTO.WalletTransactionSummary = await GetWalletTransactionSummary(dashboardFilterCriteria);
                    dashboardDTO.WalletPaymentLogSummary = await GetWalletPaymentSummary(dashboardFilterCriteria);
                    dashboardDTO.WalletBreakdown = await GetWalletBreakdown(userEntity.UserActiveCountryId);

                    dashboardDTO.EarningsBreakdownDTO = new EarningsBreakdownDTO();

                    //get all earnings
                    dashboardDTO.EarningsBreakdownDTO.GrandTotal = await GetTotalFinancialReportEarnings(dashboardFilterCriteria);
                    var demmurage = await _uow.FinancialReport.GetTotalFinancialReportDemurrage(dashboardFilterCriteria);

                    dashboardDTO.EarningsBreakdownDTO.GrandTotal += demmurage;

                    //get outstanding corporate payments
                    if (dashboardFilterCriteria.ActiveCountryId == 1)
                    {
                        var outstandingPayments = _uow.Wallet.GetAllAsQueryable().Where(s => s.CompanyType == CompanyType.Corporate.ToString() && s.Balance < 0).Sum(x => x.Balance);
                        dashboardDTO.OutstandingCorporatePayment = System.Math.Abs(outstandingPayments);
                    }
                }
                _uow.Complete();
            }

            //set for TargetAmount and TargetOrder
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);

            // get shipment ordered
            var shipmentsOrderedByServiceCenter = serviceCentreShipmentsQueryable.ToList().AsQueryable();
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // set properties
            dashboardDTO.TotalShipmentDelivered = TotalShipmentDeliveredQueryable.Count();
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count();

            // MostRecentOrder
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };

            // populate customer
            //await PopulateCustomer(dashboardDTO);

            // populate graph data
            //await PopulateGraphDataByDate(dashboardDTO);
            await PopulateGraphDataByDateGlobal(dashboardDTO, startDate, endDate);

            // reset the dashboardDTO.ShipmentsOrderedByServiceCenter
            dashboardDTO.ShipmentsOrderedByServiceCenter = null;

            return dashboardDTO;
        }

        private async Task PopulateGraphDataByDate(DashboardDTO dashboardDTO)
        {
            var graphDataList = new List<GraphDataDTO>();
            var shipmentsOrderedByServiceCenter = dashboardDTO.ShipmentsOrderedByServiceCenter;
            int currentMonth = DateTime.Now.Month;

            if (dashboardDTO.DashboardAccess)
            {
                var threeMonthsAgo = DateTime.Now.AddMonths(-2);
                currentMonth = threeMonthsAgo.Month;
            }

            //use this date as the next year of when we launched Agility to cater for
            //month we have not launch agility that will be empty
            int year = 2018;

            // fill GraphDataDTO by month
            for (int month = 1; month <= 12; month++)
            {
                var thisMonthShipments = shipmentsOrderedByServiceCenter.Where(
                    s => s.DateCreated.Month == month && s.IsFromMobile == false);

                var firstDataToGetYear = thisMonthShipments.FirstOrDefault();
                if (firstDataToGetYear != null)
                {
                    year = firstDataToGetYear.DateCreated.Year;
                }
                else if (dashboardDTO.ServiceCentre != null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.ServiceCentre.DateCreated.Year;
                }
                else if (dashboardDTO.Station != null && dashboardDTO.ServiceCentre == null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.Station.DateCreated.Year;
                }

                var graphData = new GraphDataDTO
                {
                    CalculationDay = 1,
                    ShipmentMonth = month,
                    ShipmentYear = year,
                    TotalShipmentByMonth = thisMonthShipments.Count(),
                    TotalSalesByMonth = (from a in thisMonthShipments
                                         select a.GrandTotal).DefaultIfEmpty(0).Sum()
                };

                graphDataList.Add(graphData);

                // set the current month graphData
                if (currentMonth == month)
                {
                    dashboardDTO.CurrentMonthGraphData = graphData;
                }
            }

            dashboardDTO.GraphData = graphDataList;
            await Task.FromResult(0);

        }

        private async Task PopulateGraphDataByDateGlobal(DashboardDTO dashboardDTO, DateTime startDate, DateTime endDate)
        {
            var graphDataList = new List<GraphDataDTO>();
            var shipmentsOrderedByServiceCenter = dashboardDTO.ShipmentsOrderedByServiceCenter;

            dashboardDTO.CurrentMonthGraphData = new GraphDataDTO();

            //use this date as the next year of when we launched Agility to cater for
            //month we have not launch agility that will be empty
            int year = 2018;
            int startMonth = startDate.Month;
            int endMonth = endDate.Month;

            //fill GraphDataDTO by month
            foreach (DateTime date in EachMonth(startDate, endDate))
            {
                var month = date.Month;

                var thisMonthShipments = shipmentsOrderedByServiceCenter.Where(
                    s => s.DateCreated.Month == month && s.IsFromMobile == false);

                var firstDataToGetYear = thisMonthShipments.FirstOrDefault();
                if (firstDataToGetYear != null)
                {
                    year = firstDataToGetYear.DateCreated.Year;
                }
                else if (dashboardDTO.ServiceCentre != null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.ServiceCentre.DateCreated.Year;
                }
                else if (dashboardDTO.Station != null && dashboardDTO.ServiceCentre == null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.Station.DateCreated.Year;
                }

                var graphData = new GraphDataDTO
                {
                    CalculationDay = 1,
                    ShipmentMonth = month,
                    ShipmentYear = year,
                    TotalShipmentByMonth = thisMonthShipments.Count(),
                    TotalSalesByMonth = (from a in thisMonthShipments
                                         select a.GrandTotal).DefaultIfEmpty(0).Sum()
                };

                graphDataList.Add(graphData);

                dashboardDTO.CurrentMonthGraphData.TotalShipmentByMonth += graphData.TotalShipmentByMonth;
                dashboardDTO.CurrentMonthGraphData.TotalSalesByMonth += graphData.TotalSalesByMonth;
            }

            dashboardDTO.GraphData = graphDataList;
            await Task.FromResult(0);

        }

        private IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var month = from.Date; month.Date <= thru.Date; month = month.AddMonths(1))
                yield return month;
        }

        private async Task PopulateGraphDataByDateInMagaya(DashboardDTO dashboardDTO)
        {
            var graphDataList = new List<GraphDataDTO>();
            var shipmentsOrderedByServiceCenter = dashboardDTO.MagayaShipmentOrdered;
            int currentMonth = DateTime.Now.Month;

            //month we have not launch agility that will be empty
            int year = 2018;

            // fill GraphDataDTO by month
            for (int month = 1; month <= 12; month++)
            {
                var thisMonthShipments = shipmentsOrderedByServiceCenter.Where(
                    s => s.DateCreated.Month == month && s.IsFromMobile == false);

                var firstDataToGetYear = thisMonthShipments.FirstOrDefault();

                if (firstDataToGetYear != null)
                {
                    year = firstDataToGetYear.DateCreated.Year;
                }
                else if (dashboardDTO.ServiceCentre != null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.ServiceCentre.DateCreated.Year;
                }
                else if (dashboardDTO.Station != null && dashboardDTO.ServiceCentre == null && firstDataToGetYear == null)
                {
                    year = dashboardDTO.Station.DateCreated.Year;
                }

                var graphData = new GraphDataDTO
                {
                    CalculationDay = 1,
                    ShipmentMonth = month,
                    ShipmentYear = year,
                    TotalShipmentByMonth = thisMonthShipments.Count(),
                    TotalSalesByMonth = (from a in thisMonthShipments
                                         select a.GrandTotal).DefaultIfEmpty(0).Sum()
                };

                graphDataList.Add(graphData);

                // set the current month graphData
                if (currentMonth == month)
                {
                    dashboardDTO.CurrentMonthGraphData = graphData;
                }
            }

            dashboardDTO.GraphData = graphDataList;
            await Task.FromResult(0);
        }

        //Get Wallet balance 
        private async Task<decimal> GetWalletBalanceForAllCustomers(int countryId)
        {
            return await _uow.Wallet.GetTotalWalletBalance(countryId);
        }

        //Get Total Earnings in Financial Reports 
        private async Task<decimal> GetTotalFinancialReportEarnings(DashboardFilterCriteria dashboardFilterCriteria)
        {
            return await _uow.FinancialReport.GetTotalFinancialReportEarnings(dashboardFilterCriteria);
        }

        private async Task<WalletBreakdown> GetWalletBreakdown(int countryId)
        {
            return await _uow.Wallet.GetWalletBreakdown(countryId);
        }

        //Get Wallet Transaction Credit
        private async Task<WalletTransactionSummary> GetWalletTransactionSummary(DashboardFilterCriteria dashboardFilterCriteria)
        {
            return await _uow.WalletTransaction.GetWalletTransactionSummary(dashboardFilterCriteria);
        }

        //Get Wallet Transaction Credit
        private async Task<WalletPaymentLogSummary> GetWalletPaymentSummary(DashboardFilterCriteria dashboardFilterCriteria)
        {
            return await _uow.WalletTransaction.GetWalletPaymentSummary(dashboardFilterCriteria);
        }

        //Get Claim for Non-Dashboard Access
        private async Task<DashboardDTO> GetClaimDetails(string[] claimValue)
        {
            var dashboardDTO = new DashboardDTO();

            if (claimValue[0] == "Public")
            {
                dashboardDTO.Public = "Public";
            }
            else if (claimValue[0] == "Region")
            {
                //get the region
                var regionDTO = await _regionService.GetRegionById(int.Parse(claimValue[1]));
                dashboardDTO.Region = regionDTO;
            }
            else if (claimValue[0] == "Station")
            {
                //get the station
                var stationDTO = await _stationService.GetStationById(int.Parse(claimValue[1]));
                dashboardDTO.Station = stationDTO;
            }
            else if (claimValue[0] == "ServiceCentre")
            {
                var serviceCentre = await _serviceCenterService.GetServiceCentreById(int.Parse(claimValue[1]));
                dashboardDTO.ServiceCentre = serviceCentre;
            }

            return dashboardDTO;
        }

        //Get Earnings Breakdown
        private async Task<EarningsBreakdownDTO> GetEarningsBreakdown(IQueryable<FinancialReport> earnings)
        {
            var earningsBreakdownDTO = new EarningsBreakdownDTO();

            earningsBreakdownDTO.GIGGO = earnings.Where(x => x.Source == ReportSource.GIGGo).Select(x => x.Earnings).DefaultIfEmpty(0).Sum();
            earningsBreakdownDTO.Agility = earnings.Where(x => x.Source == ReportSource.Agility).Select(x => x.Earnings).DefaultIfEmpty(0).Sum();
            earningsBreakdownDTO.GrandTotal = earnings.Select(x => x.Earnings).DefaultIfEmpty(0).Sum();
            earningsBreakdownDTO.Demurrage = earnings.Select(x => x.Demurrage).DefaultIfEmpty(0).Sum();

            return earningsBreakdownDTO;

        }
    }
}