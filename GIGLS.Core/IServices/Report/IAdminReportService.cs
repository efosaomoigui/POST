using GIGLS.Core.DTO.Admin;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Report
{
    public interface IAdminReportService
    {
        Task<AdminReportDTO> GetAdminReport();
    }
}