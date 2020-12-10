using System.IO;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System;
using GIGLS.Core.DTO.ServiceCentres;

namespace ApiProcessor
{
    public class ApiCaller
    {
        //Job to remind about due invoices
        public  static JObject callAPI(string method, string apiurlsegment,  TextWriter log)
        {
            string apiBaseUri1 = ConfigurationManager.AppSettings["WebApiUrl"];
            var apiurl = apiBaseUri1 + apiurlsegment;

            log.WriteLine("URL " + apiurl);

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
            log.WriteLine("Operation done successfully "+ apiurl);

            return jObject;

        }

        public  static JObject callVoidAPI(string method, string apiurlsegment, TextWriter log) 
        {
            string apiBaseUri1 = ConfigurationManager.AppSettings["WebApiUrl"];
            var apiurl = apiBaseUri1 + apiurlsegment;

            log.WriteLine("URL " + apiurl);

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
            log.WriteLine("Operation done successfully " + apiurl);
            return jObject;
        }

        internal static object callAPI(string v1, string v2, string sCs)
        {
            throw new NotImplementedException();
        }
    }
}
