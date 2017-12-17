using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using System.Linq;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchRepository : Repository<Dispatch, GIGLSContext>, IDispatchRepository
    {
        private GIGLSContext _context;

        public DispatchRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<DispatchDTO>> GetDispatchAsync()
        {
            try
            {
                var dispatchs = Context.Dispatch;

                var dispatchDto = from r in dispatchs
                                  select new DispatchDTO
                                  {
                                      DispatchId = r.DispatchId,
                                      RegistrationNumber = r.RegistrationNumber,
                                      ManifestNumber = r.ManifestNumber,
                                      Amount = r.Amount,
                                      RescuedDispatchId = r.RescuedDispatchId,
                                      DriverDetail = r.DriverDetail,
                                      DispatchedBy = r.DispatchedBy,
                                      ReceivedBy = r.ReceivedBy,
                                      DispatchCategory = r.DispatchCategory,
                                      DepartureId = r.DepartureId,
                                      DestinationId = r.DestinationId,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified
                                  };
                return Task.FromResult(dispatchDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
