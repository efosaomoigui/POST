using GIGL.POST.Core.Repositories;
using POST.Core.Domain.ShipmentScan;
using POST.Core.DTO.ShipmentScan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.ShipmentScan
{
    public interface IScanStatusRepository : IRepository<ScanStatus>
    {
        Task<IEnumerable<ScanStatusDTO>> GetScanStatusAsync();
    }
}
