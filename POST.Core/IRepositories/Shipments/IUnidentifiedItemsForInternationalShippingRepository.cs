using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.User;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IUnidentifiedItemsForInternationalShippingRepository : IRepository<UnidentifiedItemsForInternationalShipping>
    {
        //Task<List<InternationalCargoManifestDTO>> GetIntlCargoManifests(NewFilterOptionsDto filter);
        //Task<InternationalCargoManifestDTO> GetIntlCargoManifestByID(int cargoID);

    }
}
