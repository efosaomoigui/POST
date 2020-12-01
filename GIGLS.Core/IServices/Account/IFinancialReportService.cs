using GIGLS.Core.DTO.Account;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IFinancialReportService : IServiceDependencyMarker
    {
        Task<object> AddReport(FinancialReportDTO financialReportDTO);
    }
}
