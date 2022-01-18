using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.Wallet;
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
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        public TicketMannService(IUnitOfWork uow, IWalletService walletService)
        {
            _uow = uow;
            _walletService = walletService;
        }
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

        public async Task<string> BillTransactionRefund(string emailOrCode, decimal amount)
        {
            var response = "";
            if (string.IsNullOrWhiteSpace(emailOrCode))
            {
                throw new GenericException("Please provide valid email or customer code", $"{(int)HttpStatusCode.Forbidden}");
            }

            if (emailOrCode.StartsWith("2012GIGL"))
            {
                var listWatrans = _uow.WalletTransaction.GetAllAsQueryable().Where(x => x.CreditDebitType == CreditDebitType.Debit && x.PaymentTypeReference.Equals(emailOrCode)).ToList();
                var walletTrans = listWatrans.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                if (walletTrans is null)
                {
                    throw new GenericException("Wallet transaction does not exit", $"{(int)HttpStatusCode.BadRequest}");
                }

                //Call Ticket Mann
                var ticketMannResponse = await GetBillTransaction(walletTrans.PaymentTypeReference);

                if (ticketMannResponse is null)
                {
                    throw new GenericException("Bill transaction does not exit", $"{(int)HttpStatusCode.BadRequest}");
                }

                if (ticketMannResponse.Payload.Status != null && ticketMannResponse.Payload.Status.Contains("Complete"))
                {
                    response = "Transaction was successful";
                }
                else if (ticketMannResponse.Payload.Status != null && ticketMannResponse.Payload.Status.Contains("Failed"))
                {
                    if (string.IsNullOrWhiteSpace(walletTrans.PaymentTypeReference))
                    {
                        throw new GenericException($"Transaction reference cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    var result = await _walletService.ReverseWallet(walletTrans.PaymentTypeReference);
                    response = result.Message;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(walletTrans.PaymentTypeReference))
                    {
                        throw new GenericException($"Transaction reference cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    var result = await _walletService.ReverseWallet(walletTrans.PaymentTypeReference);
                    response = result.Message;
                }
            }
            else
            {
                if (amount <= 0)
                {
                    throw new GenericException("Please provide a valid amount", $"{(int)HttpStatusCode.Forbidden}");
                }

                var userDetails = await _uow.User.GetUserByEmailorCustomerCode(emailOrCode);
                if (userDetails is null)
                {
                    throw new GenericException("User does not exit", $"{(int)HttpStatusCode.BadRequest}");
                }

                //Get Last User Wallet transaction
                var listWatrans = _uow.WalletTransaction.GetAllAsQueryable().Where(x => x.UserId.Equals(userDetails.Id) && x.CreditDebitType == CreditDebitType.Debit && x.PaymentTypeReference.Contains("2012GIGL") && x.Amount == amount).ToList();
                var walletTrans = listWatrans.OrderByDescending(x => x.DateCreated).FirstOrDefault();

                if (walletTrans is null)
                {
                    throw new GenericException("Wallet transaction does not exit", $"{(int)HttpStatusCode.BadRequest}");
                }

                //Call Ticket Mann
                var ticketMannResponse = await GetBillTransaction(walletTrans.PaymentTypeReference);

                if (ticketMannResponse is null)
                {
                    throw new GenericException("Bill transaction does not exit", $"{(int)HttpStatusCode.BadRequest}");
                }

                if (ticketMannResponse.Payload.Status != null && ticketMannResponse.Payload.Status.Contains("Complete"))
                {
                    response = "Transaction was successful";
                }
                else if (ticketMannResponse.Payload.Status != null && ticketMannResponse.Payload.Status.Contains("Failed"))
                {
                    if (string.IsNullOrWhiteSpace(walletTrans.PaymentTypeReference))
                    {
                        throw new GenericException($"Transaction reference cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    var result = await _walletService.ReverseWallet(walletTrans.PaymentTypeReference);
                    response = result.Message;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(walletTrans.PaymentTypeReference))
                    {
                        throw new GenericException($"Transaction reference cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    var result = await _walletService.ReverseWallet(walletTrans.PaymentTypeReference);
                    response = result.Message;
                }
            }

            return response;
        }

        private async Task<TicketMannResponseDTO> GetBillTransaction(string reference)
        {
            try
            {
                var ticketMannResponse = new TicketMannResponseDTO();
                // Call ticket mann
                var url = ConfigurationManager.AppSettings["TicketMannUrl"];
                url = $"{url}{reference}";

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    string result = await response.Content.ReadAsStringAsync();
                    var jObject = JsonConvert.DeserializeObject<TicketMannResponseDTO>(result);
                    ticketMannResponse = jObject;
                }

                return ticketMannResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
