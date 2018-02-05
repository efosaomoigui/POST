﻿using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentCancelRepository : IRepository<ShipmentCancel>
    {
        Task<List<ShipmentCancelDTO>> GetShipmentCancels();
    }
}
