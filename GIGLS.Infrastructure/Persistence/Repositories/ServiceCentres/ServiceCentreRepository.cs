using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class ServiceCentreRepository : Repository<ServiceCentre, GIGLSContext>, IServiceCentreRepository
    {
        private GIGLSContext _context;
        public ServiceCentreRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentres()
        {
            try
            {                
                var centres = _context.ServiceCentre;
                var centreDto = from s in centres join sc in _context.Station on s.StationId equals sc.StationId
                                select new ServiceCentreDTO
                                {
                                    Name = s.Name,
                                    Address = s.Address,
                                    City = s.City,
                                    Email = s.Email,
                                    PhoneNumber = s.PhoneNumber,
                                    ServiceCentreId = s.ServiceCentreId,
                                    Code = s.Code,
                                    IsActive = s.IsActive,
                                    StationId = s.StationId,
                                    StationName = sc.StationName,
                                    StationCode = sc.StationCode                               
                              };
                return Task.FromResult(centreDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
