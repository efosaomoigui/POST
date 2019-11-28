using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Report;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.View;
using GIGLS.Core.View.AdminReportView;
using GIGLS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Report
{
    public class AdminReportService : IAdminReportService
    {
        private readonly IUnitOfWork _uow;
        private IServiceCentreService _serviceCenterService;

        public AdminReportService(IUnitOfWork uow, IServiceCentreService serviceCenterService)
        {
            _uow = uow;
            _serviceCenterService = serviceCenterService;
        }

        public async Task<AdminReportDTO> GetAdminReport(ShipmentCollectionFilterCriteria filterCriteria)
        {
            AdminReportDTO result = new AdminReportDTO
            {
                BusiestRoute = await GetBusiestRoutes(),
                MostShippedItemByWeight = await GetMostShippedItemByWeights(),
                TotalServiceCentreByState = await GetTotalServiceCentreByStates()

                //result.AllTimeSalesByCountry = await GetAllTimeSalesByCountries();
                //result.CustomerRevenue = await GetCustomerRevenues();
                //result.RevenuePerServiceCentre = await GetRevenuePerServiceCentres();
                //result.TotalOrdersDelivered = await GetTotalOrdersDelivered();
            };

            //Get customer count
            result.NumberOfCustomer.Corporate = await GetCorporateCount(filterCriteria);
            result.NumberOfCustomer.Ecommerce = await GetEcommerceCount(filterCriteria);
            result.NumberOfCustomer.Individual = await GetIndividaulCount(filterCriteria);

            result.InvoiceReportDTO = await InvoiceInformation(filterCriteria);

            return result;
        }
        //To display data for the website
        public async Task<AdminReportDTO> DisplayWebsiteData()
        {
            AdminReportDTO result = new AdminReportDTO();
            
            result.NumberOfCustomer.Ecommerce = await GetEcommerceCount();
            result.NumberOfCustomer.Individual = await GetIndividaulCount();
            result.NumberOfCustomer.Corporate = await GetCorporateCount();
            result.TotalCustomers = result.NumberOfCustomer.Ecommerce + result.NumberOfCustomer.Individual + result.NumberOfCustomer.Corporate;

            result.TotalOrdersDelivered = await GetTotalOrdersDelivered();
            return result;
        }
                
        private async Task<List<Report_AllTimeSalesByCountry>> GetAllTimeSalesByCountries()
        {
            var result = _uow.Invoice.GetAllTimeSalesByCountry().ToList();
            return await Task.FromResult(result);
        }

        private async Task<List<Report_BusiestRoute>> GetBusiestRoutes()
        {
            var result = _uow.Invoice.GetBusiestRoute().OrderByDescending(x => x.TotalShipment).Take(5).ToList();
            return await Task.FromResult(result);
        }

        private async Task<List<Report_CustomerRevenue>> GetCustomerRevenues()
        {
            var result = _uow.Invoice.GetCustomerRevenue().ToList();
            return await Task.FromResult(result);
        }

        private async Task<InvoiceReportDTO> InvoiceInformation(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var result = new InvoiceReportDTO();
            var invoice = _uow.Invoice.GetAllFromInvoiceAndShipments(filterCriteria).ToList();

            List<InvoiceView> invoices;
            List<InvoiceView> destInvoices;
            decimal avgDestShipmentCostPerSC = 0;
            decimal avgOriginShipmentCostPerSC = 0;
            var createdShipments = 0;
            var departedShipments = 0;

            //Filter by Service Center
            if (filterCriteria.ServiceCentreId > 0)
            {
                //For the Departure Service Center Data Only
                invoices = invoice.Where(s => s.DepartureServiceCentreId == filterCriteria.ServiceCentreId).ToList();

                //For the Detination Service Center Data Only, so as to calaulate the average coming in
                destInvoices = invoice.Where(s => s.DestinationServiceCentreId == filterCriteria.ServiceCentreId).ToList();

                //Average Price Of Shipments coming to that service Center
                avgDestShipmentCostPerSC = (destInvoices.Sum(p => p.GrandTotal) / destInvoices.Count());
                                
            }
            else
            {
                invoices = invoice;
            }
           

            var revenue = invoices.Sum(x => x.GrandTotal);
            var shipmentDeliverd = invoices.Where(x => x.IsShipmentCollected == true).Count();
            var shipmentOrdered = invoices.Count();

            //customer Type
            string individual = FilterCustomerType.IndividualCustomer.ToString();
            string ecommerce = FilterCustomerType.Ecommerce.ToString();
            string corporate = FilterCustomerType.Corporate.ToString();


            //Revenue per customer type
            var individualRevenue = invoices.Where(x => x.CompanyType == individual).Sum(x => x.GrandTotal);
            var ecommerceRevenue = invoices.Where(x => x.CompanyType == ecommerce).Sum(x => x.GrandTotal);
            var corporateRevenue = invoices.Where(x => x.CompanyType == corporate).Sum(x => x.GrandTotal);

            //Count of Shipments Per Customer Type 
            var individualShipmentCount = invoices.Where(x => x.CompanyType == individual).Count();
            var ecommerceShipmentCount = invoices.Where(x => x.CompanyType == ecommerce).Count();
            var corporateShipmentCount = invoices.Where(x => x.CompanyType == corporate).Count();

            //Get Average Spent Per Customer Type
            var avgIndividualCost = individualRevenue / ((individualShipmentCount == 0) ? 1 : individualShipmentCount) ;
            var avgEcommerceCost = ecommerceRevenue / ((ecommerceShipmentCount == 0) ? 1 : ecommerceShipmentCount);
            var avgCorporateCost = corporateRevenue / ((corporateShipmentCount == 0) ? 1 : corporateShipmentCount);

            //All home delivery
            var homeDeliveries = invoices.Where(x => x.PickupOptions == PickupOptions.HOMEDELIVERY).Count();

            //All Terminal Pickup
            var terminalPickups = invoices.Where(x => x.PickupOptions == PickupOptions.SERVICECENTER).Count();

            //Ecommerce home delivery
            var ecommerceHome = invoices.Where(x => x.PickupOptions == PickupOptions.HOMEDELIVERY && x.CompanyType == ecommerce).Count();

            //Ecommerce Terminal Pickup
            var ecommerceTerminal = invoices.Where(x => x.PickupOptions == PickupOptions.SERVICECENTER && x.CompanyType == ecommerce).Count();

            //Service Centers Revenue By Sales No Service Center Filtering
            var salesPerServiceCenter = await SalesPerServiceCenter(invoice);

            if(filterCriteria.ServiceCentreId > 0)
            {
                //Average Price of shipments leaving that service center
                avgOriginShipmentCostPerSC = revenue / shipmentOrdered;
            }

            //Shipments Created
            createdShipments = invoices.Where(x => x.ShipmentScanStatus == ShipmentScanStatus.CRT).Count();

            //Shipments departed service center
            departedShipments = invoices.Where(x => x.ShipmentScanStatus == ShipmentScanStatus.DSC || x.ShipmentScanStatus == ShipmentScanStatus.DTR || x.ShipmentScanStatus == ShipmentScanStatus.DPC).Count();

            //Total Weight
            var totalWeight = invoices.Sum(x => x.ApproximateItemsWeight);

            //Average Price of Total Shipments
            var avgTotalShipments = revenue / ((shipmentOrdered == 0) ? 1 : shipmentOrdered);

            //Most Shipped Items By Weight
            var weight = await MostShippedItemByWeight(invoices);

            //Average Shipment Cost for only Shipments within Lagos
            int lagosStationId = 4;

            // get the service centre
            var serviceCentres = await _serviceCenterService.GetServiceCentresByStationId(lagosStationId);
            int[] serviceCenterIds = serviceCentres.Select(s => s.ServiceCentreId).ToArray();
            
            var lagosShipments = invoice.Where(s => serviceCenterIds.Contains(s.DepartureServiceCentreId) && serviceCenterIds.Contains(s.DestinationServiceCentreId));
            var sumlagosShipments = lagosShipments.Sum(x => x.GrandTotal);
            var countLagosShipments = lagosShipments.Count();
            var avgLagosShipments = sumlagosShipments / ((countLagosShipments == 0 ? 1 : countLagosShipments));

            result.Revenue = revenue;
            result.ShipmentDelivered = shipmentDeliverd;
            result.ShipmentOrdered = shipmentOrdered;
            result.CorporateRevenue = corporateRevenue;
            result.EcommerceRevenue = ecommerceRevenue;
            result.IndividualRevenue = individualRevenue;
            result.IndividualShipments = individualShipmentCount;
            result.EcommerceShipments = ecommerceShipmentCount;
            result.CorporateShipments = corporateShipmentCount;

            result.AverageShipmentIndividual = avgIndividualCost;
            result.AverageShipmentECommerce = avgEcommerceCost;
            result.AverageShipmentCorporate = avgCorporateCost;

            result.ECommerceHomeDeliveries = ecommerceHome;
            result.ECommerceTerminalPickups = ecommerceTerminal;

            result.HomeDeliveries = homeDeliveries;
            result.TerminalPickups = terminalPickups;

            result.Sales = salesPerServiceCenter;
            result.AvgOriginCostPerServiceCenter = avgOriginShipmentCostPerSC;
            result.AvgDestCostPerServiceCenter = avgDestShipmentCostPerSC;
            result.CreatedShipments = createdShipments;
            result.DepartedShipments = departedShipments;
            result.TotalWeight = totalWeight;
            result.TotalShipmentAvg = avgTotalShipments;
            result.WeightData = weight;
            result.AvgLagosShipment = avgLagosShipments;

            return result;
        }

        private async Task<List<object>> SalesPerServiceCenter(List<InvoiceView> invoice)
        {
            var salesData = await _uow.Invoice.SalesPerServiceCenter(invoice);
            return salesData;
        }

        private async Task<List<object>> MostShippedItemByWeight(List<InvoiceView> invoice)
        {
            var weightData = await _uow.Invoice.MostShippedItemsByWeight(invoice);
            return weightData;
        }

        private async Task<List<Report_MostShippedItemByWeight>> GetMostShippedItemByWeights()
        {
            var result = _uow.Invoice.GetMostShippedItemByWeight().OrderByDescending(x => x.Total).Take(5).ToList();
            return await Task.FromResult(result);
        }

        private async Task<List<Report_RevenuePerServiceCentre>> GetRevenuePerServiceCentres()
        {
            var result = _uow.Invoice.GetRevenuePerServiceCentre().OrderByDescending(x => x.Total).ToList();
            return await Task.FromResult(result);
        }

        private async Task<List<Report_TotalServiceCentreByState>> GetTotalServiceCentreByStates()
        {
            var result = _uow.Invoice.GetTotalServiceCentreByState().OrderByDescending(x => x.TotalService).ToList();
            return await Task.FromResult(result);
        }

        private async Task<Report_TotalOrdersDelivered> GetTotalOrdersDelivered()
        {
            var result = _uow.Invoice.GetTotalOrdersDelivered().FirstOrDefault();
            return await Task.FromResult(result);
        }

        private async Task<int> GetCorporateCount()
        {
            var result = _uow.Company.GetAllAsQueryable().Where(x => x.CompanyType == CompanyType.Corporate).Count();
            return await Task.FromResult(result);
        }

        private async Task<int> GetCorporateCount(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var result = _uow.Company.GetAllAsQueryable().Where(x => x.CompanyType == CompanyType.Corporate
            && (x.DateCreated >= startDate && x.DateCreated < endDate) ).Count();
            return await Task.FromResult(result);
        }

        private async Task<int> GetEcommerceCount()
        {
            var result = _uow.Company.GetAllAsQueryable().Where(x => x.CompanyType == CompanyType.Ecommerce).Count();
            return await Task.FromResult(result);
        }
        private async Task<int> GetEcommerceCount(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
                        
            var result = _uow.Company.GetAllAsQueryable().Where(x => x.CompanyType == CompanyType.Ecommerce
            && (x.DateCreated >= startDate && x.DateCreated < endDate)).Count();
            return await Task.FromResult(result);
        }

        private async Task<int> GetIndividaulCount()
        {
            var result = _uow.IndividualCustomer.GetAllAsQueryable().Select(x => x.PhoneNumber).Distinct().Count();
            return await Task.FromResult(result);
        }
        private async Task<int> GetIndividaulCount(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var result = _uow.IndividualCustomer.GetAllAsQueryable().Where(x => x.DateCreated >= startDate && x.DateCreated < endDate)
                .Select(x => x.PhoneNumber).Distinct().Count();
            return await Task.FromResult(result);
        }
    }
}