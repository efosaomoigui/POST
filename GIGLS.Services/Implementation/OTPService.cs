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

namespace GIGLS.Services.Implementation
{
    public class OTPService : IOTPService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISMSService _SmsService;
        private readonly IEmailService _EmailService;
        private readonly IUserService _UserService;

        public OTPService(IUnitOfWork uow, ISMSService MessageService, IEmailService EmailService, IUserService UserService)
        {
            _uow = uow;
            _SmsService = MessageService;
            _EmailService = EmailService;
            _UserService = UserService;
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
                    CustomerId = user.CustomerId,
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
    }
}
