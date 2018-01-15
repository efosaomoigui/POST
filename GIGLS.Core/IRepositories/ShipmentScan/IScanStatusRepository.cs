using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface IScanStatusRepository : IRepository<ScanStatus>
    {
        Task<IEnumerable<ScanStatusDTO>> GetScanStatusAsync();
    }
}
