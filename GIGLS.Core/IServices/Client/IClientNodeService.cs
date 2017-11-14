using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Client
{
    public interface IClientNodeService : IServiceDependencyMarker
    {
        Task<IEnumerable<ClientNodeDTO>> GetClientNodes();
        Task<ClientNodeDTO> GetClientNodeById(int clientNodeId);
        Task<object> AddClientNode(ClientNodeDTO clientNode);
        Task UpdateClientNode(int clientNodeId, ClientNodeDTO clientNode);
        Task RemoveClientNode(int clientNodeId);
    }
}
