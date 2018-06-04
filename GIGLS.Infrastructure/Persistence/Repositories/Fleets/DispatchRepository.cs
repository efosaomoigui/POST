using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchRepository : Repository<Dispatch, GIGLSContext>, IDispatchRepository
    {
        private GIGLSContext _context;

        public DispatchRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<DispatchDTO>> GetDispatchAsync(int[] serviceCentreIds)
        {
            try
            {
                var dispatchs = _context.Dispatch.AsQueryable();
                if (serviceCentreIds.Length > 0)
                {
                    dispatchs = dispatchs.Where(s => s.ServiceCentreId != null);
                    dispatchs = dispatchs.Where(s => serviceCentreIds.Contains((int)s.ServiceCentreId));
                }

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
                                      DateModified = r.DateModified,
                                      ServiceCentreId = r.ServiceCentreId,
                                      ServiceCentre = new ServiceCentreDTO
                                      {
                                          Code = r.ServiceCentre.Code,
                                          Name = r.ServiceCentre.Name
                                      },
                                      UserDetail = Context.Users.Where(c => c.Id == r.DriverDetail).Select(x => new UserDTO
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Department = x.Department,
                                          Designation = x.Designation,
                                          Email = x.Email,
                                          PhoneNumber = x.PhoneNumber,
                                          Organisation = x.Organisation,
                                          PictureUrl = x.PictureUrl,
                                          Gender = x.Gender                                          
                                      }).FirstOrDefault()
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
