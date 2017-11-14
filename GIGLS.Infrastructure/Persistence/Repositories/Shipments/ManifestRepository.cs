using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ManifestRepository : Repository<Manifest, GIGLSContext> , IManifestRepository
    {
        private GIGLSContext _context;
        public ManifestRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<ManifestDTO>> GetManifests()
        {
            try
            {
                var manifest = Context.Manifest;

                var manifestDto = from r in manifest
                                 select new ManifestDTO()
                                 {
                                     DateTime = r.DateTime,
                                     ManifestCode = r.ManifestCode,
                                     ManifestId = r.ManifestId,
                                     //MasterWaybill = r.MasterWaybill,
                                     //ReceiverBy
                                     //VehicleTrip
                                     //Shipments
                                     //DispatchedBy = r.DispatechedBy,                                     
                                 };
                return Task.FromResult(manifestDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
