using GIGLS.Core;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.IServices.Report;
using GIGLS.Core.View.AdminReportView;
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

        public AdminReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AdminReportDTO> GetAdminReport()
        {
            var result = new AdminReportDTO();

            result.AllTimeSalesByCountry = await GetAllTimeSalesByCountries();
            result.BusiestRoute = await GetBusiestRoutes();
            result.CustomerRevenue = await GetCustomerRevenues();
            result.MostShippedItemByWeight = await GetMostShippedItemByWeights();
            result.RevenuePerServiceCentre = await GetRevenuePerServiceCentres();
            result.TotalServiceCentreByState = await GetTotalServiceCentreByStates();
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
    }
}