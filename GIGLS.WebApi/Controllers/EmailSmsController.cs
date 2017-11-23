using GIGLS.Core.IServices;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers
{
    //[Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/emailSms")]
    public class EmailSmsController : BaseWebApiController
    {
        private readonly IEmailSmsService _emailSmsService;

        public EmailSmsController(IEmailSmsService emailSmsService):base(nameof(EmailSmsController))
        {
            _emailSmsService = emailSmsService;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("email")]
        public async Task<IServiceResponse<IEnumerable<EmailSmsDTO>>> GetEmails()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var emails = await _emailSmsService.GetEmailAsync();

                return new ServiceResponse<IEnumerable<EmailSmsDTO>>
                {
                    Object = emails
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("sms")]
        public async Task<IServiceResponse<IEnumerable<EmailSmsDTO>>> GetSms()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var sms = await _emailSmsService.GetSmsAsync();

                return new ServiceResponse<IEnumerable<EmailSmsDTO>>
                {
                    Object = sms
                };
            });
        }


        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddEmailSms(EmailSmsDTO emailSmsDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var emailSms = await _emailSmsService.AddEmailSms(emailSmsDTO);

                return new ServiceResponse<object>
                {
                    Object = emailSms
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{emailSmsId:int}")]
        public async Task<IServiceResponse<EmailSmsDTO>> GetEmailSms(int emailSmsId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var emailSms = await _emailSmsService.GetEmailSmsById(emailSmsId);

                return new ServiceResponse<EmailSmsDTO>
                {
                    Object = emailSms
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{emailSmsId:int}")]
        public async Task<IServiceResponse<bool>> DeleteEmailSms(int emailSmsId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _emailSmsService.RemoveEmailSms(emailSmsId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{emailSmsId:int}")]
        public async Task<IServiceResponse<bool>> UpdateEmailSms(int emailSmsId, EmailSmsDTO emailSmsDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _emailSmsService.UpdateEmailSms(emailSmsId, emailSmsDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
