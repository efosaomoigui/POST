using GIGL.POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Account;
using POST.Core.DTO.Customers;
using POST.Core.DTO.DHL;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.Zone;
using POST.Core.Enums;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdParty.WebServices.Magaya.Business.New;
using ThirdParty.WebServices.Magaya.DTO;
using ThirdParty.WebServices.Magaya.Services;

namespace POST.Core.IServices.Shipments
{
    public interface IInternationalCargoManifestService : IServiceDependencyMarker
    {
        Task<InternationalCargoManifestDTO> AddCargoManifest(InternationalCargoManifestDTO cargoManifest);
        Task<List<InternationalCargoManifestDTO>> GetIntlCargoManifests(NewFilterOptionsDto filter);
        Task<InternationalCargoManifestDTO> GetIntlCargoManifestByID(int cargoID);
    }


}
