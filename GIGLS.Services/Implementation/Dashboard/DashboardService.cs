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

        public DashboardService(
            IShipmentService shipmentService, IUserService userService,
            IShipmentTrackingService shipmentTrackingService,
            IServiceCentreService serviceCenterService,
            IStationService stationService,
            IIndividualCustomerService individualCustomerService,
            ICompanyService companyService
            )
        {
            _shipmentService = shipmentService;
            _userService = userService;
            _shipmentTrackingService = shipmentTrackingService;
            _serviceCenterService = serviceCenterService;
            _stationService = stationService;
            _individualCustomerService = individualCustomerService;
            _companyService = companyService;
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

            // get shipment delivered - global
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();
            var shipmentsDeliveredByServiceCenter =
                shipmentsDelivered.Where(s => serviceCentreShipments.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();
            var shipmentsOrderedByServiceCenter =
                    shipmentsOrdered.Where(s => serviceCentreShipments.Select(d => d.Waybill).Contains(s.Waybill));


            // get all customers - individual and company
            var companys = await _companyService.GetCompanies();
            var individuals = await _individualCustomerService.GetIndividualCustomers();
            var totalCustomers = companys.Count + individuals.Count;

            // set properties
            dashboardDTO.ServiceCentre = serviceCentre;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrdered.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForStation(int stationId)
        {
            var dashboardDTO = new DashboardDTO();

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentres();
            var serviceCenterIds = serviceCentres.Where(s => s.StationId == stationId).Select(s => s.ServiceCentreId).ToArray();
            var serviceCentreShipments = await _shipmentService.GetShipments(serviceCenterIds);

            // get shipment delivered - global
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();
            var shipmentsDeliveredByServiceCenter =
                shipmentsDelivered.Where(s => serviceCentreShipments.Select(d => d.Waybill).Contains(s.Waybill));


            // get shipment ordered
            var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();
            var shipmentsOrderedByServiceCenter =
                    shipmentsOrdered.Where(s => serviceCentreShipments.Select(d => d.Waybill).Contains(s.Waybill));


            // get all customers - individual and company
            var companys = await _companyService.GetCompanies();
            var individuals = await _individualCustomerService.GetIndividualCustomers();
            var totalCustomers = companys.Count + individuals.Count;

            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrdered.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            return dashboardDTO;
        }

        private async Task<DashboardDTO> GetDashboardForGlobal()
        {
            var dashboardDTO = new DashboardDTO();

            // get shipment delivered
            var shipmentTrackings = await _shipmentTrackingService.GetShipmentTrackings();
            var shipmentsDelivered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Delivered.ToString()).ToList();

            // get shipment ordered
            var shipmentsOrdered = shipmentTrackings.Where(s => s.Status == ShipmentScanStatus.Recieved.ToString()).ToList();

            // get all customers - individual and company
            var companys = await _companyService.GetCompanies();
            var individuals = await _individualCustomerService.GetIndividualCustomers();
            var totalCustomers = companys.Count + individuals.Count;

            // set properties
            //dashboardDTO.ServiceCentre = serviceCentreDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrdered.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            return dashboardDTO;
        }
    }
}
