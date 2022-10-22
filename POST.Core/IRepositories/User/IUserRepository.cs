using GIGL.POST.Core.Repositories;
using POST.Core.DTO.User;
using POST.CORE.Domain;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.User
{
    public interface IUserRepository : IAuthRepository<GIGL.POST.Core.Domain.User> //where TEntity:class
    {
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetUsers();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetCustomerUsers(string email);
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetCustomerUsers();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetCorporateCustomerUsers();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetPartnerUsers();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetSystemUsers();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetDispatchCaptains();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetCaptains();
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetDispatchRiders();
        Task<GIGL.POST.Core.Domain.User> GetUserById(string id);
        Task<GIGL.POST.Core.Domain.User> GetUserByEmail(string email);
        Task<GIGL.POST.Core.Domain.User> GetUserById(int id);
        Task<GIGL.POST.Core.Domain.User> GetUserByChannelCode(string channelCode);
        Task<GIGL.POST.Core.Domain.User> GetUserByName(string userName); 
        Task<IdentityResult> RegisterUser(GIGL.POST.Core.Domain.User user, string password);
        Task<IdentityResult> UpdateUser(string userId, GIGL.POST.Core.Domain.User user);
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
        Task<bool> CheckPasswordAsync(GIGL.POST.Core.Domain.User user, string password);
        Task<GIGL.POST.Core.Domain.User> GetUserByPhoneNumber(string PhoneNumber);
        IQueryable<GIGL.POST.Core.Domain.User> GetCorporateCustomerUsersAsQueryable();

        Task<bool> IsUserHasAdminRole(string userId);
        Task<GIGL.POST.Core.Domain.User> GetUserByEmailorPhoneNumber(string email, string PhoneNumber);
        Task<List<GIGL.POST.Core.Domain.User>> GetUserListByEmailorPhoneNumber(string email, string PhoneNumber);
        Task<GIGL.POST.Core.Domain.User> GetUserUsingCustomer(string emailPhoneCode);
        Task<GIGL.POST.Core.Domain.User> ActivateUserByEmail(string email, bool isActive);
        Task<GIGL.POST.Core.Domain.User> GetUserUsingCustomerForCustomerPortal(string emailPhoneCode);
        Task<GIGL.POST.Core.Domain.User> GetUserUsingCustomerForMobileScanner(string emailPhoneCode);
        Task<bool> IsCustomerHasAgentRole(string userId);
        Task<GIGL.POST.Core.Domain.User> GetUserUsingCustomerForAgentApp(string emailPhoneCode);
        Task<List<GIGL.POST.Core.Domain.User>> GetUsers(string[] ids);
        Task<GIGL.POST.Core.Domain.User> GetUserByCompanyName(string name);
        Task<GIGL.POST.Core.Domain.User> GetEmployeeUserByEmail(string email);
        Task<List<GIGL.POST.Core.Domain.User>> GetPartnerUsersByEmail(string email);
        Task<IEnumerable<GIGL.POST.Core.Domain.User>> GetPartnerUsersByEmail2(string email);
        Task<GIGL.POST.Core.Domain.User> GetUserByEmailorCustomerCode(string emailOrCode);
        Task<List<GIGL.POST.Core.Domain.User>> GetUsersListByCode(List<string> codes);
    }
}
