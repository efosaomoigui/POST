using System;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices;
using System.Net.Http;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using GIGLS.WebApi.Models;
//using Audit.WebApi;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Owin;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace GIGLS.WebApi.Controllers


{

    //[AuditApi]
    public class BaseWebApiController : ApiController
    {
        private readonly string _controllerName;

        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        //private string _databaseForeignKeyErrorMessage;

        //protected string CurrentUserId => $"[{User.Identity.GetUserId() ?? "0"}]({(!string.IsNullOrEmpty(User.Identity.Name) ? User.Identity.Name : "Anonymous")})";

        //protected ILog Logger;

        protected BaseWebApiController(string controllerName)
        {
            _controllerName = controllerName;
            //Logger = LogManager.GetLogger(controllerName);
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected async Task<IServiceResponse<T>> HandleApiOperationAsync<T>(
            Func<Task<ServiceResponse<T>>> action, [CallerLineNumber] int lineNo = 0,
            [CallerMemberName] string method = "")
        {
            var apiResponse = new ServiceResponse<T>
            {
                Code = $"{(int)HttpStatusCode.OK}",
                ShortDescription = "Operation was successful!"
            };

            //var logger = LogManager.GetLogger($"{_controllerName} / {method} - {CurrentUserId}");

            //logger.Info($">>>=============== ENTERS ({method}) ===============>>>");

            try
            {
                if (!ModelState.IsValid)
                    throw new GenericException("There were errors in your input, please correct them and try again.", $"{(int)HttpStatusCode.BadRequest}");

                var methodResponse = await action.Invoke();

                apiResponse.Object = methodResponse.Object;
                apiResponse.Total = methodResponse.Total;
                apiResponse.RefCode = methodResponse.RefCode;
                apiResponse.Shipmentcodref = methodResponse.Shipmentcodref;
                apiResponse.ShortDescription = string.IsNullOrEmpty(methodResponse.ShortDescription)
                    ? apiResponse.ShortDescription
                    : methodResponse.ShortDescription;

            }
            catch (GenericException giglsex) //Error involving form field values
            {
                //logger.Warn($"L{lineNo} - {gmgex.ErrorCode}: {gmgex.Message}");

                apiResponse.ShortDescription = giglsex.Message;
                apiResponse.Code = giglsex.ErrorCode;

                if (!ModelState.IsValid)
                {
                    apiResponse.ValidationErrors = ModelState.ToDictionary(
                        m =>
                        {
                            var tokens = m.Key.Split('.');
                            return tokens.Length > 0 ? tokens[tokens.Length - 1] : tokens[0];
                        },
                        m => m.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage)
                    );
                }
                else
                {
                    apiResponse.Code = $"{(int)HttpStatusCode.InternalServerError}";
                    List<string> errorList = new List<string>();
                    errorList.Add(giglsex.Message);
                    if(giglsex.InnerException != null ) errorList.Add(giglsex.InnerException?.Message);
                    if(giglsex.InnerException?.InnerException != null) errorList.Add(giglsex.InnerException?.InnerException?.Message);
                    if(giglsex.InnerException?.InnerException?.InnerException != null) errorList.Add(giglsex.InnerException?.InnerException?.InnerException?.Message);
                    apiResponse.ValidationErrors.Add("Error", errorList);
                }
            }
            catch (Exception ex)
            {
                //logger.Error($"L{lineNo} {ex}");
                apiResponse.ShortDescription = $"Sorry, we are unable process your request. Please try again or contact support for assistance.";
                apiResponse.Code = $"{(int)HttpStatusCode.InternalServerError}";

                List<string> errorList = new List<string>();
                errorList.Add(ex.Message);
                if(ex.InnerException != null) errorList.Add(ex.InnerException?.Message);
                if(ex.InnerException?.InnerException != null) errorList.Add(ex.InnerException?.InnerException?.Message);
                if(ex.InnerException?.InnerException?.InnerException != null) errorList.Add(ex.InnerException?.InnerException?.InnerException?.Message);
                apiResponse.ValidationErrors.Add("Error", errorList);
            }

            //logger.Info($"<<<=============== EXITS ({method}) ===============<<< ");

            return apiResponse;
        }

        protected async Task<IServiceResponse<T>> HandleApiOperationAsync2<T>(
            Func<ServiceResponse<T>> action, [CallerLineNumber] int lineNo = 0,
            [CallerMemberName] string method = "")
        {
            var apiResponse = new ServiceResponse<T>
            {
                Code = $"{(int)HttpStatusCode.OK}",
                ShortDescription = "Operation was successful!"
            };

            //var logger = LogManager.GetLogger($"{_controllerName} / {method} - {CurrentUserId}");

            //logger.Info($">>>=============== ENTERS ({method}) ===============>>>");

            try
            {
                if (!ModelState.IsValid)
                    throw new GenericException("There were errors in your input, please correct them and try again.", $"{(int)HttpStatusCode.BadRequest}");

                var methodResponse = action.Invoke();

                apiResponse.Object = methodResponse.Object;

                //Checking if we set description from controller else set with apiResponse ShortDescription
                apiResponse.ShortDescription = string.IsNullOrEmpty(methodResponse.ShortDescription)
                    ? apiResponse.ShortDescription
                    : methodResponse.ShortDescription;

            }
            catch (GenericException giglsex)
            {
                //logger.Warn($"L{lineNo} - {gmgex.ErrorCode}: {gmgex.Message}");
                apiResponse.ShortDescription = giglsex.Message;
                apiResponse.Code = giglsex.ErrorCode;

                if (!ModelState.IsValid)
                {
                    apiResponse.ValidationErrors = ModelState.ToDictionary(
                        m =>
                        {
                            var tokens = m.Key.Split('.');
                            return tokens.Length > 0 ? tokens[tokens.Length - 1] : tokens[0];
                        },
                        m => m.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage)
                    );
                }
            }
            catch (Exception ex)
            {
                //logger.Error($"L{lineNo} {ex}");
                apiResponse.ShortDescription = "Sorry, we are unable process your request. Please try again or contact support for assistance.(" + ex.Message + ")";
                apiResponse.Code = $"{(int)HttpStatusCode.InternalServerError}";
            }

            //logger.Info($"<<<=============== EXITS ({method}) ===============<<< ");

            return apiResponse;
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }


}
