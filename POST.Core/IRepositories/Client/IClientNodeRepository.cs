using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Client
{
    public interface IClientNodeRepository : IRepository<ClientNode>
    {
        Task<List<ClientNodeDTO>> GetClientNodesAsync();
    }
}
