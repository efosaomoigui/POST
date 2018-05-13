using GIGLS.Core.IServices.CustomerPortal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using System.Linq;
using AutoMapper;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.CashOnDeliveryAccount;

namespace GIGLS.Services.Business.CustomerPortal
{
    public class CustomerPortalService : ICustomerPortalService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IInvoiceService _invoiceService;
        private readonly IShipmentTrackService _iShipmentTrackService;
        private readonly IUserService _userService;
        private readonly IWalletTransactionService _iWalletTransactionService;
        private readonly ICashOnDeliveryAccountService _iCashOnDeliveryAccountService;

        public CustomerPortalService(IUnitOfWork uow, IShipmentService shipmentService, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService, IUserService userService,
            IWalletTransactionService iWalletTransactionService, ICashOnDeliveryAccountService iCashOnDeliveryAccountService)
        {
            _shipmentService = shipmentService;
            _invoiceService = invoiceService;
            _iShipmentTrackService = iShipmentTrackService;
            _userService = userService;
            _iWalletTransactionService = iWalletTransactionService;
            _iCashOnDeliveryAccountService = iCashOnDeliveryAccountService;
            _uow = uow;
            MapperConfig.Initialize();
        }


        public async Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            var invoices = _uow.Invoice.GetAllFromInvoiceView();

            //filter by CustomerId
            if (f_Criteria.CustomerId > 0)
            {
                invoices = invoices.Where(s => s.CustomerId == f_Criteria.CustomerId);
            }

            //filter by CustomerType
            if (f_Criteria.FilterCustomerType != null)
            {
                invoices = invoices.Where(s => s.CustomerType == f_Criteria.FilterCustomerType.ToString());
            }

            //filter by UserId
            if (f_Criteria.UserId != null)
            {
                invoices = invoices.Where(s => s.UserId == f_Criteria.UserId);
            }

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);
            return await Task.FromResult(invoicesDto);
        }

        public async Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            var walletTransactionSummary = await _iWalletTransactionService.GetWalletTransactionByWalletId(wallet.WalletId);
            return walletTransactionSummary;
        }

        public async Task<InvoiceDTO> GetInvoiceByWaybill(string waybill)
        {
            var invoice = await _invoiceService.GetInvoiceByWaybill(waybill);
            return invoice;
        }

        public async Task<IEnumerable<InvoiceViewDTO>> GetInvoices()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            //get the customer type
            var customerType = "";
            if (wallet.CustomerType == Core.Enums.CustomerType.Company)
            {
                customerType = "Company";
            }
            else
            {
                customerType = "Individual";
            }

            var invoices = _uow.Invoice.GetAllFromInvoiceView().Where(s => s.CustomerId == wallet.CustomerId &&
            s.CustomerType == customerType).ToList();

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);
            return invoicesDto;
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            var result = await _iShipmentTrackService.TrackShipment(waybillNumber);
            return result;
        }

        public async Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            var result = await _iCashOnDeliveryAccountService.GetCashOnDeliveryAccountByWallet(wallet.WalletNumber);
            return result;
        }
    }
}
