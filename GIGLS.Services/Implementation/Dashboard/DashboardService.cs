using GIGLS.Core;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.IServices.Dashboard;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using System;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;

namespace GIGLS.Services.Implementation.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private IShipmentService _shipmentService;
        private IUserService _userService;
        private IShipmentTrackingService _shipmentTrackingService;
        private IServiceCentreService _serviceCenterService;
        private IStationService _stationService;
        private IIndividualCustomerService _individualCustomerService;
        private ICompanyService _companyService;
        private ICustomerService _customerService;

        public DashboardService(
            IShipmentService shipmentService, IUserService userService,
            IShipmentTrackingService shipmentTrackingService,
            IServiceCentreService serviceCenterService,
            IStationService stationService,
            IIndividualCustomerService individualCustomerService,
            ICompanyService companyService,
            ICustomerService customerService
            )
        {
            _shipmentService = shipmentService;
            _userService = userService;
            _shipmentTrackingService = shipmentTrackingService;
            _serviceCenterService = serviceCenterService;
            _stationService = stationService;
            _individualCustomerService = individualCustomerService;
            _companyService = companyService;
            _customerService = customerService;
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

                if (claimValue[0] == "Global")
                {
                    dashboardDTO = await GetDashboardForGlobal();
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

        private async Task<DashboardDTO> GetDashboardForServiceCentre(int serviceCenterId)
        {
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = new int[] { serviceCenterId };
            // get the service centre
            var serviceCentre = await _serviceCenterService.GetServiceCentreById(serviceCenterId);
            var serviceCentreShipments = await _shipmentService.GetShipments(serviceCenterIds);

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentre.TargetOrder;
            dashboardDTO.TargetAmount = serviceCentre.TargetAmount;


            // get shipment delivered - global
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();
            var shipmentsDeliveredByServiceCenter =
                serviceCentreShipments.Where(s => shipmentsDelivered.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();
            //var shipmentsOrderedByServiceCenter =
            //        serviceCentreShipments.Where(s => shipmentsOrdered.Select(d => d.Waybill).Contains(s.Waybill));
            var shipmentsOrderedByServiceCenter = serviceCentreShipments;
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // get all customers - individual and company
            //var companys = await _companyService.GetCompanies();
            //var individuals = await _individualCustomerService.GetIndividualCustomers();
            //var totalCustomers = companys.Count + individuals.Count;
            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                shipmentsOrderedByServiceCenter.
                OrderByDescending(s => s.DateCreated).Take(5);

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
            var dashboardDTO = new DashboardDTO();

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            var serviceCenterIds = serviceCentres.Where(s => s.StationId == stationId).Select(s => s.ServiceCentreId).ToArray();
            var serviceCentreShipments = await _shipmentService.GetShipments(serviceCenterIds);

            //set for TargetAmount and TargetOrder
            dashboardDTO.TargetOrder = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Where(s => s.StationId == stationId).Sum(s => s.TargetAmount);

            // get shipment delivered - global
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();
            var shipmentsDeliveredByServiceCenter =
                serviceCentreShipments.Where(s => shipmentsDelivered.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();
            //var shipmentsOrderedByServiceCenter =
            //        serviceCentreShipments.Where(s => shipmentsOrdered.Select(d => d.Waybill).Contains(s.Waybill));
            var shipmentsOrderedByServiceCenter = serviceCentreShipments;
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // get all customers - individual and company
            //var companys = await _companyService.GetCompanies();
            //var individuals = await _individualCustomerService.GetIndividualCustomers();
            //var totalCustomers = companys.Count + individuals.Count;
            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);

            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            var stationDTO = await _stationService.GetStationById(stationId);
            dashboardDTO.Station = stationDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                shipmentsOrderedByServiceCenter.
                OrderByDescending(s => s.DateCreated).Take(5);

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
            var dashboardDTO = new DashboardDTO();

            int[] serviceCenterIds = { };   //empty array
            var serviceCentreShipments = await _shipmentService.GetShipments(serviceCenterIds);

            //set for TargetAmount and TargetOrder
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            dashboardDTO.TargetOrder = serviceCentres.Sum(s => s.TargetOrder);
            dashboardDTO.TargetAmount = serviceCentres.Sum(s => s.TargetAmount);


            // get shipment delivered
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();

            // get shipment ordered
            //var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();
            var shipmentsOrderedByServiceCenter = serviceCentreShipments;
            dashboardDTO.ShipmentsOrderedByServiceCenter = shipmentsOrderedByServiceCenter;

            // get all customers - individual and company
            //var companys = await _companyService.GetCompanies();
            //var individuals = await _individualCustomerService.GetIndividualCustomers();
            //var totalCustomers = companys.Count + individuals.Count;
            var totalCustomers = GetTotalCutomersCount(shipmentsOrderedByServiceCenter);


            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrderedByServiceCenter.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            // MostRecentOrder
            var mostRecentOrder =
                serviceCentreShipments.
                OrderByDescending(s => s.DateCreated).Take(5);

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

        private async Task PopulateGraphData(DashboardDTO dashboardDTO)
        {
            var graphDataList = new List<GraphDataDTO>();
            var shipmentsOrderedByServiceCenter = dashboardDTO.ShipmentsOrderedByServiceCenter;
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

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
                                         select a.GrandTotal).Sum()
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

        private int GetTotalCutomersCount(List<ShipmentDTO> shipmentsOrderedByServiceCenter)
        {
            int count = 0;
            HashSet<string> customerHashSet = new HashSet<string>();

            foreach (var shipment in shipmentsOrderedByServiceCenter)
            {
                customerHashSet.Add($"{shipment.CustomerType}:{shipment.CustomerId}");
            }

            count = customerHashSet.Count;
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
                    catch (Exception ex)
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
    }
}
