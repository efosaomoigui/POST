using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [RoutePrefix("api/firebase")]
    public class FirebaseAccessController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public FirebaseAccessController(ICustomerPortalService portalService) : base(nameof(FirebaseAccessController))
        {

            _portalService = portalService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("sendpickuprequestmessage/{userid}")]
        public async Task<IServiceResponse<bool>> SendPickUpRequestMessage([FromUri] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        await _portalService.SendPickUpRequestMessage(userId);
                        response.Object = true;
                    }
                }
                return response;
            });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("addmobilepickuprequestfortimedoutrequests")]
        public async Task<IServiceResponse<bool>> AddPickupRequestForTimedOutRequest([FromBody] MobilePickUpRequestsDTO PickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var shipmentItem = await _portalService.AddMobilePickupRequest(PickupRequest);
                        response.Object = true;
                    }
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("shipmentreassignment")]
        public async Task<IServiceResponse<bool>> ChangeShipmentOwnershipForPartner(PartnerReAssignmentDTO pickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var shipmentItem = await _portalService.ChangeShipmentOwnershipForPartner(pickupRequest);
                        response.Object = true;
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
