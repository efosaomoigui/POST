using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Utility;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.Enums;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class StellasService : IStellasService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private static string _auth = System.Web.HttpContext.Current.Server.MapPath("~");

        public StellasService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        public async Task<CreateStellaAccounResponsetDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO)
        {
            string secretKey = ConfigurationManager.AppSettings["StellasSecretKey"];
            string url = ConfigurationManager.AppSettings["StellasCreateAccount"];
            string bizId = ConfigurationManager.AppSettings["BusinessID"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string authorization = await GetToken();
            if (String.IsNullOrEmpty(authorization))
            {
                var auth = await Authenticate();
                if (auth.Key)
                {
                    authorization = await GetToken();
                }
                else
                {
                    throw new GenericException(auth.ToString());
                }
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("SECRET_KEY", secretKey);
                client.DefaultRequestHeaders.Add("businessId", bizId);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authorization}");
                var json = JsonConvert.SerializeObject(createStellaAccountDTO);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, data);
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var auth = await Authenticate();
                    authorization = await GetToken();
                    var retrialResponse = await client.PostAsync(url, data);
                    string retrialResponseResult = await retrialResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CreateStellaAccounResponsetDTO>(retrialResponseResult);
                    return result;
                }
                else if (response.IsSuccessStatusCode)
                {
                    string responseResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CreateStellaAccounResponsetDTO>(responseResult);
                    return result;
                }
                else
                {
                    throw new GenericException(response.Content.ReadAsStringAsync().Result, response.StatusCode.ToString());
                }
            }
        }

        public async Task<GetCustomerBalanceDTO> GetCustomerStellasAccount(string accountNo)
        {
            var url = ConfigurationManager.AppSettings["StellasSandBox"];
            string secretKey = ConfigurationManager.AppSettings["StellasSecretKey"];
            string authorization = "Bearer " + secretKey;
            url = $"{url}clients/business/customers/balance/{accountNo}";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var response = await client.GetAsync(url);
                string responseResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GetCustomerBalanceDTO>(responseResult);
                return result;
            }
        }


        private static async Task<KeyValuePair<bool, string>> Authenticate()
        {
            using (var client = new HttpClient())
            {
                var authModel = new AuthModel();
                var credentials = JToken.FromObject(authModel);
                var data = JsonConvert.SerializeObject(credentials);
                KeyValuePair<string, string>[] nameValueCollection = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("grant_type", "password"), new KeyValuePair<string, string>("email", authModel.email), new KeyValuePair<string, string>("password", authModel.password) };
                FormUrlEncodedContent content = new FormUrlEncodedContent(nameValueCollection);
                string authUrl = ConfigurationManager.AppSettings["StellasAuth"];

                var path = _auth + @"\Auth";
                var response = await client.PostAsync(authUrl, content);
                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JToken.Parse(await response.Content.ReadAsStringAsync());
                        // save the result in a file
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (StreamWriter sw = new StreamWriter($"{path}\\config.txt"))
                        {
                            sw.Write(result);
                        }
                        return new KeyValuePair<bool, string>(true, result.ToString());
                    }
                }
                catch (Exception exp)
                {
                    return new KeyValuePair<bool, string>(false, exp.Message);
                }
                var ex = await response.Content.ReadAsStringAsync();
                return new KeyValuePair<bool, string>(false, ex); 
            }
        }

        private static async Task<string> GetToken()
        {
            try
            {
                var loc = _auth + @"\Auth";
                string path = $"{loc}\\config.txt";
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        var obj = await sr.ReadToEndAsync().ContinueWith(t => JObject.Parse(t.Result));
                        return (string)obj["data"]["accessToken"];
                    }
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
            return String.Empty;
        }

        private async Task<string> Retry(string url,string data)
        {
            var token = String.Empty;
            var auth = await Authenticate();
            if (auth.Key)
            {
                token = JObject.Parse(auth.Value).SelectToken("accessToken").ToString();
                if (String.IsNullOrEmpty(token))
                {
                    return string.Empty;
                }
                return token;
            }
            return token;
        }

        private class AuthModel
        {
            public string email { get; set; } = "it@giglogistics.com";
            public string password { get; set; } = "Password@001";

        }
    }
}