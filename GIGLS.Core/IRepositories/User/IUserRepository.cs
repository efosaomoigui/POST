using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.Domain;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.User
{
    public interface IUserRepository : IAuthRepository<GIGL.GIGLS.Core.Domain.User> //where TEntity:class
    {
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCustomerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCorporateCustomerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetPartnerUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetSystemUsers();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetDispatchCaptains();
        Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetDispatchRiders();
        Task<GIGL.GIGLS.Core.Domain.User> GetUserById(string id);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserByEmail(string email);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserById(int id);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserByChannelCode(string channelCode);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserByName(string userName); 
        Task<IdentityResult> RegisterUser(GIGL.GIGLS.Core.Domain.User user, string password);
        Task<IdentityResult> UpdateUser(string userId, GIGL.GIGLS.Core.Domain.User user);
        Task<IdentityResult> Remove(string userId);

        Task<AppRole> GetRoleById(string userId); 
        Task<IEnumerable<AppRole>> GetRoles();
        Task<IdentityResult> AddRole(string role); 
        Task<IdentityResult> RemoveRole(string roleId); 
        Task<IdentityResult> UpdateRole(string roleId, RoleDTO roleDTO);
        Task<IdentityResult> AddToRoleAsync(string roleId, string name);
        Task<IList<string>> GetUserRoles(string userid);
        Task<bool> IsInRoleAsync(string roleId, string name);
        Task<IdentityResult> RemoveFromRoleAsync(string roleId, string name);

        Task<IdentityResult> AddClaimAsync(string userid, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(string userid, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(string userid);

        Task<IdentityResult> ResetPassword(string userid, string password);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(GIGL.GIGLS.Core.Domain.User user, string password);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserByPhoneNumber(string PhoneNumber);
        IQueryable<GIGL.GIGLS.Core.Domain.User> GetCorporateCustomerUsersAsQueryable();

        Task<bool> IsUserHasAdminRole(string userId);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserByEmailorPhoneNumber(string email, string PhoneNumber);
        Task<List<GIGL.GIGLS.Core.Domain.User>> GetUserListByEmailorPhoneNumber(string email, string PhoneNumber);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserUsingCustomer(string emailPhoneCode);
        Task<GIGL.GIGLS.Core.Domain.User> ActivateUserByEmail(string email, bool isActive);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserUsingCustomerForCustomerPortal(string emailPhoneCode);
        Task<GIGL.GIGLS.Core.Domain.User> GetUserUsingCustomerForMobileScanner(string emailPhoneCode);
        Task<bool> IsCustomerHasAgentRole(string userId);
    }
}
