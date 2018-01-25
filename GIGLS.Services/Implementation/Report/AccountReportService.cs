using GIGLS.CORE.IServices.Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using AutoMapper;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;

namespace GIGLS.Services.Implementation.Report
{
    public class AccountReportService : IAccountReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IShipmentService _shipmentService;
        

        public AccountReportService(IUnitOfWork uow, IUserService userService, IShipmentService shipmentService)
        {
            _uow = uow;
            _userService = userService;
            _shipmentService = shipmentService;
        }
        public async Task<List<GeneralLedgerDTO>> GetExpenditureReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Debit;
            var generalLedgerDTO = await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria, serviceCenterIds);

            foreach (var item in generalLedgerDTO)
            {
                var user = await _uow.User.GetUserById(item.UserId);
                item.UserId = user.FirstName + " " + user.LastName;
            }
            return generalLedgerDTO;
        }

        public async Task<List<GeneralLedgerDTO>> GetIncomeReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            accountFilterCriteria.creditDebitType = Core.Enums.CreditDebitType.Credit;
            var generalLedgerDTO = await _uow.GeneralLedger.GetGeneralLedgersAsync(accountFilterCriteria, serviceCenterIds);
             
            foreach (var item in generalLedgerDTO)
            {
                var user = await _uow.User.GetUserById(item.UserId);
                item.UserId = user.FirstName + " " + user.LastName;
            }
            return generalLedgerDTO;
        }

        public async Task<List<InvoiceDTO>> GetInvoiceReports(AccountFilterCriteria accountFilterCriteria)
        {
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var invoices = await _uow.Invoice.GetInvoicesAsync(accountFilterCriteria, serviceCenterIds);

            //get shipment info
            foreach(var item in invoices)
            {
                var shipmentDTO = await _shipmentService.GetShipment(item.Waybill);
                var user = await _uow.User.GetUserById(shipmentDTO.UserId);
                item.Shipment = shipmentDTO;
                item.Shipment.UserId = user.FirstName + " " + user.LastName;
            }
            return invoices;
        }
    }
}
