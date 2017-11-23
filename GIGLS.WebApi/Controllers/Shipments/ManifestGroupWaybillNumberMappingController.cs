using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/manifestgroupwaybillnumbermapping")]
    public class ManifestGroupWaybillNumberMappingController : BaseWebApiController
    {
        private readonly IManifestGroupWaybillNumberMappingService _service;
        public ManifestGroupWaybillNumberMappingController(IManifestGroupWaybillNumberMappingService service) : base(nameof(ManifestGroupWaybillNumberMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("id")]
        public async Task<IServiceResponse<bool>> MappingManifestToGroupWaybillNumber(int manifestId, int groupWaybillNumberId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToGroupWaybillNumber(manifestId, groupWaybillNumberId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("code")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(string manifest, string groupWaybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToGroupWaybillNumber(manifest, groupWaybillNumber);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestforgroupwaybillnumber/id/{groupwaybillNumberId}")]
        public async Task<IServiceResponse<ManifestDTO>> GetManifestForGroupWaybillNumber(int groupwaybillNumberId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var manifestDTO = await _service.GetManifestForGroupWaybillNumber(groupwaybillNumberId);

                return new ServiceResponse<ManifestDTO>
                {
                    Object = manifestDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestforgroupwaybillnumber/code/{groupwaybillNumber}")]
        public async Task<IServiceResponse<ManifestDTO>> GetManifestForGroupWaybillNumber(string groupwaybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var manifestDTO = await _service.GetManifestForGroupWaybillNumber(groupwaybillNumber);

                return new ServiceResponse<ManifestDTO>
                {
                    Object = manifestDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("groupwaybillnumbersinmanifest/id/{manifestId}")]
        public async Task<IServiceResponse<List<GroupWaybillNumberDTO>>> GetGroupWaybillNumbersInManifest(int manifestId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetGroupWaybillNumbersInManifest(manifestId);

                return new ServiceResponse<List<GroupWaybillNumberDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("groupwaybillnumbersinmanifest/code/{manifest}")]
        public async Task<IServiceResponse<List<GroupWaybillNumberDTO>>> GetGroupWaybillNumbersInManifest(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetGroupWaybillNumbersInManifest(manifest);

                return new ServiceResponse<List<GroupWaybillNumberDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("RemoveGroupWaybillNumberFromManifest/{manifest}/{groupWaybillNumber}")]
        public async Task<IServiceResponse<bool>> RemoveGroupWaybillNumberFromManifest(string manifest, string groupWaybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveGroupWaybillNumberFromManifest(manifest, groupWaybillNumber);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
