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
        private readonly IStellasService _stellasService;

        public CODWalletService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService, IStellasService stellasService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            _stellasService = stellasService;
            MapperConfig.Initialize();
        }


        public async Task<StellasResponseDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO)
        {
            var exist = await _uow.CODWallet.ExistAsync(x => x.CustomerCode == createStellaAccountDTO.CustomerCode);
            if (exist)
            {
                throw new GenericException("user already has a COD wallet", $"{(int)HttpStatusCode.NotFound}");
            }
            var user = await _uow.Company.GetAsync(x => x.CustomerCode == createStellaAccountDTO.CustomerCode);
            var Aspuser = await _uow.User.GetUserByChannelCode(createStellaAccountDTO.CustomerCode);
            if (user is null)
            {
                throw new GenericException("User does not exist as an Ecommerce user!", $"{(int)HttpStatusCode.NotFound}");
            }
            CODWalletDTO res = new CODWalletDTO();
            var resp = await _stellasService.CreateStellasAccount(createStellaAccountDTO);
            if (resp.status)
            {
                if (resp.data is CreateStellaAccounResponsetDTO)
                {
                    var result = (CreateStellaAccounResponsetDTO)resp.data;
                    //now create this user CODWALLET on agility  
                    CODWalletDTO codWalletDTO = new CODWalletDTO
                    {
                        AccountNo = result.data.account_details.accountNumber,
                        AvailableBalance = result.data.account_details.availableBalance,
                        CustomerId = result.data.account_details.customerId,
                        CustomerCode = user.CustomerCode,
                        CustomerType = CustomerType.Company,
                        CompanyType = CompanyType.Ecommerce.ToString(),
                        AccountType = result.data.account_details.accountType,
                        WithdrawableBalance = result.data.account_details.withdrawableBalance,
                        CustomerAccountId = result.data.account_details.customerId,
                        DateOfBirth = result.data.customerDetails.dateOfBirth,
                        PlaceOfBirth = result.data.customerDetails.placeOfBirth,
                        Address = result.data.customerDetails.address,
                        NationalIdentityNo = result.data.customerDetails.nationalIdentityNo,
                        UserId = Aspuser.Id,
                        FirstName = createStellaAccountDTO.firstName,
                        LastName = createStellaAccountDTO.lastName

                    };
                    await AddCODWallet(codWalletDTO);
                    res = codWalletDTO;
                    resp.data = res;
                }
            }
            else
            {
                res = null;
            }
            return resp;
        }



        public async Task AddCODWallet(CODWalletDTO codWalletDTO)
        {
            try
            {
                var codWallet = Mapper.Map<CODWallet>(codWalletDTO);
                _uow.CODWallet.Add(codWallet);
                _uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<StellasResponseDTO> GetStellasAccountBal(string customerCode)
        {
            if (String.IsNullOrEmpty(customerCode))
            {
                throw new GenericException("Invalid code", $"{(int)HttpStatusCode.BadRequest}");
            }
            var custmerAccountInfo = await _uow.CODWallet.GetAsync(x => x.CustomerCode == customerCode);
            if (custmerAccountInfo != null)
            {
                var bal = await _stellasService.GetCustomerStellasAccount(custmerAccountInfo.AccountNo);
                return bal; 
            }
            return new StellasResponseDTO();
        }

        public async Task<StellasResponseDTO> GetStellasBanks()
        {
            return await _stellasService.GetBanks();
        }

        public async Task<StellasResponseDTO> StellasWithdrawal(StellasWithdrawalDTO stellasWithdrawalDTO)
        {
            return await _stellasService.StellasWithdrawal(stellasWithdrawalDTO);
        }

        public async Task<StellasResponseDTO> StellasTransfer(StellasTransferDTO transferDTO)
        {
            return await _stellasService.StellasTransfer(transferDTO);
        }

        public async Task<StellasResponseDTO> StellasValidateBankName(ValidateBankNameDTO validateBankNameDTO)
        {
            if (validateBankNameDTO is null)
            {
                throw new GenericException("invalid payload");
            }
            return await _stellasService.StellasValidateBankName(validateBankNameDTO);
        }


    }
}