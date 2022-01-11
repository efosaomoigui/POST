﻿using GIGLS.Core.DTO;
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

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete login successfully:");
                }
                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);

                getTokenResponse = jObject.GetValue("access_token").ToString();

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

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<MerchantSalesDTO>(resultJson);
                result = jObject;


                return result;
            }
        }

        public async Task<CustomerTransactionsDTO> GetCustomerTransactionsSummary(DateFilterCriteria filter)
        {
            CustomerTransactionsDTO result = new CustomerTransactionsDTO();

            if (filter.StartDate == null)
            {
                throw new GenericException("Start date is required", $"{(int)HttpStatusCode.Forbidden}");
            }

            if (filter.EndDate == null)
            {
                throw new GenericException("End date is required", $"{(int)HttpStatusCode.Forbidden}");
            }

            CustomerTransactionFilter payload = new CustomerTransactionFilter
            {
                StartDate = filter.StartDate.Value.ToString("yyyy-MM-dd"),
                EndDate = filter.EndDate.Value.ToString("yyyy-MM-dd")
            };

            string token = await GetToken();
            using (var client = new HttpClient())
            {
                //Get login details
                var url = ConfigurationManager.AppSettings["TicketMannBaseUrl"];
                var customerTransactions = ConfigurationManager.AppSettings["TicketMannCustomerTransactions"];
                url = $"{url}/{customerTransactions}";

                //setup client
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Convert payload to string content / serialize
                var json = JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, data);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var jObject = JsonConvert.DeserializeObject<CustomerTransactionsDTO>(resultJson);
                result = jObject;

                if (result.Payload.Transactions.Any())
                {
                    var filterList = ConfigurationManager.AppSettings["TicketMannFilterList"];
                    if (filterList != null)
                    {
                        result.Payload.Transactions = result.Payload.Transactions.Where(x => !string.IsNullOrWhiteSpace(x.TransactionStatus) && filterList.Contains(x.TransactionStatus)).OrderByDescending(x => x.DateofTransaction).ToList();
                    }
                }
                return result;
            }
        }
    }
}
