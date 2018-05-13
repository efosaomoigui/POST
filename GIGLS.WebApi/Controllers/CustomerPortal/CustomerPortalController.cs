using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.CORE.DTO.Report;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    //[Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/portal")]
    public class CustomerPortalController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService; 
        
        public CustomerPortalController(ICustomerPortalService portalService) : base(nameof(CustomerPortalController))
        {
            _portalService = portalService;
        }
        
        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("transaction")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _portalService.GetShipmentTransactions(f_Criteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }
        
        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("wallet")]
        public async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactionSummary = await _portalService.GetWalletTransactions(); 

                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = walletTransactionSummary
                };
            });
        }
        
        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("invoice")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetInvoices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoices();

                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = invoice
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("bywaybill/{waybill}")]
        public async Task<IServiceResponse<InvoiceDTO>> GetInvoiceByWaybill([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoiceByWaybill(waybill);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> TrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.TrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("cod")]
        public async Task<IServiceResponse<CashOnDeliveryAccountSummaryDTO>> GetCashOnDeliveryAccount()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.GetCashOnDeliveryAccount();

                return new ServiceResponse<CashOnDeliveryAccountSummaryDTO>
                {
                    Object = result
                };
            });
        }
    }
}
