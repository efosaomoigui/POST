using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
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
