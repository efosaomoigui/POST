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
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CODWalletService : ICODWalletService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly string secretKey = ConfigurationManager.AppSettings["StellasSecretKey"];

        public CODWalletService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        public async Task AddCODWallet(CreateStellaAccountDTO createStellaAccountDTO)
        {
            var user = _uow.Company.GetAsync(x => x.CustomerCode == createStellaAccountDTO.CustomerCode);
            if (user is null)
            {

            }

            var url = ConfigurationManager.AppSettings["StellasUrl"];
            url = $"{url}account/create-customer";
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(createStellaAccountDTO);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
               // var jObject = JsonConvert.DeserializeObject<NodeResponse>(result);
              //  nodeResponse = jObject;
            }
        }

        public Task<WalletDTO> GetCODWalletById(int walletId)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Domain.Wallet.Wallet> GetCODWalletById(string walletNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WalletDTO>> GetCODWallets()
        {
            throw new NotImplementedException();
        }

        public Task RemoveCODWallet(int walletId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCODWallet(int walletId, WalletTransactionDTO walletTransactionDTO, bool hasServiceCentre = true)
        {
            throw new NotImplementedException();
        }
    }
}