using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation.Account;
using GIGLS.Messaging.MessageService;
using GIGLS.Core.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace ReminderJobs
{
    public class Functions
    {

        //Job to remind about due invoices
        public async static void InvoiceReminderJob( string message, TextWriter log)
        {

            JObject invoiceview = null;

            //make a call to the reminder invoices services to collect all close to due invoices
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/giglsresourceapi/api/reminderinvoices");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            var urlParameters = "";

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            JToken invoices = null;


            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var responseJson = await response.Content.ReadAsStringAsync();
                invoiceview = JObject.Parse(responseJson);
                invoices = invoiceview.GetValue("Object");
            }


            //create email obj for sending the messages
            EmailService _emailservice = new EmailService();
            int countmessages = 0;

            foreach (var invoice in invoices)
            {
                //prepare the message for reminder
                MessageDTO messageDto = new MessageDTO();
                await _emailservice.SendAsync(messageDto);
                countmessages += countmessages;
            }

            //var invoiceTuple = _invoiceservice.GetInvoices(filterOptionsDto);
            log.WriteLine("Sent a total number of "+ countmessages + " messages");
        }


        //Job to remind about wallet balances
        public static void WalletReminderJob(string message, TextWriter log) 
        {
            log.WriteLine(message);
        }
    }
}
