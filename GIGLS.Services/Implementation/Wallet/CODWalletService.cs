using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.Domain.Utility;
using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using POST.Core.IMessageService;
using POST.Core.IServices.User;
using POST.Core.IServices.Utility;
using POST.Core.IServices.Wallet;
using POST.CORE.Enums;
using POST.Infrastructure;
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

namespace POST.Services.Implementation.Wallet
{
    public class CODWalletService : ICODWalletService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IStellasService _stellasService;
        private readonly IMessageSenderService _messageSenderService;

        public CODWalletService(IUserService userService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService, IStellasService stellasService, IMessageSenderService messageSenderService)
        {
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
            _stellasService = stellasService;
            _messageSenderService = messageSenderService;
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

                    //Create User login details on stella core banking
                    if (!string.IsNullOrEmpty(result.data.account_details.customerId))
                    {
                        var resultResponse = await AddCustomerToStellaCoreBanking(result.data.account_details.customerId);
                        var loginDetails = new CreateAccountCoreBankingResponseDTO();

                        if (resultResponse != null && resultResponse.data != null)
                        {
                            loginDetails = (CreateAccountCoreBankingResponseDTO)resultResponse.data;
                        } 

                        //Send stella account login details to customer
                        if (loginDetails !=null && loginDetails.Data != null)
                        {
                            if (loginDetails.Data?.LoginDetails != null)
                            {
                                //Save username and password to codwallet table
                                await AddCODWalletLoginDetails(user.CustomerCode, loginDetails.Data.LoginDetails.Username, loginDetails.Data.LoginDetails.Password);

                                //Send Email to user
                                var message = new MessageDTO
                                {
                                    ToEmail = user.Email,
                                    To = user.Email,
                                    MessageTemplate = "CODWalletAccountCreation",
                                    StellaLoginDetails = new StellaLoginDetails
                                    {
                                        Username = loginDetails.Data.LoginDetails.Username,
                                        CustomerName = user?.Name,
                                        AccountNumber = result.data.account_details.accountNumber
                                    }
                                };
                                await _messageSenderService.SendEmailForStellaLoginDetails(message);
                            }
                        }
                    }
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

        public async Task<bool> CheckIfUserHasCODWallet(string customerCode)
        {
            if (String.IsNullOrEmpty(customerCode))
            {
                throw new GenericException("Invalid code", $"{(int)HttpStatusCode.BadRequest}");
            }
            customerCode = customerCode.Trim();
            return await _uow.CODWallet.ExistAsync(x => x.CustomerCode == customerCode);
        }

        public async Task<StellasResponseDTO> ValidateBVNNumber(ValidateCustomerBVN payload)
        {
            if (payload == null)
            {
                throw new GenericException("Invalid Payload", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (string.IsNullOrEmpty(payload.FirstName))
            {
                throw new GenericException("FirstName is required", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (string.IsNullOrEmpty(payload.LastName))
            {
                throw new GenericException("LastName is required", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (string.IsNullOrEmpty(payload.Bvn))
            {
                throw new GenericException("BVN is required", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (string.IsNullOrEmpty(payload.PhoneNo))
            {
                throw new GenericException("Phone number is required", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (string.IsNullOrEmpty(payload.DateOfBirth))
            {
                throw new GenericException("Date of birth is required", $"{(int)HttpStatusCode.BadRequest}");
            }
            return await _stellasService.ValidateBVNNumber(payload);
        }

        public async Task<StellasResponseDTO> AddCustomerToStellaCoreBanking(string customerId)
        {
            try
            {
                var payload = new CreateAccountCoreBankingDTO { CustomerId = customerId };
                return await _stellasService.CreateAccountOnCoreBanking(payload);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LoginDetailsDTO> GetStellasAccountLoginDetails(string customerCode)
        {
            var result = new LoginDetailsDTO { UserName = "No login details", Password = "No login details" };
            if (String.IsNullOrEmpty(customerCode))
            {
                throw new GenericException("Invalid code", $"{(int)HttpStatusCode.BadRequest}");
            }
            var custmerAccountInfo = await _uow.CODWallet.GetAsync(x => x.CustomerCode == customerCode);
            if (custmerAccountInfo != null)
            {
                result.UserName = string.IsNullOrEmpty(custmerAccountInfo.UserName) ? result.UserName : custmerAccountInfo.UserName;
                result.Password = string.IsNullOrEmpty(custmerAccountInfo.Password) ? result.Password : custmerAccountInfo.Password;
                return result;
            }
            return result;
        }
        public async Task AddCODWalletLoginDetails(string customerCode, string userName, string password)
        {
            try
            {
                var codWallet = _uow.CODWallet.GetAllAsQueryable().Where(x => x.CustomerCode == customerCode).FirstOrDefault();
                if (codWallet != null)
                {
                    codWallet.UserName = userName;
                    codWallet.Password = password;
                    _uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}