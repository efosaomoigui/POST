using POST.Core.DTO.Admin;
using POST.Core.DTO.Report;
using POST.CORE.DTO.Report;
using System.Threading.Tasks;

namespace POST.Core.IServices.Report
{
    public interface IAdminReportService : IServiceDependencyMarker
    {
        Task<AdminReportDTO> GetAdminReport(ShipmentCollectionFilterCriteria filterCriteria);
        Task<AdminReportDTO> DisplayWebsiteData();
    }
}