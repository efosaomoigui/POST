using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class TicketMannService : ITicketMannService
    {
        public async Task<string> GetToken()
        {
            string getTokenResponse = "";
            using (var client = new HttpClient())
            {
                //Get login details
                var tokenUrl = ConfigurationManager.AppSettings["TicketMannBaseUrl"];
                var username = ConfigurationManager.AppSettings["TicketMannUsername"];
                var password = ConfigurationManager.AppSettings["TicketMannPassword"];
                tokenUrl = $"{tokenUrl}/token";

                //setup client
                client.BaseAddress = new Uri(tokenUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                         new KeyValuePair<string, string>("grant_type", "password"),
                         new KeyValuePair<string, string>("Username", username),
                         new KeyValuePair<string, string>("Password", password),
                 });

                //setup login data

                HttpResponseMessage responseMessage = await client.PostAsync("Token", formContent);
                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);

                getTokenResponse = jObject.GetValue("access_token").ToString();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete login successfully:");
                }

                return getTokenResponse;
            }
        }

        public async Task<MerchantSalesDTO> GetMerchantSalesSummary(DateFilterCriteria filter)
        {
            MerchantSalesDTO result = new MerchantSalesDTO();

            if (filter.StartDate == null)
            {
                throw new GenericException("Start date is required", $"{(int)HttpStatusCode.Forbidden}");
            }

            if (filter.EndDate == null)
            {
                throw new GenericException("End date is required", $"{(int)HttpStatusCode.Forbidden}");
            }

            string token = await GetToken();
            using (var client = new HttpClient())
            {
                //Get login details
                var url = ConfigurationManager.AppSettings["TicketMannBaseUrl"];
                url = $"{url}/MerchantSales/GetGroupedSalesbyMerchantIdByRange?startDate={filter.StartDate.Value.ToString("yyyy-MM-dd")}&endDate={filter.EndDate.Value.ToString("yyyy-MM-dd")}";

                //setup client
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);
                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<MerchantSalesDTO>(resultJson);
                result = jObject;

                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                return result;
            }
        }
    }
}
