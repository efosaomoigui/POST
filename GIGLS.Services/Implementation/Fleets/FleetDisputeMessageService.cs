using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetDisputeMessageService : IFleetDisputeMessageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IFleetPartnerService _fleetPartnerService;
        private readonly IFleetService _fleetService;
        private readonly ICaptainService _captainService;

        public FleetDisputeMessageService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService, IPartnerService partnerService, IFleetPartnerService fleetPartnerService, IFleetService fleetService, ICaptainService captainService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _fleetPartnerService = fleetPartnerService;
            _fleetService = fleetService;
            _captainService = captainService;
        }

        public async Task<bool> AddFleetDisputeMessageAsync(FleetDisputeMessageDto dto)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    if (!(await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == dto.VehicleNumber.Trim().ToLower())))
                    {
                        throw new GenericException($"Fleet/Vehicle with Registration Number: {dto.VehicleNumber} does not exist!");
                    }

                    var vehicleOwner = await _captainService.GetVehicleByRegistrationNumberAsync(dto.VehicleNumber);
                    var fleetDisputeMessage = new FleetDisputeMessage()
                    {
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        DisputeMessage = dto.DisputeMessage,
                        VehicleNumber = dto.VehicleNumber,
                        FleetOwnerId = vehicleOwner.VehicleOwnerId
                    };
                    _uow.FleetDisputeMessage.Add(fleetDisputeMessage);
                    await _uow.CompleteAsync();

                    var owner = await _userService.GetUserById(vehicleOwner.VehicleOwnerId);

                    // send mail
                    var disputeMailDto = new FleetDisputeMessageMailDto()
                    {
                        FleetOwnerEmail = owner.Email,
                        DisputeMessage = dto.DisputeMessage,
                        FleetManager = $"{currentUser.FirstName} {currentUser.LastName}",
                        VehicleNumber = dto.VehicleNumber
                    };
                    await _messageSenderService.SendGenericEmailMessage(MessageType.DISPUTEMSGEMAIL, disputeMailDto);

                    return true;
                }
                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetDisputeMessageDto>> GetAllFleetDisputeMessagesAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    var disputeMassages = _uow.FleetDisputeMessage.GetAll("FleetOwner").Select(x => new FleetDisputeMessageDto()
                    {
                        DisputeMessage = x.DisputeMessage,
                        VehicleNumber = x.VehicleNumber,
                        FleetOwnerId = x.FleetOwnerId,
                    }).ToList();

                    return disputeMassages;
                }
                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<UserDTO> GetCurrentUserRoleAsync()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            return currentUser;
        }
    }
}
