using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;
using System.Linq;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/manifestgroupwaybillnumbermapping")]
    public class ManifestGroupWaybillNumberMappingController : BaseWebApiController
    {
        private readonly IManifestGroupWaybillNumberMappingService _service;
        public ManifestGroupWaybillNumberMappingController(IManifestGroupWaybillNumberMappingService service) : base(nameof(ManifestGroupWaybillNumberMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ManifestGroupWaybillNumberMappingDTO>>> GetAllManifestGroupWayBillNumberMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetAllManifestGroupWayBillNumberMappings();

                return new ServiceResponse<IEnumerable<ManifestGroupWaybillNumberMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings.ToList()
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingManifestToGroupWaybillNumber(ManifestGroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToGroupWaybillNumber(data.ManifestCode, data.GroupWaybillNumbers);
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getmanifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<ManifestGroupWaybillNumberMappingDTO>> GetManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestforWaybill = await _service.GetManifestForWaybill(waybill);

                return new ServiceResponse<ManifestGroupWaybillNumberMappingDTO>
                {
                    Object = manifestforWaybill
                };
            });
        }
    }
}
