using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/invoice")]
    public class InvoiceController : BaseWebApiController
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService) : base(nameof(InvoiceController))
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddInvoice(InvoiceDTO invoiceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _invoiceService.AddInvoice(invoiceDto);

                return new ServiceResponse<object>
                {
                    Object = invoice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{invoiceId:int}")]
        public async Task<IServiceResponse<InvoiceDTO>> GetInvoice(int invoiceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _invoiceService.GetInvoiceById(invoiceId);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("bywaybill")]
        public async Task<IServiceResponse<InvoiceDTO>> GetInvoiceByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _invoiceService.GetInvoiceByWaybill(waybill);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{invoiceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteInvoice(int invoiceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _invoiceService.RemoveInvoice(invoiceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{invoiceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateInvoice(int invoiceId, InvoiceDTO invoiceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _invoiceService.UpdateInvoice(invoiceId, invoiceDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
