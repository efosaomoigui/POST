﻿using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.User;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IShipmentExportRepository : IRepository<ShipmentExport>
    {
        Task<List<ShipmentExportDTO>> GetShipmentExport();
        Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported();
        Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported(NewFilterOptionsDto filterOptionsDto);
    }
}
