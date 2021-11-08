using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IUnidentifiedItemsForInternationalShippingRepository : IRepository<UnidentifiedItemsForInternationalShipping>
    {
        //Task<List<InternationalCargoManifestDTO>> GetIntlCargoManifests(NewFilterOptionsDto filter);
        //Task<InternationalCargoManifestDTO> GetIntlCargoManifestByID(int cargoID);

    }
}
