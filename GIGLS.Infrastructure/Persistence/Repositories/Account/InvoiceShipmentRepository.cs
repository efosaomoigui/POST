using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class InvoiceShipmentRepository : Repository<InvoiceShipment, GIGLSContext>, IInvoiceShipmentRepository
    {
        public InvoiceShipmentRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<List<InvoiceShipmentDTO>> GetInvoiceShipmentsAsync()
        {
            var invoiceShipments = Context.InvoiceShipment;
            var invoiceShipmentDto = from invoiceShipment in invoiceShipments
                                     select new InvoiceShipmentDTO
                                     {
                                         InvoiceShipmentId = invoiceShipment.InvoiceShipmentId,
                                         InvoiceId = invoiceShipment.InvoiceId,
                                         ShipmentId = invoiceShipment.ShipmentId
                                     };
            return Task.FromResult(invoiceShipmentDto.ToList());
        }
    }
}
