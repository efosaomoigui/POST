using GIGLS.Core;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Account;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Expenses;
using GIGLS.Core.DTO.Report;
using System;
using GIGLS.Core.IServices.User;
using AutoMapper;
using GIGLS.Core.Domain.Expenses;

namespace GIGLS.Services.Implementation.Account
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IGeneralLedgerService _generalLedgerService;

        public ExpenditureService(IUnitOfWork uow,IUserService userService, IGeneralLedgerService generalLedgerService)
        {
            _uow = uow;
            _userService = userService;
            _generalLedgerService = generalLedgerService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<GeneralLedgerDTO>> GetExpenditures()
        {
            var expenditures = await _generalLedgerService.GetGeneralLedgersAsync(CreditDebitType.Debit);
            return expenditures;
        }

        public Task<object> AddExpenditure(GeneralLedgerDTO generalLedger)
        {
            generalLedger.CreditDebitType = CreditDebitType.Debit;
            generalLedger.PaymentServiceType = PaymentServiceType.Miscellaneous;
            return _generalLedgerService.AddGeneralLedger(generalLedger);
        }

        public async Task<object> AddExpenditure(ExpenditureDTO expenditureDto)
        {
            var expenditure = Mapper.Map<Expenditure>(expenditureDto);

            if(expenditureDto.UserId == null)
            {
                expenditure.UserId = await _userService.GetCurrentUserId();
            }

            if(expenditure.ServiceCentreId < 1)
            {
                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                expenditure.ServiceCentreId = serviceCenterIds[0];
            }

            _uow.Expenditure.Add(expenditure);
            await _uow.CompleteAsync();
            return new { id = expenditure.ExpenditureId };
        }

        public async Task<IEnumerable<ExpenditureDTO>> GetExpenditures(ExpenditureFilterCriteria expenditureFilterCriteria)
        {
            if (!expenditureFilterCriteria.StartDate.HasValue)
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                expenditureFilterCriteria.StartDate = startDate;
            }

            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var expenditures = await _uow.Expenditure.GetExpenditures(expenditureFilterCriteria, serviceCenterIds);
            return expenditures;
        }
    }
}
