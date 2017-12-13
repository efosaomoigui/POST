using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/cashondeliveryaccount")]
    public class CashOnDeliveryAccountController : BaseWebApiController
    {
        public CashOnDeliveryAccountController() : base(nameof(CashOnDeliveryAccountController))
        {
        }
    }
}
