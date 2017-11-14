using GIGLS.Core;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGeneralLedgerService _generalLedgerService;

        public ExpenditureService(IUnitOfWork uow, IGeneralLedgerService generalLedgerService)
        {
            _uow = uow;
            _generalLedgerService = generalLedgerService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<GeneralLedgerDTO>> GetExpenditures()
        {
            var expenditures = await _generalLedgerService.GetGeneralLedgersAsync(CreditDebitType.Debit);
            return expenditures;
        }

    }
}
