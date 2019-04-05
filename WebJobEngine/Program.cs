using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            callReprintJob();
        }

        private async static Task<JObject> callReprintJob()
        {
            string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
            apiBaseUri = apiBaseUri + "api/webjobs/runreprintexpirycounter";

            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //call the api for run
                HttpResponseMessage responseMessage = await client.GetAsync(apiBaseUri);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("There are bugs that require fixes in your code:");
                }

                //get access token from response body
                var responseJson = responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson.ToString());

                return jObject;
            }
        }
    }
}
