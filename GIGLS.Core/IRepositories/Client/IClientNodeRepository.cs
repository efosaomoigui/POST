using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Client
{
    public interface IClientNodeRepository : IRepository<ClientNode>
    {
        Task<List<ClientNodeDTO>> GetClientNodesAsync();
    }
}
