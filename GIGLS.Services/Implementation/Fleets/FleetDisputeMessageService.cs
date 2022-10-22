using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.MessagingLog;
using POST.Core.DTO.Node;
using POST.Core.DTO.User;
using POST.Core.Enums;
using POST.Core.IMessageService;
using POST.Core.IServices;
using POST.Core.IServices.Fleets;
using POST.Core.IServices.Node;
using POST.Core.IServices.Partnership;
using POST.Core.IServices.User;
using POST.Infrastructure;

namespace POST.Services.Implementation.Fleets
{
    public class FleetDisputeMessageService : IFleetDisputeMessageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IFleetPartnerService _fleetPartnerService;
        private readonly IFleetService _fleetService;
        private readonly ICaptainService _captainService;
        private readonly INodeService _nodeService;

        public FleetDisputeMessageService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService, IPartnerService partnerService, IFleetPartnerService fleetPartnerService, IFleetService fleetService, ICaptainService captainService, INodeService nodeService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _fleetPartnerService = fleetPartnerService;
            _fleetService = fleetService;
            _captainService = captainService;
            _nodeService = nodeService;
        }

        public async Task<bool> AddFleetDisputeMessageAsync(FleetDisputeMessageDto dto)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    if (!(await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == dto.VehicleNumber.Trim().ToLower())))
                    {
                        throw new GenericException($"Fleet/Vehicle with Registration Number: {dto.VehicleNumber} does not exist!");
                    }

                    var fleet = _uow.Fleet.SingleOrDefault(x => x.RegistrationNumber.ToLower().Trim() == dto.VehicleNumber.ToLower().Trim());

                    var vehicle = await _captainService.GetVehicleByRegistrationNumberAsync(dto.VehicleNumber);
                    var fleetDisputeMessage = new FleetDisputeMessage()
                    {
                        DateModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        DisputeMessage = dto.DisputeMessage,
                        VehicleNumber = dto.VehicleNumber,
                        FleetOwnerId = vehicle.VehicleOwnerId
                    };
                    _uow.FleetDisputeMessage.Add(fleetDisputeMessage);
                    await _uow.CompleteAsync();

                    var vehicleOwner = await _userService.GetUserById(vehicle.VehicleOwnerId);

                    //send message for new open job card
                    var messageDTO = new MessageDTO
                    {
                        CustomerName = $"{vehicleOwner.FirstName} {vehicleOwner.LastName}",
                        ToEmail = vehicleOwner.Email,
                        To = vehicleOwner.Email,
                        FleetEnterprisePartnerName = $"{vehicleOwner.FirstName} {vehicleOwner.LastName}",
                        VehicleName = fleet.FleetName,
                        VehicleNumber = dto.VehicleNumber,                        
                        FleetOfficer = $"{currentUser.FirstName} {currentUser.LastName}",
                        DateOfDispute = fleetDisputeMessage.DateCreated.ToShortDateString(),
                        DisputeDetails = fleetDisputeMessage.DisputeMessage
                    };
                    await _messageSenderService.SendEmailFleetDisputeMessageAsync(messageDTO);

                    // push notification
                    var notificationResponse = await PushNewNotification(vehicleOwner.Id, "New Dispute Message Created", 
                        $"Vehicle Number: {dto.VehicleNumber}, Dispute Details: {dto.DisputeMessage}, Created By: {currentUser.FirstName} {currentUser.LastName}");

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

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
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

        private async Task<NewNodeResponse> PushNewNotification(string customerId, string title, string message)
        {
            NewNodeResponse notification = new NewNodeResponse();

            if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
            {
                //push notification 
                var payload = new PushNotificationMessageDTO
                {
                    CustomerId = customerId,
                    Title = title,
                    Message = message
                };
                notification = await _nodeService.PushNotificationsToEnterpriseAPI(payload);
            }
            return notification;
        }
    }
}
