using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessage;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.CORE.IServices.Report;
using GIGLS.Infrastructure;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Messaging.MessageService;
using GIGLS.Services.Implementation.Report;
using GIGLS.Services.Implementation.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [RoutePrefix("api/customerinvoice")]
    public class CustomerInvoiceController : BaseWebApiController
    {
        private IShipmentReportService _service;
        private IEmailService _emailService;
        public CustomerInvoiceController(IShipmentReportService service) : base(nameof(CustomerInvoiceController))
        {
            _emailService = new EmailService();
            _service = service;
        }

        [HttpGet]
        [Route("getcustomerinvoice")]
        public async Task<bool> GenerateCustomerInvoice()
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    //setup client
                    string apiBaseUri = "http://localhost/giglsresourceapi/";
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    System.Net.Http.HttpResponseMessage responseMessage = client.GetAsync("api/report/getcustomerinvoice").Result;

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Error calling your.0 api: ");
                    }
                }

                return true;

            }

            catch (Exception ex)
            {

                throw new GenericException("Error calling your.0 api: " + ex.ToString());
            }

        }
    }
}
