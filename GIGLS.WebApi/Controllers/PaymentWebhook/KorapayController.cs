using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GIGLS.WebApi.Controllers.PaymentWebhook
{
    //[Authorize]
    [RoutePrefix("api/korapay")]
    public class KorapayController : BaseWebApiController
    {
        // GET: Korapay
        private readonly IKorapayPaymentService _korapayService;

        public KorapayController(IKorapayPaymentService korapayService) : base(nameof(KorapayController))
        {
            _korapayService = korapayService;
        }

        [HttpPost]
        [Route("encrypt")]
        public async Task<IServiceResponse<string>> EncryptData(KorapayWebhookDTO payload)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await  _korapayService.Encrpt(payload);

                return new ServiceResponse<string>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("initializecharge")]
        public async Task<IServiceResponse<string>> InitializeCharge(KoarapayInitializeCharge payload)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _korapayService.InitializeCharge(payload);

                return new ServiceResponse<string>
                {
                    Object = result
                };
            });
        }
    }
}