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
using GIGLS.Infrastructure;
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
            IPasswordGenerator codegenerator, IMessageSenderService messageSenderService )
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
            if(result.IsValid ==true)
            {
                userdto.IsActive=true;
                var user = await _UserService.UpdateUser(userdto.Id, userdto);
            }
            return userdto;
        }
               
        public  async Task<OTPDTO> GenerateOTP(UserDTO user)
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
        public  async Task<bool> SendOTP(OTPDTO user)
        {

            var message = new MobileMessageDTO
            {
                SenderEmail = user.EmailAddress,
                SenderPhoneNumber = user.PhoneNumber,
                OTP = user.Otp
            };
            var response = await _messageSenderService.SendMessage(MessageType.OTP, EmailSmsType.All, message);
            return response;
        }
        public async Task<UserDTO> CheckDetails(string user, string userchanneltype)
        {
            UserDTO registerUser = new UserDTO();
            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                user.Trim();
                registerUser = await _UserService.GetUserByEmail(user);
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
                if(vehicle.Count() != 0)
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
                    var code = await _codegenerator.Generate(5);
                    var referrerCodeDTO = new ReferrerCodeDTO
                    {
                        Referrercode = code,
                        UserId = registerUser.Id,
                        UserCode = registerUser.UserChannelCode

                    };
                    var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                    _uow.ReferrerCode.Add(referrercode);
                    await _uow.CompleteAsync();
                    registerUser.Referrercode = referrercode.Referrercode;
                }
                var averageratings = await GetAverageRating(registerUser.UserChannelCode, userchanneltype);
                var IsVerified = await IsPartnerActivated(registerUser.UserChannelCode);
                registerUser.IsVerified = IsVerified;
                registerUser.AverageRatings = averageratings;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    if (!user.Contains("+234"))
                    {
                        user = "+234" + user.Remove(0, 1);
                    };
                    registerUser = await _UserService.GetUserByPhone(user);
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
                        var code = await _codegenerator.Generate(5);
                        var referrerCodeDTO = new ReferrerCodeDTO
                        {
                            Referrercode = code,
                            UserId = registerUser.Id,
                            UserCode = registerUser.UserChannelCode

                        };
                        var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                        _uow.ReferrerCode.Add(referrercode);
                        await _uow.CompleteAsync();
                        registerUser.Referrercode = referrercode.Referrercode;
                    }
                    var averageratings = await GetAverageRating(registerUser.UserChannelCode, userchanneltype);
                    var IsVerified = await IsPartnerActivated(registerUser.UserChannelCode);
                    registerUser.AverageRatings = averageratings;
                    registerUser.IsVerified = IsVerified;
                }
                else
                {
                    throw new GenericException("Invalid Details");
                }
            }
            return registerUser;

        }

        public async Task<double> GetAverageRating(string CustomerCode,string usertype)
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
                var ratings = await _uow.MobileRating.FindAsync(s =>s.CustomerCode == CustomerCode);
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

    }
}
