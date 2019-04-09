using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebJobEngine
{
    class Program
    {
        public static void Main(string[] args)
        {
            callReprintJob("GET", "api/webjobs/runreprintexpirycounter");
        }

        private async static Task<JObject> callReprintJob(string method, string apiurlsegment)
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
            return jObject;

        }
    }
}
