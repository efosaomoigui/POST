using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.View;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace GIGLS.WebApi.Controllers.Account
{
    public class ReminderController : BaseWebApiController
    {

        private readonly IInvoiceService _invoiceService;
        public ReminderController(IInvoiceService invoiceService) : base(nameof(ReminderController))
        {
            _invoiceService = invoiceService;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
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


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ReminderInvoices/{datetoduedate}")]
        public async Task<IServiceResponse<IEnumerable<InvoiceView>>> ReminderInvoices([FromUri] double datetoduedate)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _invoiceService.GetInvoicesForReminder(datetoduedate);

                return new ServiceResponse<IEnumerable<InvoiceView>>
                {
                    Object = result  
                };
            });
        }
    }
}