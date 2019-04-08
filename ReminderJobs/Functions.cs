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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace ReminderJobs
{
    public class Functions
    {

        //Job to remind about due invoices
        public async static Task<JObject> ReminderJob(string message, TextWriter log, string method, string apiurlsegment)
        {
            string apiBaseUri1 = ConfigurationManager.AppSettings["WebApiUrl"];
            var apiurl = apiBaseUri1 + apiurlsegment;

            WebRequest requestObject = WebRequest.Create(apiurl);
            requestObject.Method = method;
            HttpWebResponse response = (HttpWebResponse)requestObject.GetResponse();
            string result = "";

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                sr.Close();
            }
            var jObject = JObject.Parse(result);

            //var invoiceTuple = _invoiceservice.GetInvoices(filterOptionsDto);
            log.WriteLine("Operation done successfully");

            return jObject;

        }
        

        //Job to remind about wallet balances
        public static void WalletReminderJob(string message, TextWriter log) 
        {
            log.WriteLine(message);
        }
    }
}
