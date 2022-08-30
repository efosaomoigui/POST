using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
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

namespace GIGLS.Services.Implementation.Wallet
{
   public  class AzapayPaymentService : IAzapayPaymentService
    {
        private readonly IUnitOfWork _uow;
        public AzapayPaymentService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
        public async Task<bool> AddAzaPayTransferDetails(AzapayTransferDetailsDTO transferDetailsDTO)
        {
            try
            {
                if (transferDetailsDTO is null)
                {
                    throw new GenericException("invalid payload", $"{(int)HttpStatusCode.BadRequest}");
                }

                var entity = await _uow.TransferDetails.ExistAsync(x => x.RefId == transferDetailsDTO.RefId);
                if (entity)
                {
                    throw new GenericException($"This transfer details with RefId {transferDetailsDTO.RefId} already exist.", $"{(int)HttpStatusCode.Forbidden}");
                }

                if (transferDetailsDTO.Status == "CONFIRMED")
                {
                    transferDetailsDTO.TransactionStatus = "success";
                }
                else
                {
                    transferDetailsDTO.TransactionStatus = "pending";
                }

                transferDetailsDTO.ProcessingPartner = ProcessingPartnerType.Azapay;
                var transferDetails = Mapper.Map<TransferDetails>(transferDetailsDTO);
                transferDetails.PaymentReference = transferDetailsDTO.RefId;
                _uow.TransferDetails.Add(transferDetails);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ValidateTimedAccountResponseDTO> ValidateTimedAccountRequest(string accountnumber)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                throw new GenericException("Invalid account number", $"{(int)HttpStatusCode.BadRequest}");
            }

            return await ValidateTimedAccount(accountnumber);
        }

        public async Task<GetTransactionHistoryResponseDTO> GetTransactionHistoryRequest()
        {
            return await GetTransactionHistory();
        }

        public async Task<InitiateTimedAccountResponseDTO> InitiateTimedAccountRequest(InitiateTimedAccountRequestDTO payload)
        {
            if (payload == null)
            {
                throw new GenericException("Invalid request payload", $"{(int)HttpStatusCode.BadRequest}");
            }
            return await InitiateTimedAccount(payload);
        }

        public async Task<AzapayTransferResponseDTO> AzapayTransferRequest(AzapayTransferRequestDTO payload)
        {
            if (payload == null)
            {
                throw new GenericException("Invalid request payload", $"{(int)HttpStatusCode.BadRequest}");
            }
            return await SendAzapayTransferRequest(payload);
        }

        private async Task<ValidateTimedAccountResponseDTO> ValidateTimedAccount(string accountnumber)
        {
            ValidateTimedAccountResponseDTO result = new ValidateTimedAccountResponseDTO();
            string token =  GetAccessKey();
            using (var client = new HttpClient())
            {
                //Set up url
                var baseurl = ConfigurationManager.AppSettings["AzapayBaseUrl"];
                var validateTimedAccountUrl = ConfigurationManager.AppSettings["AzapayValidateTimedAccount"];
                baseurl = $"{baseurl}{validateTimedAccountUrl}{accountnumber}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Access-key", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(baseurl);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();
                var status = JObject.Parse(resultJson)["status"].ToString();

                if(status != "200")
                {
                    var message = JObject.Parse(resultJson)["message"].ToString();
                    throw new GenericException(message);
                }

                result = JsonConvert.DeserializeObject<ValidateTimedAccountResponseDTO>(resultJson);

                return result;
            }
        }

        private async Task<GetTransactionHistoryResponseDTO> GetTransactionHistory()
        {
            GetTransactionHistoryResponseDTO result = new GetTransactionHistoryResponseDTO();
            string token = GetAccessKey();
            using (var client = new HttpClient())
            {
                //Set up url
                var baseurl = ConfigurationManager.AppSettings["AzapayBaseUrl"];
                var history = ConfigurationManager.AppSettings["AzapayGetTransactionHistory"];
                baseurl = $"{baseurl}{history}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Access-key", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(baseurl);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new GenericException("Operation could not complete successfully:");
                }

                string resultJson = await response.Content.ReadAsStringAsync();

                var status = JObject.Parse(resultJson)["status"].ToString();

                if (status != "200")
                {
                    var message = JObject.Parse(resultJson)["message"].ToString();
                    throw new GenericException(message);
                }

                result = JsonConvert.DeserializeObject<GetTransactionHistoryResponseDTO>(resultJson);

                return result;
            }
        }

        private async Task<InitiateTimedAccountResponseDTO> InitiateTimedAccount(InitiateTimedAccountRequestDTO payload)
        {
            var officialEmail = ConfigurationManager.AppSettings["AzapayEmail"];
            payload.CustomerEmail = string.IsNullOrEmpty(payload.CustomerEmail) ? officialEmail : payload.CustomerEmail;

            InitiateTimedAccountResponseDTO result = new InitiateTimedAccountResponseDTO();
            string token = GetAccessKey();
            using (var client = new HttpClient())
            {
                //Set up url
                var baseurl = ConfigurationManager.AppSettings["AzapayBaseUrl"];
                var initiateAccount = ConfigurationManager.AppSettings["AzapayInitiateTimedAccount"];
                baseurl = $"{baseurl}{initiateAccount}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Access-key", token);
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

                string resultJson = await response.Content.ReadAsStringAsync();

                var status = JObject.Parse(resultJson)["status"].ToString();

                if (status != "200")
                {
                    var message = JObject.Parse(resultJson)["message"].ToString();
                    throw new GenericException(message);
                }

                result = JsonConvert.DeserializeObject<InitiateTimedAccountResponseDTO>(resultJson);

                return result;
            }
        }

        private async Task<AzapayTransferResponseDTO> SendAzapayTransferRequest(AzapayTransferRequestDTO payload)
        {
            AzapayTransferResponseDTO result = new AzapayTransferResponseDTO();
            string token = GetAccessKey();
            using (var client = new HttpClient())
            {
                //Set up url
                var baseurl = ConfigurationManager.AppSettings["AzapayBaseUrl"];
                var transfer = ConfigurationManager.AppSettings["AzapayTransfer"];
                baseurl = $"{baseurl}{transfer}";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //setup client
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Add("Access-key", token);
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

                string resultJson = await response.Content.ReadAsStringAsync();

                var status = JObject.Parse(resultJson)["status"].ToString();

                if (status != "200")
                {
                    var message = JObject.Parse(resultJson)["message"].ToString();
                    throw new GenericException(message);
                }

                result = JsonConvert.DeserializeObject<AzapayTransferResponseDTO>(resultJson);

                return result;
            }
        }

        private string GetAccessKey()
        {
            var accessKey = ConfigurationManager.AppSettings["AzapayAccessKey"]; ;
            return accessKey;
        }
    }
}
