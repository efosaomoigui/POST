using GIGLS.CORE.IServices.Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using AutoMapper;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Services.Implementation.Report
{
    public class AccountReportService : IAccountReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public AccountReportService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
        }
        public async Task<List<GeneralLedgerDTO>> GetExpenditureReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Debit;
            return await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria, serviceCenterIds);
        }

        public async Task<List<GeneralLedgerDTO>> GetIncomeReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Credit;
            return await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria, serviceCenterIds);
        }

        public async Task<List<InvoiceDTO>> GetInvoiceReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var invoices = await _uow.Invoice.GetInvoicesAsync(accountFilterCriteria, serviceCenterIds);

            //get shipment info
            foreach(var item in invoices)
            {
                var shipment = _uow.Shipment.SingleOrDefault(s => s.Waybill == item.Waybill);
                var shipmentDTO = Mapper.Map<ShipmentDTO>(shipment);
                item.Shipment = shipmentDTO;
            }

            return invoices;
        }
    }
}
