using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public CellulantPaymentService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService)
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
            var isAdmin = await CheckUserRoleIsAdmin();
            var isRegion = await CheckUserPrivilegeIsRegion();


            List<TransferDetailsDTO> transferDetailsDto = new List<TransferDetailsDTO>();

            if (!isAdmin && !isRegion)
            {
                var crAccount = await GetServiceCentreCrAccount();

                if (string.IsNullOrWhiteSpace(crAccount))
                {
                    throw new GenericException($"Service centre does not have a CRAccount.");
                }

                transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter, crAccount);
            }
            else
            {
                if (isRegion == true)
                {
                    var crAccounts = await GetRegionServiceCentresCrAccount();
                    if (crAccounts.Count > 0)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter, crAccounts);
                    }
                }
                else
                {
                    transferDetailsDto = await _uow.TransferDetails.GetTransferDetails(baseFilter);
                }
            }

            return transferDetailsDto;
        }

        public async Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber)
        {
            var isAdmin = await CheckUserRoleIsAdmin();
            var isRegion = await CheckUserPrivilegeIsRegion();

            List<TransferDetailsDTO> transferDetailsDto = new List<TransferDetailsDTO>();

            if (!isAdmin && !isRegion)
            {
                var crAccount = await GetServiceCentreCrAccount();

                if (string.IsNullOrWhiteSpace(crAccount))
                {
                    throw new GenericException($"Service centre does not have a CRAccount.");
                }

                transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber, crAccount);
            }
            else
            {
                if (isRegion == true)
                {
                    var crAccounts = await GetRegionServiceCentresCrAccount();
                    if (crAccounts.Count > 0)
                    {
                        transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber, crAccounts);
                    }
                }
                else
                {
                    transferDetailsDto = await _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber);
                }
            }

            return transferDetailsDto;
        }

        private async Task<string> GetServiceCentreCrAccount()
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
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

        private async Task<List<string>> GetRegionServiceCentresCrAccount()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                List<string> crAccounts = new List<string>();
                List<int> serviceCenterIds = new List<int>();
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

                if (claimValue[0] == "Region")
                {
                    var regionId = int.Parse(claimValue[1]);
                    serviceCenterIds = await _uow.RegionServiceCentreMapping.GetAllAsQueryable().Where(x => x.RegionId == regionId).Select(x => x.ServiceCentreId).ToListAsync();
                }
                else
                {
                    throw new GenericException($"User {currentUser.Username} does not have a priviledge region claim.");
                }

                if (serviceCenterIds.Count > 0)
                {
                    crAccounts = await _uow.ServiceCentre.GetAllAsQueryable().Where(x => serviceCenterIds.Contains(x.ServiceCentreId)).Select(x => x.CrAccount).ToListAsync();
                }
                return crAccounts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CheckUserRoleIsAdmin()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var userRoles = await _userService.GetUserRoles(currentUserId);

                bool isAdmin = false;
                foreach (var role in userRoles)
                {
                    if (role == "Admin")
                    {
                        isAdmin = true;   // set to true
                    }
                }

                return isAdmin;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckUserPrivilegeIsRegion()
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var userClaims = await _userService.GetClaimsAsync(currentUserId);

                string[] claimValue = null;
                bool isRegion = false;

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

                if (claimValue[0] == "Region")
                {
                    isRegion = true;
                }

                return isRegion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
