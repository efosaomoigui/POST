﻿using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

    }
}