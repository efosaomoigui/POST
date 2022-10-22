using POST.Core.DTO.ShipmentScan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.ShipmentScan
{
    public interface IScanStatusService : IServiceDependencyMarker
    {
        Task<IEnumerable<ScanStatusDTO>> GetScanStatus();
        Task<ScanStatusDTO> GetScanStatusById(int scanStatusId);
        Task<ScanStatusDTO> GetScanStatusByCode(string code);
        Task<object> AddScanStatus(ScanStatusDTO scanStatus);
        Task UpdateScanStatus(int ScanStatusId, ScanStatusDTO scanStatus);
        Task DeleteScanStatus(int ScanStatusId);
        Task<IEnumerable<ScanStatusDTO>> GetNonHiddenScanStatus();
    }
}
