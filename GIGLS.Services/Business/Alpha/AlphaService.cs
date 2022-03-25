using GIGLS.Core.DTO.Alpha;
using GIGLS.Core.IServices.Alpha;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Alpha
{
    public class AlphaService : IAlphaService
    {
        public async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {
                var result = "";
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["AlphaBaseUrl"];
                var login = ConfigurationManager.AppSettings["AlphaLogin"];
                baseurl = $"{baseurl}{login}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new AlphaUserLoginDTO
                {
                    UserType = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AlphaUser_type"]) ? " " : ConfigurationManager.AppSettings["AlphaUser_type"],
                    Email = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AlphaUsername"]) ? " " : ConfigurationManager.AppSettings["AlphaUsername"],
                    Password = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AlphaPassword"]) ? " " : ConfigurationManager.AppSettings["AlphaPassword"],
                };
                
                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<AlphaLoginResponseDTO>(resultJson);
                result = jObject.Results.Token;

                return result;
            }
        }

        public async Task<bool> UpdateUserSubscription(AlphaSubscriptionUpdateDTO payload)
        {
            bool result = false;
            string token = await GetToken();
            using (var client = new HttpClient())
            {
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["AlphaBaseUrl"];
                var subscriptionUpdate = ConfigurationManager.AppSettings["AlphaSubscriptionUpdate"];
                baseurl = $"{ baseurl}{subscriptionUpdate}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                if(response.IsSuccessStatusCode == true)
                {
                    result = true;
                }
                
                return result;
            }
        }

        public async Task<bool> UpdateOrderStatus(AlphaUpdateOrderStatusDTO payload)
        {
            bool result = false;
            string token = await GetToken();
            using (var client = new HttpClient())
            {
                //Get login details
                var baseurl = ConfigurationManager.AppSettings["AlphaOrderUrl"];
                var subscriptionUpdate = ConfigurationManager.AppSettings["AlphaOrderUpdateStatus"];
                baseurl = $"{ baseurl}{subscriptionUpdate}";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseurl, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                if (response.IsSuccessStatusCode == true)
                {
                    result = true;
                }

                return result;
            }
        }
    }
}
