using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Account
{
    public interface IFinancialReportRepository : IRepository<FinancialReport>
    {
        Task<List<FinancialReportDTO>> GetFinancialReportBreakdown(AccountFilterCriteria accountFilterCriteria);
    }
}
