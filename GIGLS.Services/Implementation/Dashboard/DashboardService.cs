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

namespace GIGLS.Services.Implementation.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private IShipmentService _shipmentService;
        private IUserService _userService;
        private IShipmentTrackingService _shipmentTrackingService;
        private IServiceCentreService _serviceCenterService;
        private IIndividualCustomerService _individualCustomerService;
        private ICompanyService _companyService;

        public DashboardService(
            IShipmentService shipmentService, IUserService userService,
            IShipmentTrackingService shipmentTrackingService, 
            IServiceCentreService serviceCenterService,
            IIndividualCustomerService individualCustomerService,
            ICompanyService companyService
            )
        {
            _shipmentService = shipmentService;
            _userService = userService;
            _shipmentTrackingService = shipmentTrackingService;
            _serviceCenterService = serviceCenterService;
            _individualCustomerService = individualCustomerService;
            _companyService = companyService;
        }

        public async Task<DashboardDTO> GetDashboard()
        {
            var dashboardDTO = new DashboardDTO();

            // get current user and service centre
            ServiceCentreDTO serviceCentreDTO = null;
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userServiceCentreCode = currentUser.UserActiveServiceCentre;
                serviceCentreDTO = await _serviceCenterService.GetServiceCentreByCode(userServiceCentreCode);
            }
            catch (Exception ex)
            {
                // service centre was not passed

            }

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
            dashboardDTO.ServiceCentre = serviceCentreDTO;
            dashboardDTO.TotalShipmentDelivered = shipmentsDelivered.Count;
            dashboardDTO.TotalShipmentOrdered = shipmentsOrdered.Count;
            dashboardDTO.TotalCustomers = totalCustomers;

            return dashboardDTO;
        }

    }
}
