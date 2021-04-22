using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IServices;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.IServices.Report;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Report
{
    [Authorize(Roles = "Report, ViewAdmin")]
    [RoutePrefix("api/report")]
    public class ReportsController : BaseWebApiController
    {
        private readonly IShipmentReportService _shipmentService;
        private readonly IAccountReportService _accountService;

        public ReportsController(IShipmentReportService shipmentService, IAccountReportService accountService) : base(nameof(ReportsController))
        {
            _shipmentService = shipmentService;
            _accountService = accountService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("todayshipments")]
        public async Task<IServiceResponse<List<ShipmentDTO>>> GetTodayShipments()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var shipments = await _shipmentService.GetTodayShipments();

                return new ServiceResponse<List<ShipmentDTO>>
                {
                    Object = shipments
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("shipments")]
        public async Task<IServiceResponse<List<ShipmentDTO>>> GetShipments(ShipmentFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _shipmentService.GetShipments(filterCriteria);

                return new ServiceResponse<List<ShipmentDTO>>
                {
                    Object = shipments
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("income")]
        public async Task<IServiceResponse<List<GeneralLedgerDTO>>> GetIncome(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var incomes = await _accountService.GetIncomeReports(accountFilterCriteria);

                return new ServiceResponse<List<GeneralLedgerDTO>>
                {
                    Object = incomes
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("demurage")]
        public async Task<IServiceResponse<List<GeneralLedgerDTO>>> GetDemurage(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var incomes = await _accountService.GetDemurageReports(accountFilterCriteria);

                return new ServiceResponse<List<GeneralLedgerDTO>>
                {
                    Object = incomes
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("expenditure")]
        public async Task<IServiceResponse<List<GeneralLedgerDTO>>> GetExpenditure(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var expenditures = await _accountService.GetExpenditureReports(accountFilterCriteria);

                return new ServiceResponse<List<GeneralLedgerDTO>>
                {
                    Object = expenditures
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("invoice")]
        public async Task<IServiceResponse<List<InvoiceDTO>>> GetInvoice(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _accountService.GetInvoiceReports(accountFilterCriteria);

                return new ServiceResponse<List<InvoiceDTO>>
                {
                    Object = invoices
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("invoiceFromView")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetInvoiceFromView(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _accountService.GetInvoiceReportsFromView(accountFilterCriteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices 
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("invoiceFromViewWithDeliverTime")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> invoiceFromViewWithDeliverTime(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _accountService.GetInvoiceReportsFromViewPlusDeliveryTime(accountFilterCriteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("customershipments")]
        public async Task<IServiceResponse<List<ShipmentDTO>>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var shipments = await _shipmentService.GetCustomerShipments(f_Criteria);

                return new ServiceResponse<List<ShipmentDTO>>
                {
                    Object = shipments
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("scanstatusFromView")]
        public async Task<IServiceResponse<List<ScanStatusReportDTO>>> GetScanStatusFromView(ScanTrackFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentTrackings = await _shipmentService.GetShipmentTrackingFromViewReport(f_Criteria);

                return new ServiceResponse<List<ScanStatusReportDTO>>
                {
                    Object = shipmentTrackings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("shipmentprogresssummary")]
        public async Task<IServiceResponse<DashboardDTO>> GetShipmentProgressSummary(ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var summary = await _shipmentService.GetShipmentProgressSummary(baseFilterCriteria);

                return new ServiceResponse<DashboardDTO>
                {
                    Object = summary
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("shipmentprogresssummarybreakdown")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentProgressSummaryBreakDown(ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var summary = await _shipmentService.GetShipmentProgressSummaryBreakDown(baseFilterCriteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = summary
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("giggoshipment")]
        public async Task<IServiceResponse<List<PreShipmentMobileReportDTO>>> PreShipmentMobileReport(MobileShipmentFilterCriteria baseFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var summary = await _shipmentService.GetPreShipmentMobile(baseFilterCriteria);

                return new ServiceResponse<List<PreShipmentMobileReportDTO>>
                {
                    Object = summary
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("earningsbreakdown")]
        public async Task<IServiceResponse<EarningsBreakdownDTO>> GetEarningsBreakdown(DashboardFilterCriteria dashboardFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var earnings = await _accountService.GetEarningsBreakdown(dashboardFilter);

                return new ServiceResponse<EarningsBreakdownDTO>
                {
                    Object = earnings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("financialreport")]
        public async Task<IServiceResponse<List<FinancialReportDTO>>> GetFinancialBreakdownByType(AccountFilterCriteria accountFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var financialreport = await _accountService.GetFinancialBreakdownByType(accountFilter);

                return new ServiceResponse<List<FinancialReportDTO>>
                {
                    Object = financialreport
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("walletfundingbreakdown")]
        public async Task<IServiceResponse<List<WalletPaymentLogView>>> GetWalletPaymentLogBreakdown(DashboardFilterCriteria dashboardFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var report = await _accountService.GetWalletPaymentLogBreakdown(dashboardFilter);

                return new ServiceResponse<List<WalletPaymentLogView>>
                {
                    Object = report
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("outboundfinancialreport")]
        public async Task<IServiceResponse<List<OutboundFinancialReportDTO>>> GetFinancialBreakdownOfOutboundShipments(AccountFilterCriteria accountFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var financialreport = await _accountService.GetFinancialBreakdownOfOutboundShipments(accountFilter);

                return new ServiceResponse<List<OutboundFinancialReportDTO>>
                {
                    Object = financialreport
                };
            });
        }
    }
}