using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
                                      //IsActive = gw.IsActive,
                                      MovementManifestCode = gw.MovementManifestCode,
                                      MovementManifestNumberId = gw.MovementManifestNumberId,
                                      ServiceCentreId = gw.DepartureServiceCentreId,
                                      UserId = gw.UserId,
                                      ServiceCentreCode = gw.DepartureServiceCentre.Code
                                  };
            return Task.FromResult(movementNumberDto.ToList());
        }
    }
}
