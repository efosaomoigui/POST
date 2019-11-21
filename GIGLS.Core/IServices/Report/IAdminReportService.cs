using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Report;
using GIGLS.CORE.DTO.Report;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Report
{
    public interface IAdminReportService : IServiceDependencyMarker
    {
        Task<AdminReportDTO> GetAdminReport(ShipmentCollectionFilterCriteria filterCriteria);
        Task<AdminReportDTO> DisplayWebsiteData();
    }
}