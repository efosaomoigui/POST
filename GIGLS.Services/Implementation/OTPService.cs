using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IServices;
using System.Threading.Tasks;
using System;
using GIGLS.Core.IMessage;
using AutoMapper;
using GIGLS.Core.IServices.User;
using System.Text.RegularExpressions;
using GIGLS.Core.IServices.Utility;
using System.Collections.Generic;
using System.Linq;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using System.Net;

namespace GIGLS.Services.Implementation
{
    public class OTPService : IOTPService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISMSService _SmsService;
        private readonly IEmailService _EmailService;
        private readonly IUserService _UserService;
        private readonly IPasswordGenerator _codegenerator;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IWalletTransactionService _iWalletTransactionService;

        public OTPService(IUnitOfWork uow, ISMSService MessageService, IEmailService EmailService, IUserService UserService,
            IPasswordGenerator codegenerator, IMessageSenderService messageSenderService, IGlobalPropertyService globalPropertyService,
            IWalletTransactionService iWalletTransactionService)
        {
            _uow = uow;
            _SmsService = MessageService;
            _EmailService = EmailService;
            _UserService = UserService;
            _codegenerator = codegenerator;
            _messageSenderService = messageSenderService;
            _globalPropertyService = globalPropertyService;
            _iWalletTransactionService = iWalletTransactionService;
            MapperConfig.Initialize();
        }
        public async Task<UserDTO> IsOTPValid(int OTP)
        {
            var otpbody = await _uow.OTP.IsOTPValid(OTP);
            var userdto = new UserDTO();

            if (otpbody.IsValid == true)
            {
                userdto = await _UserService.GetActivatedUserByEmail(otpbody.EmailAddress, true);
                _uow.OTP.Remove(otpbody);
                await _uow.CompleteAsync();
            }
            return userdto;
        }

        private string ExtractPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Trim();

            bool IsPhone = Regex.IsMatch(phoneNumber, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");

            if (IsPhone)
            {
                if (!(phoneNumber.StartsWith("0") || phoneNumber.StartsWith("+")))
                {
                    phoneNumber = phoneNumber.Remove(0, 4);
                }

                if (phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.Remove(0, 4);
                }

                if (phoneNumber.StartsWith("0"))
                {
                    phoneNumber = phoneNumber.Remove(0, 1);
                }
            }

            return phoneNumber;
        }

        public async Task<UserDTO> ValidateOTP(OTPDTO otp)
        {
            otp.EmailAddress = ExtractPhoneNumber(otp.EmailAddress);

            //get the otp details using the email 
            var result = _uow.OTP.GetAllAsQueryable().Where(x => x.Otp == otp.Otp && (x.EmailAddress == otp.EmailAddress || x.PhoneNumber.Contains(otp.EmailAddress))).ToList();
            var otpbody = result.LastOrDefault();

            if (otpbody == null)
            {
                throw new GenericException("Invalid OTP", $"{(int)HttpStatusCode.Forbidden}");
            }
            else
            {
                DateTime LatestTime = DateTime.Now;
                TimeSpan span = LatestTime.Subtract(otpbody.DateCreated);
                int difference = Convert.ToInt32(span.TotalMinutes);
                if (difference < 5)
                {
                    //customer activated
                    var userdto = await _UserService.GetActivatedUserByEmail(otpbody.EmailAddress, true);
                    _uow.OTP.Remove(otpbody);
                    await _uow.CompleteAsync();

                    if (userdto.IsActive)
                    {
                        await CalculateReferralBonus(userdto);
                    }

                    return userdto;
                }
                else
                {
                    _uow.OTP.Remove(otpbody);
                    await _uow.CompleteAsync();
                    throw new GenericException("OTP has expired!.Kindly Resend OTP.", $"{(int)HttpStatusCode.Forbidden}");
                }
            }
        }

