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
    public interface IGiglgoStationRepository : IRepository<GiglgoStation>
    {

        Task<List<GiglgoStationDTO>> GetGoStations();
        Task<GiglgoStationDTO> GetGoStationsById(int stationId);
    }
}
