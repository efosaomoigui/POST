﻿using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IShipmentCategory : IRepository<InboundShipmentCategory>
    {
        Task<List<InboundShipmentCategoryDTO>> GetInboundCategory(int countryId);
    }
}
