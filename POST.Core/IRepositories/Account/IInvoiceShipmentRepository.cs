using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Account
{
    public interface IInvoiceShipmentRepository : IRepository<InvoiceShipment>
    {
        Task<List<InvoiceShipmentDTO>> GetInvoiceShipmentsAsync();
    }
}
