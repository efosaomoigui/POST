using GIGL.POST.Core.Domain;
using POST.Core;
using POST.Core.DTO;
using POST.Core.DTO.Account;
using POST.Core.Enums;
using POST.Core.IMessage;
using POST.Core.IServices.ServiceCentres;
using POST.Core.IServices.User;
using POST.Core.IServices.Utility;
using POST.CORE.IServices.Report;
using POST.Infrastructure;
using POST.Infrastructure.Persistence;
using POST.Messaging.MessageService;
using POST.Services.Implementation.Report;
using POST.Services.Implementation.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.CustomerInvoice
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
                    string apiBaseUri = ""; // "http://localhost/giglsresourceapi/";
                   // string apiBaseUri = "https://api.gigagilitysystems.com/";
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.Http.HttpResponseMessage responseMessage = client.GetAsync("api/report/getcustomerinvoice").Result;
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return false;
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
