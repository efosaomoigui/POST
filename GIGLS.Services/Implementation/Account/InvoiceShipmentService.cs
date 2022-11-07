using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IServices.Account;
using POST.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Account
{
    public class InvoiceShipmentService : IInvoiceShipmentService
    {
        private readonly IUnitOfWork _uow;

        public InvoiceShipmentService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<InvoiceShipmentDTO>> GetInvoiceShipments()
        {
            var invoiceShipments = await _uow.InvoiceShipment.GetInvoiceShipmentsAsync();
            return invoiceShipments;
        }

        public async Task<InvoiceShipmentDTO> GetInvoiceShipmentById(int invoiceShipmentId)
        {
            var invoiceShipment = await _uow.InvoiceShipment.GetAsync(invoiceShipmentId);

            if (invoiceShipment == null)
            {
                throw new GenericException("InvoiceShipment does not exists");
            }
            return Mapper.Map<InvoiceShipmentDTO>(invoiceShipment);
        }

        public async Task<object> AddInvoiceShipment(InvoiceShipmentDTO invoiceShipmentDto)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceShipmentDto.InvoiceId);
            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            var shipment = await _uow.Shipment.GetAsync(invoiceShipmentDto.ShipmentId);
            if (shipment == null)
            {
                throw new GenericException("Shipment does not exists");
            }

            var newInvoiceShipment = Mapper.Map<InvoiceShipment>(invoiceShipmentDto);

            _uow.InvoiceShipment.Add(newInvoiceShipment);
            await _uow.CompleteAsync();
            return new { id = newInvoiceShipment.InvoiceShipmentId };
        }

        public async Task UpdateInvoiceShipment(int invoiceShipmentId, InvoiceShipmentDTO invoiceShipmentDto)
        {
            var invoice = await _uow.Invoice.GetAsync(invoiceShipmentDto.InvoiceId);
            if (invoice == null)
            {
                throw new GenericException("Invoice does not exists");
            }

            var shipment = await _uow.Shipment.GetAsync(invoiceShipmentDto.ShipmentId);
            if (shipment == null)
            {
                throw new GenericException("Shipment does not exists");
            }

            var invoiceShipment = await _uow.InvoiceShipment.GetAsync(invoiceShipmentId);

            if (invoiceShipment == null)
            {
                throw new GenericException("InvoiceShipment does not exists");
            }

            invoiceShipment.InvoiceId = invoiceShipmentDto.InvoiceId;
            invoiceShipment.ShipmentId = invoiceShipmentDto.ShipmentId;

            await _uow.CompleteAsync();
        }

        public async Task RemoveInvoiceShipment(int invoiceShipmentId)
        {
            var invoiceShipment = await _uow.InvoiceShipment.GetAsync(invoiceShipmentId);

            if (invoiceShipment == null)
            {
                throw new GenericException("InvoiceShipment does not exists");
            }
            _uow.InvoiceShipment.Remove(invoiceShipment);
            await _uow.CompleteAsync();
        }
    }
}
