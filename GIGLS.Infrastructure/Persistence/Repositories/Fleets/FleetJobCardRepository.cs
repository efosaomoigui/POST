using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using Ninject.Activation;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class FleetJobCardRepository : Repository<FleetJobCard, GIGLSContext>, IFleetJobCardRepository
    {
        private GIGLSContext _context;

        public FleetJobCardRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FleetJobCardDto>> GetFleetJobCardsAsync()
        {
            try
            {
                var fleets = _context.FleetJobCard.Include("FleetManager").Include("EnterprisePartner");

                var fleetDto = from x in fleets where x.Status == FleetJobCardStatus.Open.ToString()
                    select new FleetJobCardDto()
                    {
                        FleetJobCardId = x.FleetJobCardId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        Status = x.Status,
                        VehiclePartToFix = x.VehiclePartToFix,
                        FleetManagerId = x.FleetManagerId,
                        Amount = x.Amount,
                        VehicleNumber = x.VehicleNumber
                    };
                return await Task.FromResult(fleetDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