        public async Task<OTPDTO> GenerateOTP(UserDTO user)
        {
            int id = GeneratePassword();
            var otp = new OTPDTO
            {
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                CustomerId = user.UserChannelCode,
                Otp = id
            };

            var result = Mapper.Map<OTP>(otp);
            _uow.OTP.Add(result);
            await _uow.CompleteAsync();
            return otp;
        }

        private static int GeneratePassword()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new Random();
            return rdm.Next(min, max);
        }

        public async Task<bool> SendOTP(OTPDTO user)
        {
            var message = new MobileMessageDTO
            {
                SenderEmail = user.EmailAddress,
                SenderPhoneNumber = user.PhoneNumber,
                OTP = user.Otp,
                SMSSenderPlatform = SMSSenderPlatform.TWILIO
            };

            var response = await _messageSenderService.SendMessage(MessageType.OTP, EmailSmsType.All, message);
            return response;
        }

        public async Task<UserDTO> CheckDetails(string user, string userchanneltype)
        {
            try
            {
                var registerUser = await _UserService.GetUserUsingCustomer(user);
                
                if (registerUser != null)
                {
                    var company = await _uow.Company.GetAsync(s => s.CustomerCode == registerUser.UserChannelCode);
                    if(company != null)
                    {
                        registerUser.IsEligible = Convert.ToBoolean(company.IsEligible);
                    }
                }

                UserDTO registerUserDTo = new UserDTO();
                registerUserDTo = await CheckVehicleInformation(registerUser, userchanneltype);

                return registerUserDTo;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //Check Details for Customer Portal
        public async Task<UserDTO> CheckDetailsForCustomerPortal(string user)
        {
            try
            {
                var registerUser = await _UserService.GetUserUsingCustomerForCustomerPortal(user);
                return registerUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Check Details for Mobile Scanner 
        public async Task<UserDTO> CheckDetailsForMobileScanner(string user)
        {
            try
            {
                var registerUser = await _UserService.GetUserUsingCustomerForMobileScanner(user);
                return registerUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Check Details for Fast Track Agent App  
        public async Task<UserDTO> CheckDetailsForAgentApp(string user)
        {
            try
            {
                var registerUser = await _UserService.GetUserUsingCustomerForAgentApp(user);
                return registerUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<double> GetAverageRating(string CustomerCode, string usertype)
        {
            if (usertype == UserChannelType.Partner.ToString())
            {
                var ratings = await _uow.MobileRating.FindAsync(s => s.PartnerCode == CustomerCode);
                var count = ratings.Count();
                var averageratings = ratings.Sum(x => x.CustomerRating);
                averageratings = (averageratings / count);
                if (averageratings.ToString() == "NaN")
                {
                    averageratings = 0.00;
                }
                var rating = (double)averageratings;
                return rating;
            }
            else
            {
                var ratings = await _uow.MobileRating.FindAsync(s => s.CustomerCode == CustomerCode);
                var count = ratings.Count();
                var averageratings = ratings.Sum(x => x.PartnerRating);
                averageratings = (averageratings / count);
                if (averageratings.ToString() == "NaN")
                {
                    averageratings = 0.00;
                }
                var rating = (double)averageratings;
                return rating;
            }
        }

        public async Task<bool> IsPartnerActivated(string CustomerCode)
        {
            try
            {
                bool IsActivated = false;
                var partner = await _uow.Partner.GetAsync(s => s.PartnerCode == CustomerCode);
                if (partner != null)
                {
                    IsActivated = partner.IsActivated;
                }
                return IsActivated;
            }
            catch
            {
                throw;
            }
        }

        private async Task<UserDTO> CheckVehicleInformation(UserDTO registerUser, string userchanneltype)
        {
            try
            {
                var VehicleType = await _uow.Partner.GetAsync(s => s.PartnerCode == registerUser.UserChannelCode);
                if (VehicleType != null)
                {
                    registerUser.VehicleLicenseExpiryDate = VehicleType.VehicleLicenseExpiryDate;
                    var vehicle = await _uow.VehicleType.FindAsync(s => s.Partnercode == registerUser.UserChannelCode);

                    if (VehicleType.VehicleType != null)
                    {
                        var vehicleTypeArray = vehicle.Select(x => x.Vehicletype).ToList();

                        if (!vehicleTypeArray.Contains(VehicleType.VehicleType))
                        {
                            var vehicletypeDTO = new VehicleTypeDTO
                            {
                                Partnercode = registerUser.UserChannelCode,
                                Vehicletype = VehicleType.VehicleType,
                            };
                            var vehicletype = Mapper.Map<VehicleType>(vehicletypeDTO);
                            _uow.VehicleType.Add(vehicletype);
                            VehicleType.VehicleType = null;
                            await _uow.CompleteAsync();
                        }
                    }

                    if (vehicle.Any())
                    {
                        registerUser.VehicleDetails = new List<VehicleTypeDTO>();
                        registerUser.VehicleType = new List<string>();
                        var vehicles = Mapper.Map <List<VehicleTypeDTO>>(vehicle);
                        foreach (var item in vehicles)
                        {
                            registerUser.VehicleType.Add(item.Vehicletype);
                            registerUser.VehicleDetails.Add(item);
                        }
                    }

                    registerUser.IsVerified = VehicleType.IsActivated;
                }

                var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.UserCode == registerUser.UserChannelCode);
                if (referrerCode != null)
                {
                    registerUser.Referrercode = referrerCode.Referrercode;
                }
                else
                {
                    registerUser = await GenerateReferrerCode(registerUser);
                }

                var averageratings = await GetAverageRating(registerUser.UserChannelCode, userchanneltype);
                registerUser.AverageRatings = averageratings;
                return registerUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO> GenerateReferrerCode(UserDTO user)
        {

            var code = await _codegenerator.Generate(5);
            var ReferrerCodeExists = await _uow.ReferrerCode.GetAsync(s => s.UserCode == user.UserChannelCode);
            if (ReferrerCodeExists == null)
            {
                var referrerCodeDTO = new ReferrerCodeDTO
                {
                    Referrercode = code,
                    UserId = user.Id,
                    UserCode = user.UserChannelCode
                };
                var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                _uow.ReferrerCode.Add(referrercode);
                await _uow.CompleteAsync();
                user.Referrercode = referrercode.Referrercode;
            }
            else
            {
                user.Referrercode = ReferrerCodeExists.Referrercode;
            }
            return user;
        }

        private async Task CalculateReferralBonus(UserDTO User)
        {           
            if (User.RegistrationReferrercode != null && User.IsUniqueInstalled == true)
            {
                var referrercode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == User.RegistrationReferrercode);
                if (referrercode != null)
                {
                    var bonus = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ReferrerCodeBonus.ToString() && s.CountryId == User.UserActiveCountryId);

                    if(bonus != null)
                    {
                        decimal bonusAmount = Convert.ToDecimal(bonus.Value);

                        var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrercode.UserCode);
                        if (wallet != null)
                        {
                            wallet.Balance = wallet.Balance + bonusAmount;
                        }

                        var ReferrerUser = await _UserService.GetUserByChannelCode(referrercode.UserCode);
                        var transaction = new WalletTransactionDTO
                        {
                            WalletId = wallet.WalletId,
                            CreditDebitType = CreditDebitType.Credit,
                            Amount = bonusAmount,
                            ServiceCentreId = 296,
                            Waybill = "",
                            Description = "Referral Bonus",
                            PaymentType = PaymentType.Online,
                            UserId = ReferrerUser.Id
                        };
                        var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                        await _uow.CompleteAsync();

                        var messageExtensionDTO = new MobileMessageDTO()
                        {
                            SenderName = ReferrerUser.FirstName + " " + ReferrerUser.LastName,
                            SenderEmail = ReferrerUser.Email

                        };
                        await _messageSenderService.SendGenericEmailMessage(MessageType.MRB, messageExtensionDTO);
                    }                   
                }
            }

        }

    }
}
