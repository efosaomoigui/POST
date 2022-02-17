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

        public StellasService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        public async Task<CreateStellaAccounResponsetDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO)
        {
            var url = ConfigurationManager.AppSettings["StellasSandBox"];
            string secretKey = ConfigurationManager.AppSettings["StellasSecretKey"];
            string authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjM2OThlOTU4LTUwNTEtNDc0My04MGU1LTljOTFhNDAxNjliMSIsImVtYWlsIjoiaXRAZ2lnbG9naXN0aWNzLmNvbSIsImNsaWVudElkIjoiY2NiYTZmMjUtYjk0NS00Yzc3LTg2YzMtZmY3MDFjMTI5ZTYxIiwiaWF0IjoxNjQ1MDMwODkxLCJleHAiOjE2NDUyMDM2OTF9.TiAvDAwfJieOUu6pisB4J5Mf55SL_UqKay2zcdq26v0";
            url = $"{url}account/create-customer";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("SECRET_KEY", secretKey);
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var json = JsonConvert.SerializeObject(createStellaAccountDTO);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, data);
                string responseResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CreateStellaAccounResponsetDTO>(responseResult);
                return result;
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
    }
}