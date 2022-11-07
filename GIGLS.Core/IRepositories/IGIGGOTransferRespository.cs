using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IGIGGOTransferRespository : IServiceDependencyMarker
    {
        Task<object> AddCODTransfer(GIGGOCODTransferDTO payload);
    }
}
