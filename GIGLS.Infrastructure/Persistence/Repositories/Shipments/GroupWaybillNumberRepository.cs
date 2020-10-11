using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class GroupWaybillNumberRepository : Repository<GroupWaybillNumber, GIGLSContext>, IGroupWaybillNumberRepository
    {
        public GroupWaybillNumberRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<GroupWaybillNumberDTO>> GetGroupWaybills()
        {
            var groupwaybill = Context.GroupWaybillNumber.Include("ServiceCentre");

            var groupwaybillDto = from gw in groupwaybill
                                  select new GroupWaybillNumberDTO
                                  {
                                      IsActive = gw.IsActive,
                                      GroupWaybillCode = gw.GroupWaybillCode,
                                      GroupWaybillNumberId = gw.GroupWaybillNumberId,
                                      ServiceCentreId = gw.ServiceCentreId,
                                      UserId = gw.UserId,
                                      ServiceCentreCode = gw.ServiceCentre.Name
                                  };
            return Task.FromResult(groupwaybillDto.ToList());
        }
    }

    public class MovementManifestNumberRepository : Repository<MovementManifestNumber, GIGLSContext>, IMovementManifestNumberRepository
    {
        public MovementManifestNumberRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<MovementManifestNumberDTO>> GetMovementManifests()
        { 
            var MovementManifestNumber = Context.MovementManifestNumber.Include("ServiceCentre");  

            var movementNumberDto = from gw in MovementManifestNumber 
                                  select new MovementManifestNumberDTO
                                  {
                                      IsActive = gw.IsActive,
                                      MovementManifestCode = gw.MovementManifestCode,
                                      MovementManifestNumberId = gw.MovementManifestNumberId,
                                      ServiceCentreId = gw.ServiceCentreId,
                                      UserId = gw.UserId,
                                      ServiceCentreCode = gw.ServiceCentre.Name
                                  };
            return Task.FromResult(movementNumberDto.ToList());
        }
    }
}
