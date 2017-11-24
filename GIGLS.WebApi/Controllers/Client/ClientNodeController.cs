using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Client;
using GIGLS.Core.IServices.Client;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Client
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/clientNode")]
    public class ClientNodeController : BaseWebApiController
    {
        private readonly IClientNodeService _clientNodeService;
        public ClientNodeController(IClientNodeService clientNodeService) : base(nameof(ClientNodeController))
        {
            _clientNodeService = clientNodeService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ClientNodeDTO>>> GetClientNodes()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var clientNode = await _clientNodeService.GetClientNodes();

                return new ServiceResponse<IEnumerable<ClientNodeDTO>>
                {
                    Object = clientNode
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddClientNode(ClientNodeDTO clientNodeDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var clientNode = await _clientNodeService.AddClientNode(clientNodeDto);

                return new ServiceResponse<object>
                {
                    Object = clientNode
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{clientNodeId:int}")]
        public async Task<IServiceResponse<ClientNodeDTO>> GetClientNode(int clientNodeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var clientNode = await _clientNodeService.GetClientNodeById(clientNodeId);

                return new ServiceResponse<ClientNodeDTO>
                {
                    Object = clientNode
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{clientNodeId:int}")]
        public async Task<IServiceResponse<bool>> DeleteClientNode(int clientNodeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _clientNodeService.RemoveClientNode(clientNodeId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{clientNodeId:int}")]
        public async Task<IServiceResponse<bool>> UpdateClientNode(int clientNodeId, ClientNodeDTO clientNodeDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _clientNodeService.UpdateClientNode(clientNodeId, clientNodeDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
