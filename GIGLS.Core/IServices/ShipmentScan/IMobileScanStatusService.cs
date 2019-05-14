using GIGLS.Core.DTO.ShipmentScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ShipmentScan
{
    public interface IMobileScanStatusService : IServiceDependencyMarker
    {
        Task<IEnumerable<MobileScanStatusDTO>> GetScanStatus();
        Task<MobileScanStatusDTO> GetScanStatusById(int MobileScanStatusId);
        Task<MobileScanStatusDTO> GetScanStatusByCode(string code);
        Task<object> AddScanStatus(MobileScanStatusDTO scanStatus);
        Task UpdateScanStatus(int MobileScanStatusId, MobileScanStatusDTO scanStatus);
        Task DeleteScanStatus(int MobileScanStatusId);
    }
}
