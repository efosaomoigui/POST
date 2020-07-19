using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.Website;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [AllowAnonymous]
    [RoutePrefix("api/webtracking")]
    public class PublicTrackingController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;
        private readonly IWebsiteService _websiteService;

        public PublicTrackingController(ICustomerPortalService portalService, IWebsiteService websiteService) : base(nameof(PublicTrackingController))
        {
            _portalService = portalService;
            _websiteService = websiteService;
        }

        [HttpGet]
        [Route("reportsummary")]
        public async Task<IServiceResponse<AdminReportDTO>> GetWebsiteData()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var data = await _portalService.WebsiteData();
                return new ServiceResponse<AdminReportDTO>
                {
                    Object = data

                };
            });
        }

        [HttpGet]
        [Route("track/{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> PublicTrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("schedulePickup")]
        public async Task<IServiceResponse<bool>> SendSchedulePickupMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendSchedulePickupMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("requestQuote")]
        public async Task<IServiceResponse<bool>> SendRequestQuoteMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendQuoteMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("trackshipment/{waybillNumber}")]
        public async Task<IServiceResponse<MobileShipmentTrackingHistoryDTO>> TrackMobileShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.trackShipment(waybillNumber);

                return new ServiceResponse<MobileShipmentTrackingHistoryDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("reportIssues")]
        public async Task<IServiceResponse<bool>> SendGIGGoIssuesMail(AppMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendGIGGoIssuesMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("giggopresentdayshipments")]
        public async Task<IServiceResponse<List<LocationDTO>>> GetPresentDayShipmentLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipment = await _portalService.GetPresentDayShipmentLocations();
                return new ServiceResponse<List<LocationDTO>>
                {
                    Object = preshipment
                };
            });
        }

        [HttpGet]
        [Route("getwaybill/{waybillNumber}")]
        public async Task<IServiceResponse<ShipmentDetailDanfoDTO>> GetShipmentDetailForDanfo(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.GetShipmentDetailForDanfo(waybillNumber);

                return new ServiceResponse<ShipmentDetailDanfoDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("ecommerceagreement")]
        public async Task<IServiceResponse<object>> AddEcommerceAgreement(EcommerceAgreementDTO ecommerceAgreementDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<object>();
                var request = Request;
                var headers = request.Headers;
                var result = new object();
                if (headers.Contains("api_key"))
                {
                    
                    var key = await _portalService.EncryptWebsiteKey();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        result = await _websiteService.AddEcommerceAgreement(ecommerceAgreementDTO);
                        response.Object = result;
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }
    }
}
