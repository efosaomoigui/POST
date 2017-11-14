using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private INumberGeneratorMonitorService _service { get; set; }
        private IShipmentService _shipmentService { get; set; }
        private ICustomerService _customerService { get; set; }

        public InvoiceService(IUnitOfWork uow, INumberGeneratorMonitorService service,
            IShipmentService shipmentService, ICustomerService customerService)
        {
            _uow = uow;
            _service = service;
            _shipmentService = shipmentService;
            _customerService = customerService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<InvoiceDTO>> GetInvoices()
        {
            var invoices = await _uow.Invoice.GetInvoicesAsync();
            return invoices;
        }

        public async Task<InvoiceDTO> GetInvoiceById(int invoiceId)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            var invoiceDTO = Mapper.Map<InvoiceDTO>(invoice);
            
            // get Shipment
            var waybill = invoiceDTO.Waybill;
            invoiceDTO.Shipment = await _shipmentService.GetShipment(waybill);

            // get Customer
            invoiceDTO.Customer = invoiceDTO.Shipment.CustomerDetails;

            return invoiceDTO;
        }

        public async Task<object> AddInvoice(InvoiceDTO invoiceDto)
        {
            var newInvoice = Mapper.Map<Invoice>(invoiceDto);
            newInvoice.InvoiceNo = await _service.GenerateNextNumber(NumberGeneratorType.Invoice);

            _uow.Invoice.Add(newInvoice);
            await _uow.CompleteAsync();
            return new { id = newInvoice.InvoiceId };
        }

        public async Task UpdateInvoice(int invoiceId, InvoiceDTO invoiceDto)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            invoice.InvoiceId = invoiceId;
            invoice.InvoiceNo = invoiceDto.InvoiceNo;
            invoice.PaymentDate = invoiceDto.PaymentDate;
            invoice.PaymentMethod = invoiceDto.PaymentMethod;
            invoice.PaymentStatus = invoiceDto.PaymentStatus;
            invoice.Waybill = invoiceDto.Waybill;

            await _uow.CompleteAsync();
        }

        public async Task RemoveInvoice(int invoiceId)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceId);

            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }
            _uow.Invoice.Remove(invoice);
            await _uow.CompleteAsync();
        }
    }
}
