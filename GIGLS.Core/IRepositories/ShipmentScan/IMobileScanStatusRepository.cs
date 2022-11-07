using GIGL.POST.Core.Repositories;
using POST.Core.Domain.ShipmentScan;
using POST.Core.DTO.ShipmentScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.ShipmentScan
{
    public interface IMobileScanStatusRepository : IRepository<MobileScanStatus>
    {
        Task<IEnumerable<MobileScanStatusDTO>> GetMobileScanStatusAsync();
    }
}
