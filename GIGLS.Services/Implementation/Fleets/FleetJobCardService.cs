﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.DTO;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetJobCardService : IFleetJobCardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly ICaptainService _captainService;
        private readonly IFleetService _fleetService;
        private readonly IFleetPartnerService _fleetPartnerService;
        private readonly IPartnerService _partnerService;
        private readonly INodeService _nodeService;

        public FleetJobCardService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService, ICaptainService captainService, IFleetService fleetService, IFleetPartnerService fleetPartnerService, IPartnerService partnerService, INodeService nodeService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _captainService = captainService;
            _fleetService = fleetService;
            _fleetPartnerService = fleetPartnerService;
            _partnerService = partnerService;
            MapperConfig.Initialize();
            _nodeService = nodeService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    return await _uow.FleetJobCard.GetFleetJobCardsAsync();
                }
                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> OpenFleetJobCardsAsync(NewJobCard jobDto)
        {
            try
            {
                foreach (var veh in jobDto.JobCardItems)
                {
                    if (!(await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == veh.VehicleNumber.Trim().ToLower())))
                    {
                        throw new GenericException($"Fleet/Vehicle with Registration Number: {veh.VehicleNumber} does not exist!");
                    }
                }

                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    foreach (var fleetJob in jobDto.JobCardItems)
                    {
                        //VehicleDetailsDTO fleet = await _captainService.GetVehicleByRegistrationNumberAsync(fleetJob.VehicleNumber);
                        var fleet = _uow.Fleet.SingleOrDefault(x => x.RegistrationNumber.ToLower().Trim() == fleetJob.VehicleNumber.ToLower().Trim());
                        if (fleet.EnterprisePartnerId == null)
                        {
                            throw new GenericException($"The Fleet/Vehicle with Registration Number: {fleetJob.VehicleNumber} does not have associated Enterprise partner or Vehicle owner to complete this operation.");
                        }

                        var enterprisePartner = await _userService.GetUserById(fleet.EnterprisePartnerId);

                        var newFleetJob = new FleetJobCard()
                        {
                            Status = FleetJobCardStatus.Open.ToString(),
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            FleetManagerId = currentUser.Id,
                            FleetOwnerId = enterprisePartner.Id,
                            VehiclePartToFix = fleetJob.VehiclePartToFix,
                            FleetId = fleet.FleetId,
                            Amount = fleetJob.Amount,
                            VehicleNumber = fleetJob.VehicleNumber
                        };

                        _uow.FleetJobCard.Add(newFleetJob);

                        //send message for new open job card
                        var messageDTO = new MessageDTO
                        {
                            CustomerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                            ToEmail = enterprisePartner.Email,
                            To = enterprisePartner.Email,
                            FleetEnterprisePartnerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                            VehicleName = fleet.FleetName,
                            VehicleNumber = fleetJob.VehicleNumber,
                            VehiclePartToFix = fleetJob.VehiclePartToFix,
                            Amount = fleetJob.Amount.ToString(),
                            FleetOfficer = $"{currentUser.FirstName} {currentUser.LastName}",
                            
                        };
                        await _messageSenderService.SendEmailOpenJobCardAsync(messageDTO);

                        // push notification
                        await PushNewNotification(enterprisePartner.Id, "New JobCard open", $"Vehicle Number: {fleetJob.VehicleNumber}, Part to fix: {fleetJob.VehiclePartToFix}, Amount: {fleetJob.Amount}, Created By: {currentUser.FirstName} {currentUser.LastName}");
                        
                    }
                    await _uow.CompleteAsync();
                    return true;
                }
                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    dto.FleetManagerId = currentUser.Id;
                    var fleetJobCards = await _uow.FleetJobCard.GetFleetJobCardByDateRangeAsync(dto);

                    return fleetJobCards;
                }
                throw new GenericException("You are not authorized to perform this operation");
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsByFleetManagerAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    return await _uow.FleetJobCard.GetFleetJobCardsByFleetManagerAsync(currentUser.Id);
                }
                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetJobCardDto> GetFleetJobCardByIdAsync(int jobCardId)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    var jobCard = await _uow.FleetJobCard.GetFleetJobCardByIdAsync(jobCardId);
                    var jobCardDto = new FleetJobCardDto()
                    {
                        FleetJobCardId = jobCard.FleetJobCardId,
                        DateCreated = jobCard.DateCreated,
                        DateModified = jobCard.DateModified,
                        Status = jobCard.Status,
                        VehiclePartToFix = jobCard.VehiclePartToFix,
                        FleetManagerId = jobCard.FleetManagerId,
                        Amount = jobCard.Amount,
                        VehicleNumber = jobCard.VehicleNumber
                    };
                    return await Task.FromResult(jobCardDto);
                }

                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CloseJobCardByIdAsync(int jobCardId)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    var jobCard = await _uow.FleetJobCard.GetFleetJobCardByIdAsync(jobCardId);

                    jobCard.Status = FleetJobCardStatus.Closed.ToString();
                    await _uow.CompleteAsync();

                    VehicleDetailsDTO fleet = await _captainService.GetVehicleByRegistrationNumberAsync(jobCard.VehicleNumber);
                    var enterprisePartner = await _userService.GetUserById(fleet.VehicleOwnerId);

                    var passwordMessage = new PasswordMessageDTO()
                    {
                        Password = $"Maintenance Card has been closed for this vehicle {jobCard.VehicleNumber}",
                        UserEmail = enterprisePartner.Email,
                        CustomerCode = $"Part to fixed: {jobCard.VehiclePartToFix}. \nAmount spent: {jobCard.Amount}"
                    };
                    await _messageSenderService.SendGenericEmailMessage(MessageType.FPEmail, passwordMessage);

                    return await Task.FromResult(true);
                }

                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetJobCardByDateDto>> GetFleetJobCardsByFleetManagerInCurrentMonthAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    var jobCards = await _uow.FleetJobCard.GetFleetJobCardsByFleetManagerInCurrentMonthAsync(new GetFleetJobCardByDateRangeDto(){FleetManagerId = currentUser.Id, });
                    
                    return await Task.FromResult(jobCards);
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

        private async Task PushNewNotification(string customerId, string title, string message)
        {
            if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
            {
                //push notification 
                var payload = new PushNotificationMessageDTO
                {
                    CustomerId = customerId,
                    Title = title,
                    Message = message
                };
                var notification = await _nodeService.PushNotificationsToEnterpriseAPI(payload);
            }
        }
    }
}
