using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetJobCardService : IFleetJobCardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public FleetJobCardService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<FleetJobCardDTO>> GetFleetJobCardsAsync()
        {
            try
            {
                var currentUserRole = await GetCurrentUserRoleAsync();

                if (currentUserRole == "Administrator" || currentUserRole == "CaptainManagement")
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

        private async Task<string> GetCurrentUserRoleAsync()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            return currentUser.SystemUserRole;
        }
    }
}
