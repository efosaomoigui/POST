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

namespace GIGLS.Services.Implementation
{
    public class OTPService : IOTPService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISMSService _SmsService;
        private readonly IEmailService _EmailService;
        private readonly IUserService _UserService;
        private readonly IPasswordGenerator _codegenerator;

        public OTPService(IUnitOfWork uow, ISMSService MessageService, IEmailService EmailService, IUserService UserService,
            IPasswordGenerator codegenerator)
        {
            _uow = uow;
            _SmsService = MessageService;
            _EmailService = EmailService;
            _UserService = UserService;
            _codegenerator = codegenerator;
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
        public  async Task<string> SendOTP(OTPDTO user)
        {
             
            MessageDTO SMSmessage = new MessageDTO
            {
                To = user.PhoneNumber,
                FinalBody = $"Your OTP is {user.Otp}"
            };
            MessageDTO Emailmessage = new MessageDTO
            {
                CustomerName = "",
                ReceiverName = "",
                Subject = "OTP",
                ToEmail = user.EmailAddress,
                FinalBody = $"Thank you for registering .Your OTP is {user.Otp}"
            };
            var EmailResponse = await _EmailService.SendAsync(Emailmessage);
            var Smsresponse = await _SmsService.SendAsync(SMSmessage);
           return $"{EmailResponse},{Smsresponse}";
        }
        public async Task<UserDTO> CheckDetails(string user)
        {
            UserDTO registerUser = new UserDTO();
            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                user.Trim();
                registerUser = await _UserService.GetUserByEmail(user);
                var vehicle = _uow.Partner.GetAsync(s => s.PartnerCode == registerUser.UserChannelCode).Result;
                if(vehicle!= null)
                {
                    registerUser.VehicleType = vehicle.VehicleType;
                }
                var referrerCode = _uow.ReferrerCode.GetAsync(s => s.UserCode == registerUser.UserChannelCode).Result;
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
                var averageratings = await GetAverageRating(registerUser.UserChannelCode);
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
                    var vehicle = _uow.Partner.GetAsync(s => s.PartnerCode == registerUser.UserChannelCode).Result;
                    if (vehicle != null)
                    {
                        registerUser.VehicleType = vehicle.VehicleType;
                    }
                    var referrerCode = _uow.ReferrerCode.GetAsync(s => s.UserCode == registerUser.UserChannelCode).Result;
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
                    var averageratings = await GetAverageRating(registerUser.UserChannelCode);
                    registerUser.AverageRatings = averageratings;
                }
                else
                {
                    throw new GenericException("Invalid Details");
                }
            }
            return registerUser;

        }

        public async Task<double> GetAverageRating(string CustomerCode)
        {
            var ratings = await _uow.MobileRating.FindAsync(s=>s.CustomerCode == CustomerCode || s.PartnerCode == CustomerCode);
            var count = ratings.Count();
            var averageratings = ratings.Sum(x=>x.CustomerRating);
            averageratings = (averageratings / count);
            var rating = (double)averageratings;
            return rating;
        }

    }
}
