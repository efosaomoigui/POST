using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ShipmentScan
{
    public interface IMobileScanStatusRepository : IRepository<MobileScanStatus>
    {
        Task<IEnumerable<MobileScanStatusDTO>> GetMobileScanStatusAsync();
    }
}
