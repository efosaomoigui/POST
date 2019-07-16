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
using System.Linq;
using GIGLS.Core.IServices.ServiceCentres;

namespace GIGLS.WebApi.Controllers.User
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    //[RoutePrefix("api/user")]
    public class UserController : BaseWebApiController
    {
        private readonly IUserService _userService;
        private readonly IPasswordGenerator _passwordGenerator;
        private IServiceCentreService _serviceCentreService;

        public UserController(IUserService userService, IPasswordGenerator passwordGenerator,
            IServiceCentreService serviceCentreService) : base(nameof(UserController))
        {
            _userService = userService;
            _passwordGenerator = passwordGenerator;
            _serviceCentreService = serviceCentreService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/customer")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetCustomerUsers()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetCorporateCustomerUsers();
                return new ServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>
                {
                    Object = users
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/partner")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetPartnerUsers()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetPartnerUsers();
                return new ServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>
                {
                    Object = users
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/dispatchcaptains")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetDispatchCaptain()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetDispatchCaptains();
                return new ServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>
                {
                    Object = users
                };

            });
        }

        [GIGLSActivityAuthorize(Activity ="View")]
        [HttpGet]
        [Route("api/user/dispatchriders")]
        public async Task<IServiceResponse<IEnumerable<GIGL.GIGLS.Core.Domain.User>>> GetDispatchRiders()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _userService.GetDispatchRiders();
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

               //set country
               var countries = await _userService.GetPriviledgeCountrys();
               user.Country = countries;
               user.CountryName = countries.Select(x => x.CountryName).ToList();

               //set service centre
               int[] serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
               if (serviceCenterIds.Length == 1)
               {
                   var serviceCentre = await _serviceCentreService.GetServiceCentreById(serviceCenterIds[0]);
                   user.UserActiveServiceCentre = serviceCentre.Name;
               }

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
                    Object = roles.OrderBy(x => x).ToList()
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
                var vals = (claim.SystemRole !=null) ? "." + claim.SystemRole : ""; 
                var val = claim.claimValue + vals;
                Claim cl = new Claim(claim.claimType, val);
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
                var val = claim.claimValue+"."+claim.SystemRole;
                Claim cl = new Claim(claim.claimType, val);
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

        //Login afte session expires
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/login2")]
        public async Task<IServiceResponse<JObject>> Login2(string username, string password)
        {
            //trim
            if (username != null)
            {
                username = username.Trim();
            }

            if (password != null)
            {
                password = password.Trim();
            }

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
                         new KeyValuePair<string, string>("Username", username),
                         new KeyValuePair<string, string>("Password", password),
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
                
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            //trim
            if (userLoginModel.username != null)
            {
                userLoginModel.username = userLoginModel.username.Trim();
            }

            if (userLoginModel.Password != null)
            {
                userLoginModel.Password = userLoginModel.Password.Trim();
            }

            string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
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

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Operation could not complete login successfully:");
                    }

                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);                                      

                    getTokenResponse = jObject.GetValue("access_token").ToString();

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
        public async Task<IServiceResponse<bool>> RoleSettings(string userid, string systemuserid)
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("api/user/checkpriviledge")]
        public async Task<IServiceResponse<bool>> CheckPriviledge()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.CheckPriviledge();

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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPut]
        [Route("api/user/resetpassword/{userid}/{password}")]
        public async Task<IServiceResponse<bool>> ResetPassword(string userid, string password)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.ResetPassword(userid, password);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPut]
        [Route("api/user/changepassword/{userid}/{currentPassword}/{newPassword}")]
        public async Task<IServiceResponse<bool>> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.ChangePassword(userid, currentPassword, newPassword);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete update successfully");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [AllowAnonymous]
        [HttpPut]
        [Route("api/user/resetexpiredpassword/{email}/{currentPassword}/{newPassword}")]
        public async Task<IServiceResponse<bool>> ResetExpiredPassword(string email, string currentPassword, string newPassword)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.ResetExpiredPassword(email, currentPassword, newPassword);

                if (!result.Succeeded)
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




