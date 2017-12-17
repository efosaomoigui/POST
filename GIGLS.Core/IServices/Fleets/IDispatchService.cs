﻿using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IDispatchService : IServiceDependencyMarker
    {
        Task<List<DispatchDTO>> GetDispatchs();
        Task<DispatchDTO> GetDispatchById(int DispatchId);
        Task<object> AddDispatch(DispatchDTO Dispatch);
        Task UpdateDispatch(int DispatchId, DispatchDTO Dispatch);
        Task DeleteDispatch(int DispatchId);
    }
}