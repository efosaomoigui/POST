using POST.Core.Domain;
using POST.Core.DTO.Client;
using POST.Core.IRepositories.Client;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Client
{
    public class ClientNodeRepository : Repository<ClientNode, GIGLSContext>, IClientNodeRepository
    {
        public ClientNodeRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<List<ClientNodeDTO>> GetClientNodesAsync()
        {
            var clientNodes = Context.ClientNode;
            var clientNodeDto = from clientNode in clientNodes
                                select new ClientNodeDTO
                                {
                                    ClientNodeId = clientNode.ClientNodeId,
                                    Name = clientNode.Name,
                                    Base64Secret = clientNode.Base64Secret,
                                    Status = clientNode.Status
                                };
            return Task.FromResult(clientNodeDto.ToList());
        }
    }
}
