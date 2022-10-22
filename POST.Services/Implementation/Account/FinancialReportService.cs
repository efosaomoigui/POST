using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IServices.Account;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Account
{
    public class FinancialReportService : IFinancialReportService
    {
        private readonly IUnitOfWork _uow;

        public FinancialReportService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddReport(FinancialReportDTO financialReportDTO)
        {
            var financialReport = Mapper.Map<FinancialReport>(financialReportDTO);

            _uow.FinancialReport.Add(financialReport);
            await _uow.CompleteAsync();
            return new { id = financialReport.FinancialReportId };
        }
    }
}
