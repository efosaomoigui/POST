using GIGLS.Core.IServices;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IServices.User;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Infrastructure;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using EfeAuthen.Models;
using System;
using System.Configuration;
using GIGLS.CORE.Domain;
using System.Security.Claims;
using GIGLS.WebApi.Filters;
using GIGLS.CORE.DTO.User;
using GIGLS.Core.IServices.Utility;

namespace GIGLS.WebApi.Controllers.User
{
    //[Authorize]
    //[RoutePrefix("api/user")]
    public class UserController : BaseWebApiController
    {
        private readonly IUserService _userService;
        private readonly IPasswordGenerator _passwordGenerator;

        public UserController(IUserService userService, IPasswordGenerator passwordGenerator) : base(nameof(UserController))
        {
            _userService = userService;
            _passwordGenerator = passwordGenerator;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetUsers()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetUsers();
                return new ServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>
                {
                    Object = users
                };

            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/getsystemusers")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetSystemUsers()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetSystemUsers();
                return new ServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>
                {
                    Object = users
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("api/user")]
        public async Task<IServiceResponse<object>> AddUser(UserDTO userdto)
        {
            //Generate Password
            string password = await _passwordGenerator.Generate();
            userdto.Password = password;

            return await HandleApiOperationAsync(async () =>
            {

                var result = await _userService.AddUser(userdto);
                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete successfully: ", $"{GetErrorResult(result)}");
                }

                var user = await _userService.GetUserByEmail(userdto.Email);
                user.Password = password;

                return new ServiceResponse<object>
                {
                    Object = user
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/{userId}")]
        public async Task<IServiceResponse<UserDTO>> GetUser(string userId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var user = await _userService.GetUserById(userId);
               return new ServiceResponse<UserDTO>
               {
                   Object = user
               };
           });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("api/user/{userId}")]
        public async Task<IServiceResponse<bool>> Deleteuser(string userId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var result = await _userService.RemoveUser(userId);
               if (!result.Succeeded)
               {
                   throw new GenericException("Operation could not complete delete successfully: ", $"{GetErrorResult(result)}");
               }


               return new ServiceResponse<bool>
               {
                   Object = true
               };

           });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("api/user/{userId}")]
        public async Task<IServiceResponse<bool>> UpdateUser(string userId, UserDTO userdto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.UpdateUser(userId, userdto);
                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("api/user/activate/{userId}")]
        public async Task<IServiceResponse<bool>> ActivateUser(string userId, bool val)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.ActivateUser(userId, val);
                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/getuserrole/{userid}")]
        public async Task<IServiceResponse<IList<string>>> GetAllRoles(string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var roles = await _userService.GetUserRoles(userId);

                return new ServiceResponse<IList<string>>
                {
                    Object = roles
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/getrolebyid/{Id}")]
        public async Task<IServiceResponse<AppRole>> GetRole(string Id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var role = await _userService.GetRoleById(Id);
                return new ServiceResponse<AppRole>
                {
                    Object = role
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/getallroles")]
        public async Task<IServiceResponse<IEnumerable<AppRole>>> GetAllRoles()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var roles = await _userService.GetRoles();

                return new ServiceResponse<IEnumerable<AppRole>>
                {
                    Object = roles
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [Route("api/user/createrole")]
        public async Task<IServiceResponse<object>> CreateRole(RoleDTO roleDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.AddRole(roleDTO);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete successfully: ", $"{GetErrorResult(result)}");
                }

                var role = await _userService.GetRoleByName(roleDTO.Name);
                return new ServiceResponse<object>
                {
                    Object = role
                };

            });

        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("api/user/updaterole/{Id}")]
        public async Task<IServiceResponse<object>> UpdateRole(string Id, RoleDTO roleDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.UpdateRole(Id, roleDTO);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete successfully: ", $"{GetErrorResult(result)}");
                }

                var role = await _userService.GetRoleByName(roleDTO.Name);
                return new ServiceResponse<object>
                {
                    Object = role
                };

            });

        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("api/user/deleterole/{roleId}")]
        public async Task<IServiceResponse<bool>> DeleteRole(string roleId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.RemoveRole(roleId);
                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("api/user/addusertorole/{userid}")]
        public async Task<IServiceResponse<bool>> addusertorole(string userid, RoleDTO role)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var checkresult = await _userService.IsInRoleAsync(userid, role.Name);

                if (checkresult)
                {
                    throw new GenericException("User Exist in Role Already!");
                }

                var result = await _userService.AddToRoleAsync(userid, role.Name);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("api/user/removeuserfromrole/{userid}")]
        public async Task<IServiceResponse<bool>> removeuserfromrole(string userid, RoleDTO role)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var checkresult = await _userService.IsInRoleAsync(userid, role.Name);

                if (checkresult.Equals(false))
                {
                    throw new GenericException("User Does not Exist in Role!");
                }

                var result = await _userService.RemoveFromRoleAsync(userid, role.Name);
                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("api/user/addclaimforuser")]
        public async Task<IServiceResponse<bool>> addclaimforuser(AddClaimDto claim)
        {
            return await HandleApiOperationAsync(async () =>
            {
                Claim cl = new Claim(claim.claimType, claim.claimValue);
                var result = await _userService.AddClaimAsync(claim.userId, cl);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("api/user/removeclaimforuser")]
        public async Task<IServiceResponse<bool>> removeclaimforuser(AddClaimDto claim)
        {
            return await HandleApiOperationAsync(async () =>
            {
                Claim cl = new Claim(claim.claimType, claim.claimValue);
                var result = await _userService.RemoveClaimAsync(claim.userId, cl);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully: ", $"{GetErrorResult(result)}");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/getclaims/{userid}")]
        public async Task<IServiceResponse<IList<Claim>>> getclaims(string userid)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetClaimsAsync(userid);

                if (result == null)
                {
                    throw new GenericException("Operation could not complete update successfully: ");
                }

                return new ServiceResponse<IList<Claim>>
                {
                    Object = result
                };
            });

        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {

            string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
            //const string apiBaseUri = "http://giglsresourceapi.azurewebsites.net/api/";
            string getTokenResponse;

            return await HandleApiOperationAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //setup login data
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("grant_type", "password"),
                         new KeyValuePair<string, string>("Username", userLoginModel.username),
                         new KeyValuePair<string, string>("Password", userLoginModel.Password),
                     });

                    //setup login data
                    HttpResponseMessage responseMessage = client.PostAsync("token", formContent).Result;

                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Operation could not complete login successfully:");
                    }

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
        [Route("api/user/rolesettings/{userid}/{systemuserid}")]
        public async Task<IServiceResponse<bool>> RoleSettings(string userid, string systemuserid )
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.RoleSettings(systemuserid, userid);

                if (!result)
                {
                    throw new GenericException("Operation could not complete update successfully");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }


    }

}




