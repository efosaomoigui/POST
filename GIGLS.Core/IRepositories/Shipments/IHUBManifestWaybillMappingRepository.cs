﻿using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IHUBManifestWaybillMappingRepository : IRepository<HUBManifestWaybillMapping>
    {
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds);
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests);
    }
}