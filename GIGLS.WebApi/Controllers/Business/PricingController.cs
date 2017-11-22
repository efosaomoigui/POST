﻿using GIGLS.Core.IServices;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Business
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/pricing")]
    public class PricingController : BaseWebApiController
    {
        private readonly IPricingService _pricing;

        public PricingController(IPricingService pricingService) : base(nameof(PricingController))
        {
            _pricing = pricingService;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<decimal>> GetPrice(PricingDTO pricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _pricing.GetPrice(pricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }
    }
}
