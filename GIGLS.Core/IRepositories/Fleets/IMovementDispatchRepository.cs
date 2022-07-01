using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Core.IRepositories.Fleets
{
    public interface IMovementDispatchRepository : IRepository<MovementDispatch>   
    {
        Task<List<MovementDispatchDTO>> GetDispatchAsync(int[] serviceCentreIds);
        Task<List<MovementDispatchDTO>> GetMovementmanifestDispatchForPartner(string userId);
        Task<List<MovementDispatchDTO>> GetMovementmanifestDispatchForPartnerCompleted(string userId, DateTime start, DateTime end);

        //Task<List<MovementDispatchDTO>> CheckForOutstandingDispatch(string driverId); 
    }
}