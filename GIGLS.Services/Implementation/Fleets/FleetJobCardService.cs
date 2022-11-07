using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIGL.POST.Core.Domain;
using POST.Core;
using POST.Core.DTO.Captains;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.MessagingLog;
using POST.CORE.DTO.Report;
using POST.Core.DTO.User;
using POST.Core.Enums;
using POST.Core.IMessageService;
using POST.Core.IServices;
using POST.Core.IServices.Fleets;
using POST.Core.IServices.Partnership;
using POST.Core.IServices.User;
using POST.Infrastructure;
using POST.Core.IServices.Node;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Node;

namespace POST.Services.Implementation.Fleets
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

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
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
                NewNodeResponse response = new NewNodeResponse();
                var messageDtos = new List<MessageDTO>();

                foreach (var veh in jobDto.JobCardItems)
                {
                    if (!(await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == veh.VehicleNumber.Trim().ToLower())))
                    {
                        throw new GenericException($"Fleet/Vehicle with Registration Number: {veh.VehicleNumber} does not exist!");
                    }
                }

                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    var fleetJobs = new List<FleetJobCard>();
                    foreach (var job in jobDto.JobCardItems)
                    {
                        var fleet = _uow.Fleet.SingleOrDefault(x => x.RegistrationNumber.ToLower().Trim() == job.VehicleNumber.ToLower().Trim());
                        if (fleet.EnterprisePartnerId == null)
                        {
                            throw new GenericException($"The Fleet/Vehicle with Registration Number: {job.VehicleNumber} does not have associated Enterprise partner or Vehicle owner to complete this operation.");
                        }

                        var enterprisePartner = await _userService.GetUserById(fleet.EnterprisePartnerId);
                        fleetJobs.Add(new FleetJobCard
                        {
                            Status = FleetJobCardStatus.Open.ToString(),
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            FleetManagerId = currentUser.Id,
                            FleetOwnerId = enterprisePartner.Id,
                            VehiclePartToFix = job.VehiclePartToFix,
                            FleetId = fleet.FleetId,
                            Amount = job.Amount,
                            VehicleNumber = job.VehicleNumber
                        });

                        //prepare message for new open job card
                        messageDtos.Add(new MessageDTO
                        {
                            CustomerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                            ToEmail = enterprisePartner.Email,
                            To = enterprisePartner.Email,
                            FleetEnterprisePartnerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                            VehicleName = fleet.FleetName,
                            VehicleNumber = job.VehicleNumber,
                            VehiclePartToFix = job.VehiclePartToFix,
                            Amount = job.Amount.ToString(),
                            FleetOfficer = $"{currentUser.FirstName} {currentUser.LastName}",

                        });

                        // push notification
                        response = await PushNewNotification(enterprisePartner.Id, "New JobCard open", $"Vehicle Number: {job.VehicleNumber}, Part to fix: {job.VehiclePartToFix}, Amount: {job.Amount}, Created By: {currentUser.FirstName} {currentUser.LastName}");

                    }
                    _uow.FleetJobCard.AddRange(fleetJobs);
                    await _uow.CompleteAsync();

                    //send messages for new open job card
                    foreach (var msg in messageDtos)
                    {
                        await _messageSenderService.SendEmailOpenJobCardAsync(msg);
                    }

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

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    dto.FleetManagerId = currentUser.Id;

                    //if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "FleetCoordinator")
                    //{
                    //    dto.IsAdmin = true;
                    //}
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
        
        public async Task<FleetJobCardDto> GetFleetJobCardByIdAsync(int jobCardId)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    var jobCard = await _uow.FleetJobCard.GetFleetJobCardByIdAsync(jobCardId);
                    
                    if (jobCard != null)
                    {
                        var fleetManager = await _uow.User.GetUserById(jobCard.FleetManagerId);
                        var jobCardDto = new FleetJobCardDto()
                        {
                            FleetJobCardId = jobCard.FleetJobCardId,
                            DateCreated = jobCard.DateCreated,
                            DateModified = jobCard.DateModified,
                            Status = jobCard.Status,
                            VehiclePartToFix = jobCard.VehiclePartToFix,
                            FleetManagerId = jobCard.FleetManagerId,
                            Amount = jobCard.Amount,
                            VehicleNumber = jobCard.VehicleNumber,
                            RevenueStatus = jobCard.Fleet != null ? jobCard.Fleet.IsFixed.ToString() : null,
                            FleetOwnerId = jobCard.FleetOwnerId,
                            FleetOwner = jobCard.FleetOwner != null ? $"{jobCard.FleetOwner.FirstName} {jobCard.FleetOwner.LastName}" : null,
                            FleetManager = fleetManager != null ? $"{fleetManager.FirstName} {fleetManager.LastName}": null,
                            PaymentReceiptUrl = jobCard.PaymentReceiptUrl
                        };
                        return await Task.FromResult(jobCardDto);
                    }
                    throw new GenericException($"No jobcard with the id: {jobCardId} found!");
                }

                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CloseJobCardAsync(CloseJobCardDto jobCardDto)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    var jobCard = await _uow.FleetJobCard.GetFleetJobCardByIdAsync(jobCardDto.FleetJobCardId);

                    jobCard.Status = FleetJobCardStatus.Closed.ToString();
                    jobCard.PaymentReceiptUrl = jobCardDto.ReceiptUrl;

                    var jobcarddto = Mapper.Map<FleetJobCardDto> (jobCard);

                    //Add maintenance fee
                    await AddMaintenanceAmountToFleetTransaction(jobcarddto);

                    await _uow.CompleteAsync();

                    VehicleDetailsDTO fleet = await _captainService.GetVehicleByRegistrationNumberAsync(jobCard.VehicleNumber);
                    var enterprisePartner = await _userService.GetUserById(fleet.VehicleOwnerId);

                    //prepare message for new open job card
                    var msg = new MessageDTO()
                    {
                        CustomerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                        ToEmail = enterprisePartner.Email,
                        To = enterprisePartner.Email,
                        FleetEnterprisePartnerName = $"{enterprisePartner.FirstName} {enterprisePartner.LastName}",
                        VehicleName = fleet.FleetName,
                        VehicleNumber = jobCard.VehicleNumber,
                        VehiclePartToFix = jobCard.VehiclePartToFix,
                        Amount = jobCard.Amount.ToString(),
                        FleetOfficer = $"{currentUser.FirstName} {currentUser.LastName}",
                        Subject = $"Job/Maintenance Card for Vehicle: {jobCard.VehicleNumber} closed"
                    };
                    await _messageSenderService.SendEmailCloseJobCardAsync(msg);

                    // push notification
                    var response = await PushNewNotification(enterprisePartner.Id, $"Job/Maintenance Card for Vehicle: {jobCard.VehicleNumber} closed", $"Vehicle Number: {jobCard.VehicleNumber}, Part fixed: {jobCard.VehiclePartToFix}, Amount: {jobCard.Amount}, Created By: {currentUser.FirstName} {currentUser.LastName}");

                    return await Task.FromResult(true);
                }

                throw new GenericException("You are not authorized to perform this operation");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetJobCardByDateDto>> GetAllFleetJobCardsByInCurrentMonthAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "Admin" || currentUser.SystemUserRole == "CaptainManagement" || currentUser.SystemUserRole == "FleetCoordinator")
                {
                    var dto = new GetFleetJobCardByDateRangeDto();

                    dto.FleetManagerId = currentUser.Id;
                    var jobCards = await _uow.FleetJobCard.GetFleetJobCardsInCurrentMonthAsync(dto);
                    
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

        private async Task<NewNodeResponse> PushNewNotification(string customerId, string title, string message)
        {
            NewNodeResponse response = new NewNodeResponse();
            if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
            {
                //push notification 
                var payload = new PushNotificationMessageDTO
                {
                    CustomerId = customerId,
                    Title = title,
                    Message = message
                };
                response = await _nodeService.PushNotificationsToEnterpriseAPI(payload);
            }
            return response;
        }

        private async Task AddMaintenanceAmountToFleetTransaction(FleetJobCardDto jobCard)
        {
            try
            {
                //Get fleet Id
                var fleetDto = _uow.Fleet.GetAllAsQueryable().Where(x => x.RegistrationNumber.ToLower() == jobCard.VehicleNumber.ToLower()).FirstOrDefault();

                if (fleetDto == null)
                    throw new GenericException("Fleet details not found");

                var maintenanceTransaction = new FleetPartnerTransaction
                {
                    MovementManifestNumber = string.Empty,
                    CreditDebitType = CreditDebitType.Debit,
                    Amount = jobCard.Amount,
                    PaymentType = PaymentType.Wallet,
                    PaymentTypeReference = $"Maintenance-{jobCard.VehicleNumber}-{DateTime.Now.ToString()}",
                    Description = $"Maintenance amount for {jobCard.VehicleNumber} on {DateTime.Now.ToString()}",
                    FleetRegistrationNumber = jobCard.VehicleNumber,
                    DateOfEntry = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsSettled = false,
                    FleetId = fleetDto.FleetId
                };

                //Add maintenance amount to transaction table
                _uow.FleetPartnerTransaction.Add(maintenanceTransaction);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
