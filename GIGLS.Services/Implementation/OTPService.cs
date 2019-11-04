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

        public OTPService(IUnitOfWork uow, ISMSService MessageService, IEmailService EmailService, IUserService UserService,
            IPasswordGenerator codegenerator, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _SmsService = MessageService;
            _EmailService = EmailService;
            _UserService = UserService;
            _codegenerator = codegenerator;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }
        public async Task<UserDTO> IsOTPValid(int OTP)
        {
            var otpbody = await _uow.OTP.IsOTPValid(OTP);
            var result = Mapper.Map<OTPDTO>(otpbody);
            var userdto = await _UserService.GetUserByEmail(result.EmailAddress);
            if (result.IsValid == true)
            {
                userdto.IsActive = true;
                await _UserService.UpdateUser(userdto.Id, userdto);
                _uow.OTP.Remove(otpbody);
                await _uow.CompleteAsync();
            }
            return userdto;
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
                UserDTO RegisterUser = new UserDTO();
                bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
                {
                    user.Trim();
                    var registerUser = await _UserService.GetUserByEmail(user);
                    RegisterUser = await CheckVehicleInformation(registerUser, userchanneltype);
                }
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    user = user.Remove(0, 1);
                    var registerUser = await _UserService.GetUserByPhone(user);
                    RegisterUser = await CheckVehicleInformation(registerUser, userchanneltype);
                }
                if (!isEmail && !IsPhone)
                {
                    var registerUser = await _UserService.GetUserByChannelCode(user);
                    RegisterUser = await CheckVehicleInformation(registerUser, userchanneltype);
                }

                return RegisterUser;
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
                    if (VehicleType.VehicleType != null)
                    {
                        var vehicletypeDTO = new VehicleTypeDTO
                        {
                            Partnercode = registerUser.UserChannelCode,
                            Vehicletype = VehicleType.VehicleType
                        };
                        var vehicletype = Mapper.Map<VehicleType>(vehicletypeDTO);
                        _uow.VehicleType.Add(vehicletype);
                        VehicleType.VehicleType = null;
                        await _uow.CompleteAsync();
                    }
                }
                var vehicle = _uow.VehicleType.FindAsync(s => s.Partnercode == registerUser.UserChannelCode).Result.ToList();
                if (vehicle.Count() != 0)
                {
                    registerUser.VehicleType = new List<string>();
                    foreach (var item in vehicle)
                    {
                        registerUser.VehicleType.Add(item.Vehicletype);
                    }
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
                var IsVerified = await IsPartnerActivated(registerUser.UserChannelCode);
                registerUser.AverageRatings = averageratings;
                registerUser.IsVerified = IsVerified;
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

    }
}
