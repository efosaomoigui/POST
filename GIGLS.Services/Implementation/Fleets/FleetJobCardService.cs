using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetJobCardService : IFleetJobCardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        //private readonly ICaptainService _captainService;

        public FleetJobCardService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement")
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

        public async Task<bool> OpenFleetJobCardsAsync(OpenFleetJobCardDto fleetJob)
        {
            try
            {
                var currentUser = await GetCurrentUserRoleAsync();

                if (currentUser.SystemUserRole == "Administrator" || currentUser.SystemUserRole == "CaptainManagement")
                {
                    var newFleetJob = Mapper.Map<FleetJobCard>(fleetJob);
                    //var fleetJobCard = new FleetJobCard()
                    //{

                    //};
                    newFleetJob.Status = FleetJobCardStatus.Open.ToString();
                    newFleetJob.DateModified = DateTime.Now;
                    newFleetJob.FleetManagerId = currentUser.Id;
                    newFleetJob.EnterprisePartnerId = fleetJob.EnterprisePartnerId;

                    _uow.FleetJobCard.Add(newFleetJob);
                    await _uow.CompleteAsync();

                    //await _messageSenderService.SendGenericEmailMessage(MessageType.FPEmail, passwordMessage);

                    return true;
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
