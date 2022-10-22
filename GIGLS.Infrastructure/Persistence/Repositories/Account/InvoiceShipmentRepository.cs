using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IRepositories.Account;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Account
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
