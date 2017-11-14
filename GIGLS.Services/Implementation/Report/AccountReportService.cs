using GIGLS.CORE.IServices.Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core;

namespace GIGLS.Services.Implementation.Report
{
    public class AccountReportService : IAccountReportService
    {
        private readonly IUnitOfWork _uow;

        public AccountReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<GeneralLedgerDTO>> GetExpenditureReports(AccountFilterCriteria accountFilterCriteria)
        {
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Debit;
            return await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria);
        }

        public async Task<List<GeneralLedgerDTO>> GetIncomeReports(AccountFilterCriteria accountFilterCriteria)
        {
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Credit;
            return await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria);
        }

        public async Task<List<InvoiceDTO>> GetInvoiceReports(AccountFilterCriteria accountFilterCriteria)
        {
            return await _uow.Invoice.GetInvoicesAsync(accountFilterCriteria);
        }
    }
}
