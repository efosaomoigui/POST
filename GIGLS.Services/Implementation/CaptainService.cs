using GIGLS.Core;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.IServices.User;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using System;
using GIGLS.Core.IServices.Utility;
using GIGLS.Services.Implementation.Messaging;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Infrastructure;
using System.Collections.Generic;

namespace GIGLS.Services.Implementation
{
    public class CaptainService : ICaptainService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly MessageSenderService _messageSenderService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPartnerService _partnerService;

        public CaptainService(IUserService userService, IUnitOfWork uow, IPasswordGenerator passwordGenerator, MessageSenderService messageSenderService, INumberGeneratorMonitorService numberGeneratorMonitorService, IPartnerService partnerService)
        {
            _userService = userService;
            _uow = uow;
            _passwordGenerator = passwordGenerator;
            _messageSenderService = messageSenderService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _partnerService = partnerService;
        }


        public async Task<object> RegisterCaptainAsync(CaptainDTO captainDTO)
        {
            
            var currentUserId = await _userService.GetCurrentUserId();

            var currentUser = await _userService.GetUserById(currentUserId);
            //if(currentUser.SystemUserRole != "CaptainManagement" || currentUser.SystemUserRole != "Administrator") 
            if(currentUser.SystemUserRole != "Administrator") 
            {
                throw new GenericException("You are not authorized to use this feature");
            }

            var confirmUser = await _uow.User.GetUserByEmail(captainDTO.Email);
            

            string password = await _passwordGenerator.Generate();

            var user = new GIGL.GIGLS.Core.Domain.User
            {
                Organisation = captainDTO.Organisation,
                Status = captainDTO.Status,
                DateCreated = DateTime.Now.Date,
                DateModified = DateTime.Now.Date,
                Department = captainDTO.Department,
                Designation = captainDTO.Designation,
                Email = captainDTO.Email,
                FirstName = captainDTO.FirstName,
                LastName = captainDTO.LastName,
                Gender = captainDTO.Gender,
                UserName = captainDTO.Email,
                PhoneNumber = captainDTO.PhoneNumber,
                UserType = captainDTO.UserType,
                IsActive = true,
                PictureUrl = captainDTO.PictureUrl,
                SystemUserRole = "Captain",
                PasswordExpireDate = DateTime.Now
        };
            user.Id = Guid.NewGuid().ToString();

            // partner
            var partnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Partner);
            var partner = new Partner()
            {
                Address = captainDTO.Address,
                CaptainAccountName = captainDTO.AccountName,
                CaptainAccountNumber = captainDTO.AccountNumber,
                CaptainBankName = captainDTO.BankName,
                DateCreated = DateTime.Now.Date,
                Email = captainDTO.Email,
                PictureUrl = captainDTO?.PictureUrl,
                DateModified = DateTime.Now.Date,
                FirstName = captainDTO.FirstName,
                LastName= captainDTO.LastName,
                PhoneNumber = captainDTO.PhoneNumber,
                PartnerType = PartnerType.Captain,
                UserId = user.Id,
                IsDeleted = false,
                IsActivated = true,
                PartnerCode = partnerCode,
                ActivityDate = DateTime.Now.Date,
                Age = captainDTO.Age,
                PartnerName = captainDTO.FirstName + " " + captainDTO.LastName,
            };

            if (confirmUser == null)
            {
                var result = await _uow.User.RegisterUser(user, password);
                if (result.Succeeded)
                {
                    _uow.Partner.Add(partner);
                }
            }
            else
            {
                _uow.Partner.Add(partner);
            }
            await _uow.CompleteAsync();

            var captainCreationSuccessMessage = new object();
            captainCreationSuccessMessage = new
            {
                Password = password,
                Email = captainDTO.Email
            };
            await _messageSenderService.SendGenericEmailMessage(MessageType.CAPEMAIL, captainCreationSuccessMessage);
            
            return new { id = user.Id, password = password, email = user.Email };
        }

        public async Task<IReadOnlyList<ViewCaptainsDTO>> GetCaptainsByDateAsync(DateTime? date)
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);

                if (currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "Administrator")
                {
                    var captains = await _uow.CaptainRepository.GetAllCaptainsByDateAsync(date);
                    return captains;
                }

                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CaptainDetailsDTO> GetCaptainByIdAsync(int partnerId)
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);

                if (currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "Administrator")
                {
                    var captain = await _uow.CaptainRepository.GetCaptainByIdAsync(partnerId);
                    var user = await _userService.GetUserById(captain.UserId);

                    var captainDetails = new CaptainDetailsDTO()
                    {
                        PartnerId = partnerId,
                        Status = user.Status == 1 ? "Active" : "Inactive",
                        EmploymentDate = captain.DateCreated,
                        AssignedVehicleName = captain.VehicleType,
                        AssignedVehicleNumber = captain.VehicleLicenseNumber,
                        CaptainAge = captain.Age,
                        CaptainCode = captain.PartnerCode,
                        CaptainName = captain.FirstName + " " + captain.LastName,
                        CaptainPhoneNumber = captain.PhoneNumber,
                        Email = captain.Email,
                        PictureUrl = captain.PictureUrl
                    };
                    return captainDetails;
                }
                throw new GenericException("You are not authorized to use this feature");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCaptainByIdAsync(int captainId)
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);

                if (currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "Administrator")
                {
                    var captain = await _uow.CaptainRepository.GetCaptainByIdAsync(captainId);
                    if (captain == null)
                    {
                        throw new GenericException($"Captain with Id {captain.PartnerId} does not exist");
                    }
                    _uow.CaptainRepository.Remove(captain);
                    await _uow.CompleteAsync();
                }
                else
                {
                    throw new GenericException("You are not authorized to use this feature");
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
