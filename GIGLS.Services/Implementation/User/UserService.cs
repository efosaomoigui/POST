using GIGLS.Core.IServices.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.User;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.Core;
using Microsoft.AspNet.Identity;
using System;
using GIGLS.CORE.Domain;
using System.Security.Claims;
using GIGLS.Core.IServices.ServiceCentres;
using System.Web;
using GIGLS.Core.Enums;
using System.Linq;

namespace GIGLS.Services.Implementation.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            MapperConfig.Initialize();
        }

        //Register a new user
        public async Task<IdentityResult> AddUser(UserDTO userDto)
        {
            if (await _unitOfWork.User.GetUserByEmail(userDto.Email.ToLower()) != null)
            {
                throw new GenericException($"User with email: {userDto.Email} already exist");
            }
            var usertemp = new GIGL.GIGLS.Core.Domain.User() { };

            var user = Mapper.Map<GIGL.GIGLS.Core.Domain.User>(userDto);

            user.Id = usertemp.Id;
            user.DateCreated = DateTime.Now.Date;
            user.DateModified = DateTime.Now.Date;
            user.UserName = user.Email;

            var u = await _unitOfWork.User.RegisterUser(user, userDto.Password);
            return u;
        }

        //Get all users
        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetUsers()
        {
            return _unitOfWork.User.GetUsers();
        }

        //Get all system users
        public Task<IEnumerable<GIGL.GIGLS.Core.Domain.User>> GetSystemUsers()
        {
            return _unitOfWork.User.GetSystemUsers();
        }

        //Get a user by Id using Guid from identity implement of EF
        public async Task<UserDTO> GetUserById(string Id)
        {
            string userActiveServiceCentre = null;

            var user = await _unitOfWork.User.GetUserById(Id);

            if (user != null)
            {
                var activeCentre = await _unitOfWork.UserServiceCentreMapping.GetAsync(x => x.IsActive == true && x.User.Id.Equals(Id));
                if (activeCentre != null)
                {
                    var serviceCentre = await _unitOfWork.ServiceCentre.GetAsync(x => x.ServiceCentreId == activeCentre.ServiceCentreId);
                    userActiveServiceCentre = serviceCentre.Name;
                }
            }
            else
            {
                throw new GenericException("User does not exist!");
            }

            var userDto = Mapper.Map<UserDTO>(user);

            userDto.UserActiveServiceCentre = userActiveServiceCentre;

            return userDto;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.User.GetUserByEmail(email);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            return Mapper.Map<UserDTO>(user);

        }

        //Get a user by Id using int custom UserId added to users table
        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await _unitOfWork.User.GetUserById(userId);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            return Mapper.Map<UserDTO>(user);

        }


        public async Task<IdentityResult> RemoveUser(string userId)
        {
            var user = await _unitOfWork.User.GetUserById(userId);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            return await _unitOfWork.User.Remove(userId);
        }

        public async Task<IdentityResult> UpdateUser(string userid, UserDTO userDto)
        {

            var user = await _unitOfWork.User.GetUserById(userid);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            user.Department = userDto.Department;
            user.Designation = user.Designation;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Gender = userDto.Gender;
            user.Organisation = userDto.Organisation;
            user.IsActive = userDto.IsActive;
            user.PhoneNumber = userDto.PhoneNumber;
            user.SystemUserId = userDto.SystemUserId;
            user.SystemUserRole = userDto.SystemUserRole;

            return await _unitOfWork.User.UpdateUser(userid, user);

            //var userdomain = Mapper.Map<GIGL.GIGLS.Core.Domain.User>(userDto);
            //userdomain.Id = userid;
            //return await _unitOfWork.User.UpdateUser(userid, userdomain);

        }

        public async Task<IdentityResult> ActivateUser(string userid, bool val)
        {

            var user = await _unitOfWork.User.GetUserById(userid);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            user.IsActive = val;

            return await _unitOfWork.User.UpdateUser(userid, user);
        }

        public Task<AppRole> GetRoleById(string roleId)
        {
            var role = _unitOfWork.User.GetRoleById(roleId);
            //var role = Mapper.Map<RoleDTO>(result);
            return role;
        }

        public Task<AppRole> GetRoleByName(string roleName)
        {
            var role = _unitOfWork.User.GetRoleById(roleName);
            //var role = Mapper.Map<RoleDTO>(result);
            return role;
        }

        public Task<IEnumerable<AppRole>> GetRoles()
        {
            var role = _unitOfWork.User.GetRoles();
            //var role = Mapper.Map<RoleDTO>(result);
            return role;
        }

        public async Task<IdentityResult> AddRole(RoleDTO roleDTO)
        {
            if (await GetRoleByName(roleDTO.Name) != null)
            {
                throw new GenericException("Role exist already!");
            }

            var result = await _unitOfWork.User.AddRole(roleDTO.Name);
            return result;
        }

        public Task<IdentityResult> RemoveRole(string roleId)
        {
            var result = _unitOfWork.User.RemoveRole(roleId);
            return result;
        }

        public Task<IdentityResult> UpdateRole(string roleId, RoleDTO roleDTO)
        {
            var result = _unitOfWork.User.UpdateRole(roleId, roleDTO);
            return result;
        }

        public Task<IdentityResult> AddToRoleAsync(string userId, string name)
        {
            var result = _unitOfWork.User.AddToRoleAsync(userId, name);
            return result;
        }

        public Task<IList<string>> GetUserRoles(string userid)
        {
            var result = _unitOfWork.User.GetUserRoles(userid);
            return result;
        }

        public Task<bool> IsInRoleAsync(string userid, string name)
        {
            var result = _unitOfWork.User.IsInRoleAsync(userid, name);
            return result;
        }

        public Task<IdentityResult> RemoveFromRoleAsync(string userid, string name)
        {
            var result = _unitOfWork.User.RemoveFromRoleAsync(userid, name);
            return result;
        }

        public async Task<IdentityResult> AddClaimAsync(string userid, Claim claim)
        {
            var user = await _unitOfWork.User.GetUserById(userid);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }

            var userClaims = await GetClaimsAsync(userid);

            foreach (var cl in userClaims) 
            {
                if (cl.Type == "Privilege")
                {
                    await RemoveClaimAsync(userid, cl);
                }
            }

            var result = await _unitOfWork.User.AddClaimAsync(userid, claim);
            return result;
        }

        public async Task<IdentityResult> RemoveClaimAsync(string userid, Claim claim)
        {
            var user = await _unitOfWork.User.GetUserById(userid);

            if (user == null)
            {
                throw new GenericException("User does not exist!");
            }
            var result = await _unitOfWork.User.RemoveClaimAsync(userid, claim);
            return result;
        }

        public Task<IList<Claim>> GetClaimsAsync(string userid)
        {
            var result = _unitOfWork.User.GetClaimsAsync(userid);
            return result;
        }

        public Task<string> GetCurrentUserId()
        {
            var userId = HttpContext.Current?.User?.Identity?.GetUserId();
            return Task.FromResult(!string.IsNullOrEmpty(userId) ? userId : "Anonymous");
        }

        public async Task<bool> RoleSettings(string systemuserid, string userid)
        {
            var result = false;

            try
            {
                List<Claim> nonActivityClaim = new List<Claim>();

                // get the users
                var systemUser = await GetUserById(systemuserid);
                if (systemUser.UserType != UserType.System)
                {
                    throw new GenericException("User is not a System Type!");
                }

                // get the roles and claims
                var systemUserRoles = await GetUserRoles(systemuserid);
                var userRoles = await GetUserRoles(userid);

                var systemUserClaims = await GetClaimsAsync(systemuserid);
                var userClaims = await GetClaimsAsync(userid);

                // remove all roles and activity claims from the user
                foreach (var role in userRoles)
                {
                    await RemoveFromRoleAsync(userid, role);
                }

                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Activity")
                    {
                        await RemoveClaimAsync(userid, claim);
                    }
                    else
                    {
                        nonActivityClaim.Add(claim);
                    }
                }

                // assign roles and claim from systemUser
                foreach (var role in systemUserRoles)
                {
                    await AddToRoleAsync(userid, role);
                }

                foreach (var claim in systemUserClaims)
                {
                    if (claim.Type == "Activity")
                    {
                        await AddClaimAsync(userid, claim);
                    }
                }

                foreach (var claim in nonActivityClaim)
                {
                    await AddClaimAsync(userid, claim);
                }

                //update the user with the system user role
                var userDTO = await GetUserById(userid);
                userDTO.SystemUserId = systemuserid;
                userDTO.SystemUserRole = systemUser.FirstName;
                await UpdateUser(userid, userDTO);

                // complete transaction if all actions are successful
                await _unitOfWork.CompleteAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;

                throw;
            }

            return result;
        }

    }
}
