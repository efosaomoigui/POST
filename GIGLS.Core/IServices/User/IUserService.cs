using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.Domain;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.User
{
    public interface IUserService : IServiceDependencyMarker
    {
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetSystemUsers();
        Task<UserDTO> GetUserById(string userId);
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO> GetUserById(int userId);
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
        Task<ServiceCentreDTO> GetDefaultServiceCenter();

        Task<IdentityResult> ResetPassword(string userid, string password);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
    }
}
