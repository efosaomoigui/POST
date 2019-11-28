using GIGLS.Core.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.Domain;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.User
{
    public interface IUserService : IServiceDependencyMarker
    {
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCustomerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCorporateCustomerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetPartnerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetSystemUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetDispatchCaptains();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetDispatchRiders();
        Task<UserDTO> GetUserById(string userId);
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO> GetUserById(int userId);
        Task<UserDTO> GetUserByChannelCode(string channelCode);
        Task<IdentityResult> AddUser(UserDTO user);
        Task<IdentityResult> UpdateUser(string id, UserDTO user);
        Task<IdentityResult> ActivateUser(string id, bool val);
        Task<IdentityResult> RemoveUser(string userId);

        Task<AppRole> GetRoleById(string roleId);
        Task<AppRole> GetRoleByName(string roleName);
        Task<IEnumerable<AppRole>> GetRoles();
        Task<IdentityResult> AddRole(RoleDTO role);
        Task<IdentityResult> RemoveRole(string roleId);
        Task<IdentityResult> UpdateRole(string roleId, RoleDTO roleDTO);

        Task<IdentityResult> AddToRoleAsync(string userid, string name);
        Task<IList<string>> GetUserRoles(string userid);
        Task<bool> IsInRoleAsync(string roleId, string name);
        Task<IdentityResult> RemoveFromRoleAsync(string userid, string name);

        //Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsersInRole(string roleId);    AddClaimAsync
        Task<IdentityResult> AddClaimAsync(string userid, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(string userid, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(string userid);

        Task<bool> RoleSettings(string systemuserid, string userid);

        Task<string> GetCurrentUserId();
        Task<bool> CheckPriviledge();
        Task<int[]> GetPriviledgeServiceCenters();
        Task<ServiceCentreDTO[]> GetCurrentServiceCenter();
        Task<ServiceCentreDTO> GetDefaultServiceCenter();
        Task<bool> CheckSCA();
        Task<UserDTO> retUser();

        Task<IdentityResult> ResetPassword(string userid, string password);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
        Task<IdentityResult> ResetExpiredPassword(string userid, string currentPassword, string newPassword);

        Task<UserDTO> GetUserByPhone(string PhoneNumber);

        IQueryable<GIGL.GIGLS.Core.Domain.User> GetCorporateCustomerUsersAsQueryable();

        Task<int[]> GetPriviledgeCountryIds();
        Task<List<CountryDTO>> GetPriviledgeCountrys();
        Task<string> GetUserChannelCode();

        Task<bool> UserActiveCountrySettings(string userid, int countryId);

        Task<int[]> GetPriviledgeServiceCenters(string currentUserId);
        Task<int[]> GetPriviledgeCountryIds(string currentUserId);
        Task<List<CountryDTO>> GetPriviledgeCountrys(string currentUserId);
        Task<IdentityResult> ForgotPassword(string email, string password);

        Task<CountryDTO> GetUserActiveCountry();
        Task<int> GetUserActiveCountryId();
        Task<int[]> GetRegionServiceCenters(string currentUserId);

        Task<bool> IsUserHasAdminRole(string userId);
        Task<UserDTO> GetUserUsingCustomer(string emailPhoneCode);
        Task<UserDTO> GetActivatedUserByEmail(string email, bool isActive);
    }
}
