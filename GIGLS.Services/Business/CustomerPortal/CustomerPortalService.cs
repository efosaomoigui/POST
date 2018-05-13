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

namespace GIGLS.Services.Business.CustomerPortal
{
    public class CustomerPortalService : ICustomerPortalService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IInvoiceService _invoiceService;
        private readonly IShipmentTrackService _iShipmentTrackService;

        public CustomerPortalService(IUnitOfWork uow, IShipmentService shipmentService, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService)
        {
            _shipmentService = shipmentService;
            _invoiceService = invoiceService;
            _iShipmentTrackService = iShipmentTrackService;
            _uow = uow;
            MapperConfig.Initialize();
        }


        public async Task<List<ShipmentDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            var shipments = _uow.Shipment.GetAll();

            //filter by CustomerId
            if (f_Criteria.CustomerId > 0)
            {
                shipments = shipments.Where(s => s.CustomerId == f_Criteria.CustomerId);
            }

            //filter by CustomerType
            if (f_Criteria.FilterCustomerType != null)
            {
                shipments = shipments.Where(s => s.CustomerType == f_Criteria.FilterCustomerType.ToString());
            }

            //filter by UserId
            if (f_Criteria.UserId != null)
            {
                shipments = shipments.Where(s => s.UserId == f_Criteria.UserId);
            }

            var shipmentsDto = Mapper.Map<List<ShipmentDTO>>(shipments.ToList());
            return await Task.FromResult(shipmentsDto);
        }

        public Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            throw new NotImplementedException();
        }

        public async Task<InvoiceDTO> GetInvoiceByWaybill(string waybill)
        {
            var invoice = await _invoiceService.GetInvoiceByWaybill(waybill);
            return invoice;
        }

        public Task<IEnumerable<InvoiceDTO>> GetInvoices()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            var result = await _iShipmentTrackService.TrackShipment(waybillNumber);
            return result;
        }

        public Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount()
        {
            throw new NotImplementedException();
        }
    }
}
