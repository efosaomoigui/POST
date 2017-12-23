using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/manifest")]
    public class ManifestController : BaseWebApiController
    {
        private readonly IManifestService _service;

        public ManifestController(IManifestService service) : base(nameof(ManifestController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ManifestDTO>>> GetAllManifests()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifests();

                return new ServiceResponse<IEnumerable<ManifestDTO>>
                {
                    Object = manifests
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{manifestId:int}")]
        public async Task<IServiceResponse<ManifestDTO>> GetManifestById(int manifestId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifestById(manifestId);

                return new ServiceResponse<ManifestDTO>
                {
                    Object = manifests
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{manifest}")]
        public async Task<IServiceResponse<ManifestDTO>> GetManifestByCode(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifestByCode(manifest);

                return new ServiceResponse<ManifestDTO>
                {
                    Object = manifests
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddManifest(ManifestDTO manifestDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifest = await _service.AddManifest(manifestDTO);
                return new ServiceResponse<object>
                {
                    Object = manifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{manifestId:int}")]
        public async Task<IServiceResponse<bool>> DeleteManifestById(int manifestId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                await _service.DeleteManifest(manifestId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }
        
        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{manifestId:int}")]
        public async Task<IServiceResponse<bool>> UpdateManifestById(int manifestId, ManifestDTO manifestDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateManifest(manifestId, manifestDTO);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("generateMaifestCode")]
        public async Task<IServiceResponse<string>> GenerateManifestCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                ManifestDTO manifestDTO = new ManifestDTO();
                var groupwaybills = await _service.GenerateManifestCode(manifestDTO);

                return new ServiceResponse<string>
                {
                    Object = groupwaybills
                };
            });
        }


    }
}
