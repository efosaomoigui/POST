using GIGLS.Core;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class IncomeService : IIncomeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGeneralLedgerService _generalLedgerService;

        public IncomeService(IUnitOfWork uow, IGeneralLedgerService generalLedgerService)
        {
            _uow = uow;
            _generalLedgerService = generalLedgerService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<GeneralLedgerDTO>> GetAllIncome()
        {
            var incomes = await _generalLedgerService.GetGeneralLedgersAsync(CreditDebitType.Credit);
            return incomes;
        }
    }
}
