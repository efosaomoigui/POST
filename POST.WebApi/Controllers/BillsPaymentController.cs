using POST.Core.DTO;
using POST.Core.IServices;
using POST.CORE.DTO.Report;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/billspayment")]
    public class BillsPaymentController : BaseWebApiController
    {
        private readonly ITicketMannService _service;

        public BillsPaymentController(ITicketMannService service) : base(nameof(BillsPaymentController))
        {
            _service = service;
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("report")]
        public async Task<IServiceResponse<MerchantSalesPayload>> GetBillsPaymentSummary(DateFilterCriteria searchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _service.GetMerchantSalesSummary(searchDTO);

                return new ServiceResponse<MerchantSalesPayload>
                {
                    Object = response.Payload.FirstOrDefault()
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("customertransactions")]
        public async Task<IServiceResponse<CustomerTransactionsPayload>> GetBillsPaymentSummaryForCustomerTransactions(DateFilterCriteria searchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _service.GetCustomerTransactionsSummary(searchDTO);

                return new ServiceResponse<CustomerTransactionsPayload>
                {
                    Object = response.Payload
                };
            });
        }

        [HttpPut]
        [Route("{emailorcode}/{amount}/billtransactionrefund")]
        public async Task<IServiceResponse<string>> BillTransactionRefund(string emailorcode, decimal amount)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _service.BillTransactionRefund(emailorcode, amount);
                return new ServiceResponse<string>
                {
                    Object = result
                };
            });
        }
    }
}
