using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IMissingShipmentRepository : IRepository<MissingShipment>
    {
        Task<List<MissingShipmentDTO>> GetMissingShipments();
        Task<MissingShipmentDTO> GetMissingShipmentById(int missingShipmentId);
    }
}
