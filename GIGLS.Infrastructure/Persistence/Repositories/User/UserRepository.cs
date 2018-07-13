using GIGLS.Core.IRepositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Core.DTO.User;
using Microsoft.AspNet.Identity;
using GIGLS.CORE.Domain;
using System.Security.Claims;
using GIGLS.Core.Enums;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.User
{
    public class UserRepository : AuthRepository<GIGL.GIGLS.Core.Domain.User, GIGLSContext>, IUserRepository//IUserRepository
    {
        public UserRepository(GIGLSContext context) : base(context)
        {

        }

        public Task<GIGL.GIGLS.Core.Domain.User> GetUserByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            return user;
        }

        public Task<GIGL.GIGLS.Core.Domain.User> GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            return user;
        }

        public Task<GIGL.GIGLS.Core.Domain.User> GetUserById(int id)
        {
            var user = _repo.Get(id);
            return Task.FromResult(user);
        }

        public Task<GIGL.GIGLS.Core.Domain.User> GetUserByName(string userName)
        {
            var user = _userManager.FindByNameAsync(userName);
            return user;
        }

        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsers()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && x.UserType != UserType.System 
                        && x.UserChannelType == UserChannelType.Employee).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }

        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCustomerUsers()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && (x.UserChannelType == UserChannelType.Corporate 
                        || x.UserChannelType == UserChannelType.Ecommerce || x.UserChannelType == UserChannelType.IndividualCustomer)).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }

        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetCorporateCustomerUsers()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && (x.UserChannelType == UserChannelType.Corporate
                        || x.UserChannelType == UserChannelType.Ecommerce)).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }


        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetPartnerUsers()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && x.UserChannelType == UserChannelType.Partner).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }

        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetSystemUsers()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && x.UserType == UserType.System).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }

        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetDispatchCaptains()
        {
            var user = _userManager.Users.Where(x => x.IsDeleted == false && x.UserType != UserType.System
                        && x.UserChannelType == UserChannelType.Employee
                        && (x.SystemUserRole == "Dispatch Rider" || x.SystemUserRole == "Captain")).AsEnumerable();
            return Task.FromResult(user.OrderBy(x => x.FirstName).AsEnumerable());
        }

        public async Task<IdentityResult> UpdateUser(string userId, GIGL.GIGLS.Core.Domain.User user)
        {

            //var updateuser = await _userManager.FindByIdAsync(userId);

            //updateuser.Department = user.Department;
            //updateuser.Designation = user.Designation;
            //updateuser.Email = user.Email;
            //updateuser.FirstName = user.FirstName;
            //updateuser.LastName = user.LastName;
            //user.Gender = user.Gender;
            //updateuser.Organisation = user.Organisation;
            //updateuser.IsActive = user.IsActive;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> RegisterUser(GIGL.GIGLS.Core.Domain.User user, string password)
        {
            //var user = new GIGL.GIGLS.Core.Domain.User()
            //{

            //    FirstName = userdto.FirstName,
            //    LastName = userdto.LastName,
            //    //Gender = userdto.Gender,
            //    Email = userdto.Email,
            //    DateCreated = DateTime.Now.Date,
            //    DateModified = DateTime.Now.Date,
            //    UserName = userdto.Username,
            //};
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> AddRole(string name)
        {
            var role = new AppRole(name);
            role.DateCreated = DateTime.Now.Date;
            role.DateModified = DateTime.Now.Date;

            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public Task<AppRole> GetRoleById(string roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId);
            return role;
        }

        public Task<AppRole> GetRoleByName(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName);
            return role;
        }

        public Task<IEnumerable<AppRole>> GetRoles()
        {
            var role = _roleManager.Roles.Where(x => x.IsDeleted == false).AsEnumerable();
            return Task.FromResult(role.OrderBy(x => x.Name).AsEnumerable());
        }

        public async Task<IdentityResult> Remove(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.IsDeleted = true;
            return await _userManager.UpdateAsync(user);

            //return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> RemoveRole(string roleId)
        {
            //var rolResult = await _roleManager.FindByIdAsync(roleId);
            //var result = _roleManager.DeleteAsync(rolResult);
            //return await result;

            var rolResult = await _roleManager.FindByIdAsync(roleId);
            rolResult.IsDeleted = true;
            rolResult.DateModified = DateTime.Now.Date;
            return await _roleManager.UpdateAsync(rolResult);
        }

        public async Task<IdentityResult> UpdateRole(string roleId, RoleDTO roleDTO)
        {
            var rolResult = await _roleManager.FindByIdAsync(roleId);
            rolResult.Name = roleDTO.Name;
            rolResult.DateModified = DateTime.Now.Date;
            return await _roleManager.UpdateAsync(rolResult);
        }

        //Add a user to a role
        public async Task<IdentityResult> AddToRoleAsync(string userid, string name)
        {
            //var user = await this.GetUserById(userid);
            var Result = await _userManager.AddToRoleAsync(userid, name);
            return Result;
        }

        //Returns a list of roles a user has
        public async Task<IList<string>> GetUserRoles(string userid)
        {
            var Result = await _userManager.GetRolesAsync(userid);
            return Result;
        }

        //Returns true if the user with the specified ID is a member of the role
        public async Task<bool> IsInRoleAsync(string roleId, string name)
        {
            var Result = await _userManager.IsInRoleAsync(roleId, name);
            return Result;
        }

        //Remove a user with specified id from the specified role name
        public async Task<IdentityResult> RemoveFromRoleAsync(string roleId, string name)
        {
            var Result = await _userManager.RemoveFromRoleAsync(roleId, name);
            return Result;
        }

        public async Task<IdentityResult> AddClaimAsync(string userid, Claim claim)
        {
            var Result = await _userManager.AddClaimAsync(userid, claim);
            return Result;
        }

        public async Task<IdentityResult> RemoveClaimAsync(string userid, Claim claim)
        {
            var Result = await _userManager.RemoveClaimAsync(userid, claim);
            return Result;
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userid)
        {
            var Result = await _userManager.GetClaimsAsync(userid);
            return Result;
        }

        public async Task<IdentityResult> ResetPassword(string userid, string password)
        {
            await _userManager.RemovePasswordAsync(userid);
            var Result = await _userManager.AddPasswordAsync(userid, password);
            return Result;
        }

        public async Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            var Result = await _userManager.ChangePasswordAsync(userid, currentPassword, newPassword);
            return Result;
        }

        public async Task<bool> CheckPasswordAsync(GIGL.GIGLS.Core.Domain.User user, string password)
        {
            if (await _userManager.CheckPasswordAsync(user, password))
                return true;

            return false;
        }
    }
}
