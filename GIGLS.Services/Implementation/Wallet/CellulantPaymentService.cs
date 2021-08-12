using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CellulantPaymentService : ICellulantPaymentService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private IServiceCentreService _serviceCenterService;
        public CellulantPaymentService( IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            MapperConfig.Initialize();
        }

        public Task<TransferDetailsDTO> GetAllTransferDetails(string reference)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria baseFilter)
        {
            var crAccount = await GetServiceCentreCrAccount();

            if (string.IsNullOrWhiteSpace(crAccount))
            {
                throw new GenericException($"Service centre does not have a CRAccount.");
            }

            var transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter, crAccount);
            return transferDetailsDto;
        }

        public async Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber)
        {
            var crAccount = await GetServiceCentreCrAccount();

            if (string.IsNullOrWhiteSpace(crAccount))
            {
                throw new GenericException($"Service centre does not have a CRAccount.");
            }

            var transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber, crAccount);
            return transferDetailsDto;
        }

        private async Task<string> GetServiceCentreCrAccount()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var userRoles = await _userService.GetUserRoles(currentUserId);
            var userClaims = await _userService.GetClaimsAsync(currentUserId);

            string[] claimValue = null;
            string crAccount = "";
            foreach (var claim in userClaims)
            {
                if (claim.Type == "Privilege")
                {
                    claimValue = claim.Value.Split(':');   // format stringName:stringValue
                }
            }

            if (claimValue == null)
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            if (claimValue[0] == "ServiceCentre")
            {
                crAccount = await _uow.ServiceCentre.GetServiceCentresCrAccount(int.Parse(claimValue[1]));
            }
            else
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            return crAccount;
        }

        private async Task<string> GetUserRolesAndClaims()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var userRoles = await _userService.GetUserRoles(currentUserId);
            var userClaims = await _userService.GetClaimsAsync(currentUserId);

            string[] claimValue = null;
            string crAccount = "";
            foreach (var claim in userClaims)
            {
                if (claim.Type == "Privilege")
                {
                    claimValue = claim.Value.Split(':');   // format stringName:stringValue
                }
            }

            if (claimValue == null)
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            if (claimValue[0] == "ServiceCentre")
            {
                crAccount = await _uow.ServiceCentre.GetServiceCentresCrAccount(int.Parse(claimValue[1]));
            }
            else
            {
                throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            return crAccount;
        }

    }
}
