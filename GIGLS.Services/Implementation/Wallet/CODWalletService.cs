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
    public class CODWalletService : ICODWalletService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;

        public CODWalletService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            MapperConfig.Initialize();
        }

        private async Task CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO)
        {
            var user = await _uow.Company.GetAsync(x => x.CustomerCode == createStellaAccountDTO.CustomerCode);
            if (user is null)
            {

            }

            var url = ConfigurationManager.AppSettings["StellasSandBox"];
            string secretKey = ConfigurationManager.AppSettings["StellasSecretKey"];
            string authorization = "Bearer " + secretKey;
            url = $"{url}account/create-customer";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var json = JsonConvert.SerializeObject(createStellaAccountDTO);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url,data);
                string responseResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CreateStellaAccounResponsetDTO>(responseResult);

                if (result.status)
                {
                    //now create this user CODWALLET on agility  
                    CODWalletDTO codWalletDTO = new CODWalletDTO
                    {
                        AccountNo = result.account_details.accountNumber,
                        AvailableBalance = result.account_details.availableBalance,
                        CustomerId = result.account_details.customerId,
                        CustomerCode = user.CustomerCode,
                        CustomerType = CustomerType.Company,
                        CompanyType = CompanyType.Ecommerce.ToString(),
                        AccountType = result.account_details.accountType,
                        WithdrawableBalance = result.account_details.withdrawableBalance,
                        CustomerAccountId = result.account_details.customerId
                    };
                    await AddCODWallet(codWalletDTO);
                }
            }

        }

        public async Task AddCODWallet(CODWalletDTO codWalletDTO)
        {
            if (codWalletDTO != null)
            {
                var codWallet = Mapper.Map<CODWallet>(codWalletDTO);
                _uow.CODWallet.Add(codWallet);
                _uow.CompleteAsync();
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