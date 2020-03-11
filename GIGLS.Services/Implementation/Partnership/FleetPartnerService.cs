using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class FleetPartnerService : IFleetPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IMessageSenderService _messageSenderService;

        public FleetPartnerService(IUnitOfWork uow, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IUserService userService, ICompanyService companyService, IPasswordGenerator passwordGenerator, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _userService = userService;
            _companyService = companyService;
            _passwordGenerator = passwordGenerator;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }


        public async Task<object> AddFleetPartner(FleetPartnerDTO fleetPartnerDTO)
        {
            if (fleetPartnerDTO.PartnerName != null)
            {
                fleetPartnerDTO.PartnerName.Trim();
            }

            if (fleetPartnerDTO.Email != null)
            {
                fleetPartnerDTO.Email = fleetPartnerDTO.Email.Trim().ToLower();
            }


            //Pending
            ////update the customer update to have country code added to it
            //if (fleetPartnerDTO.PhoneNumber.StartsWith("0"))
            //{
            //    fleetPartnerDTO.PhoneNumber = await _companyService.AddCountryCodeToPhoneNumber(fleetPartnerDTO.PhoneNumber, fleetPartnerDTO.UserActiveCountryId);
            //}

            if (await _uow.FleetPartner.ExistAsync(v => v.PhoneNumber == fleetPartnerDTO.PhoneNumber || v.Email == fleetPartnerDTO.Email))
            {
                throw new GenericException("Fleet Partner Already Exists");
            }

            var EmailUser = await _uow.User.GetUserByEmailorPhoneNumber(fleetPartnerDTO.Email, fleetPartnerDTO.PhoneNumber);

            if (EmailUser != null)
            {
                throw new GenericException("Customer already exists");
            }

            var fleetPartnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.FleetPartner);
            var fleetPartner = Mapper.Map<FleetPartner>(fleetPartnerDTO);
            fleetPartner.FleetPartnerCode = fleetPartnerCode;
            
            string password = await _passwordGenerator.Generate();

            var user = new GIGL.GIGLS.Core.Domain.User
            {
                Email = fleetPartner.Email,
                FirstName = fleetPartner.FirstName,
                LastName = fleetPartner.LastName,
                PhoneNumber = fleetPartner.PhoneNumber,
                UserType = UserType.Regular,
                UserName = fleetPartner.Email,
                UserChannelCode = fleetPartner.FleetPartnerCode,
                UserChannelPassword = password,
                UserChannelType = UserChannelType.FleetPartner,
                UserActiveCountryId = fleetPartner.UserActiveCountryId,
                IsActive = true,
                DateCreated = DateTime.Now.Date,
                DateModified = DateTime.Now.Date,
                PasswordExpireDate = DateTime.Now                
            };

            user.Id = Guid.NewGuid().ToString();
            
            var u = await _uow.User.RegisterUser(user, password);

            if (u.Succeeded)
            {
                fleetPartner.UserId = user.Id;
                _uow.FleetPartner.Add(fleetPartner);
                await _uow.CompleteAsync();
            }

            var loginURL = ConfigurationManager.AppSettings["FleetPartnersUrl"];

            var passwordMessage = new PasswordMessageDTO()
            {
                Password = password,
                UserEmail = fleetPartner.Email,
                URL = loginURL
            };

            await _messageSenderService.SendGenericEmailMessage(MessageType.FPEmail, passwordMessage);
            return new { id = fleetPartner.FleetPartnerId };
        }

        public async Task UpdateFleetPartner(int partnerId, FleetPartnerDTO fleetPartnerDTO)
        {
            var existingParntner = await _uow.FleetPartner.GetAsync(partnerId);

            if (existingParntner == null)
            {
                throw new GenericException("Fleet Partner Does not Exist");
            }

            existingParntner.FirstName = fleetPartnerDTO.FirstName.Trim();
            existingParntner.LastName = fleetPartnerDTO.LastName.Trim();
            existingParntner.Email = fleetPartnerDTO.Email.Trim();
            existingParntner.PartnerName = fleetPartnerDTO.PartnerName.Trim();
            existingParntner.Address = fleetPartnerDTO.Address;
            existingParntner.OptionalPhoneNumber = fleetPartnerDTO.OptionalPhoneNumber;
            existingParntner.PhoneNumber = fleetPartnerDTO.PhoneNumber;
            await _uow.CompleteAsync();
        }

        public async Task RemoveFleetPartner(int partnerId)
        {
            var existingPartner = await _uow.FleetPartner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("Fleet Partner does not exist");
            }
            _uow.FleetPartner.Remove(existingPartner);
            await _uow.CompleteAsync();
        }

        public async Task<IEnumerable<FleetPartnerDTO>> GetFleetPartners()
        {
            var partners = await _uow.FleetPartner.GetFleetPartnersAsync();
            return partners;
        }

        public async Task<FleetPartnerDTO> GetFleetPartnerById(int partnerId)
        {
            var partnerDto = new FleetPartnerDTO();
            var partner = await _uow.FleetPartner.GetAsync(partnerId);

            if (partner == null)
            {
                throw new GenericException("Fleet Partner does not exist");
            }
            else
            {
                partnerDto = Mapper.Map<FleetPartnerDTO>(partner);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);

            }
            return partnerDto;
        }

        public async Task<int> CountOfPartnersUnderFleet()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var partners = await _uow.FleetPartner.FleetCount(currentUser.UserChannelCode);
            return partners;
        }
        public async Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var vehicles = await _uow.FleetPartner.GetVehiclesAttachedToFleetPartner(currentUser.UserChannelCode);
            return vehicles;
        }

        public async Task<List<PartnerTransactionsDTO>> GetFleetTransaction(ShipmentCollectionFilterCriteria filterCriteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var partners = new List<PartnerTransactionsDTO>();

            if (filterCriteria.IsDashboard)
            {
                partners = await _uow.PartnerTransactions.GetRecentFivePartnerTransactionsForFleet(currentUser.UserChannelCode);
            }
            else
            {
                partners = await _uow.PartnerTransactions.GetPartnerTransactionsForFleet(filterCriteria, currentUser.UserChannelCode);
            }

            return await Task.FromResult(partners);
        }

    }
}
