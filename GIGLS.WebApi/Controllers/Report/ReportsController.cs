using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
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
    }
}
