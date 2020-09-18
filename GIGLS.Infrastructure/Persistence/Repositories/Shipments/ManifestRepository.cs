using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ManifestRepository : Repository<Manifest, GIGLSContext>, IManifestRepository
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

        public Task<List<ManifestDTO>> GetManifest(List<Manifest> manifests)
        {
            try
            {
                var manifestDto = from r in manifests
                                  select new ManifestDTO()
                                  {
                                      DateTime = r.DateTime,
                                      ManifestCode = r.ManifestCode,
                                      ManifestId = r.ManifestId,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified,
                                      SuperManifestCode = r.SuperManifestCode,
                                      SuperManifestStatus = r.SuperManifestStatus,
                                      HasSuperManifest = r.HasSuperManifest,
                                      DepartureServiceCentreId = r.DepartureServiceCentreId,
                                      DestinationServiceCentreId = r.DestinationServiceCentreId,
                                      DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                      {
                                          Code = x.Code,
                                          Name = x.Name
                                      }).FirstOrDefault(),
                                      DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                      {
                                          Code = x.Code,
                                          Name = x.Name
                                      }).FirstOrDefault(),
                                  };

                return Task.FromResult(manifestDto.OrderByDescending(x => x.DateModified).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ManifestDTO>> GetManifestsInSuperManifest(List<string> manifestCodes)
        {
            try
            {
                var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.Where(s => manifestCodes.Contains(s.ManifestCode));

                var manifestDto = from mgw in manifestGroupwaybillMapping
                                  join p in _context.Manifest on mgw.ManifestCode equals p.ManifestCode
                                  join s in _context.GroupWaybillNumberMapping on mgw.GroupWaybillNumber equals s.GroupWaybillNumber
                                  where p.SuperManifestStatus == Core.Enums.SuperManifestStatus.AssignedSuperManifest
                                  select new ManifestDTO
                                  {
                                      ManifestCode = p.ManifestCode,
                                      DateCreated = p.DateCreated,
                                      DateModified = p.DateModified,
                                      ManifestType = p.ManifestType,
                                      DateTime = p.DateTime,
                                      IsDispatched = p.IsDispatched,
                                      IsReceived = p.IsReceived,
                                      HasSuperManifest = p.HasSuperManifest,
                                      SuperManifestStatus = p.SuperManifestStatus,
                                      IsDeleted = p.IsDeleted,
                                      SuperManifestCode = p.SuperManifestCode,
                                      DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                      {
                                          Code = x.Code,
                                          Name = x.Name,
                                          StationId = x.StationId,
                                          StationName = x.Station.StationName
                                      }).FirstOrDefault(),
                                      DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                      {
                                          Code = x.Code,
                                          Name = x.Name,
                                          StationId = x.StationId,
                                          StationName = x.Station.StationName
                                      }).FirstOrDefault()
                                  };

                return Task.FromResult(manifestDto.OrderByDescending(x => x.DateModified).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
