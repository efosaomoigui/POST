using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.View;

namespace GIGLS.WebApi.Controllers.Account
{
    [RoutePrefix("api/invoice")]
    public class InvoiceReminderController : BaseWebApiController
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceReminderController(IInvoiceService invoiceService) : base(nameof(InvoiceController)) 
        {
            _invoiceService = invoiceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<InvoiceDTO>>> GetInvoices()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var invoice = await _invoiceService.GetInvoices();

                return new ServiceResponse<IEnumerable<InvoiceDTO>>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("sendemailfordueinvoices")]
        public async Task<IServiceResponse<string>> SendEmailForDueInvoices([FromUri] int datetoduedate)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _invoiceService.SendEmailForDueInvoices(datetoduedate);

                return new ServiceResponse<string>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("sendemailForwalletbalancecheck")]
        public async Task<IServiceResponse<string>> SendEmailForWalletBalanceCheck([FromUri] decimal amountBalance)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _invoiceService.SendEmailForWalletBalanceCheck(amountBalance);

                return new ServiceResponse<string>
                {
                    Object = result
                };
            });
        }

    }
}
