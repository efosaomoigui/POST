﻿using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IShipmentExportRepository : IRepository<ShipmentExport>
    {
        Task<List<ShipmentExportDTO>> GetShipmentExport();
        Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported();
        Task<List<ShipmentExportDTO>> GetShipmentExportNotYetExported(NewFilterOptionsDto filterOptionsDto);
    }
}