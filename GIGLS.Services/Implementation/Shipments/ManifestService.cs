using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Linq;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestService : IManifestService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _service;

        public ManifestService(IUnitOfWork uow, INumberGeneratorMonitorService service)
        {
            _uow = uow;
            _service = service;
            MapperConfig.Initialize();
        }

        public async Task<object> AddManifest(ManifestDTO manifest)
        {
            try
            {
                if (await _uow.Manifest.ExistAsync(c => c.ManifestCode.ToLower() == manifest.ManifestCode.ToLower()))
                {
                    throw new GenericException("Manifest code already exist");
                }

                var newManifest = new Manifest
                {
                    DateTime = DateTime.Now,
                    ManifestCode = manifest.ManifestCode,
                    //MasterWaybill = manifest.MasterWaybill,
                    //DispatechedBy = user logged on
                    ShipmentId = manifest.ShipmentId,
                    FleetTripId = manifest.FleetTripId
                };
                _uow.Manifest.Add(newManifest);
                await _uow.CompleteAsync();
                return new { id = newManifest.ManifestId };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteManifest(int manifestId)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }
                _uow.Manifest.Remove(manifest);
                _uow.Complete();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ManifestDTO> GetManifestById(int manifestId)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }

                ManifestDTO dto = new ManifestDTO
                {
                    DateTime = manifest.DateTime,
                    ManifestCode = manifest.ManifestCode,
                    //dto.MasterWaybill = manifest.MasterWaybill;
                    //dto.ReceiverBy = manifest.ReceiverBy.FirstName + " " + manifest.ReceiverBy.LastName;
                    //dto.DispatchedBy = manifest.DispatchedBy.FirstName + " " + manifest.DispatchedBy.LastName;
                    ShipmentId = manifest.ShipmentId,
                    FleetTripId = manifest.FleetTripId
                };
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ManifestDTO> GetManifestByCode(string manifest)
        {
            try
            {
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                if (manifestObj == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }

                var manifestDTO = Mapper.Map<ManifestDTO>(manifestObj);
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateManifestCode(ManifestDTO manifestDTO)
        {
            try
            {
                var manifestCode = await _service.GenerateNextNumber(NumberGeneratorType.Manifest);
                return manifestCode;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<List<ManifestDTO>> GetManifests()
        {
            try
            {
                List<ManifestDTO> manifests = _uow.Manifest.GetAll().Select(x => new ManifestDTO
                {
                    DateTime = x.DateTime,
                    ManifestCode = x.ManifestCode,
                    //MasterWaybill = x.MasterWaybill,
                    ReceiverBy = x.ReceiverById
                    //ReceiverBy = x.ReceiverBy.FirstName + " " + x.ReceiverBy.LastName,
                    //DispatechedBy = x.DispatechedBy.FirstName + " " + x.DispatechedBy.LastName
                    //shipment mapped to manfest
                }).ToList();
                return Task.FromResult(manifests);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateManifest(int manifestId, ManifestDTO manifestDto)
        {
            try
            {
                var manifest = await _uow.Manifest.GetAsync(manifestId);
                if (manifest == null)
                {
                    throw new GenericException("Manifest information does not exist");
                }

                manifest.DateTime = DateTime.Now;
                //manifest.DispatechedBy = _uow.User.SingleOrDefault(x => x.ApplicationUserId == manifestDto.DispatechedBy);
                //manifest.MasterWaybill = manifest.MasterWaybill;
                manifest.ShipmentId = manifestDto.ShipmentId;
                manifest.FleetTripId = manifestDto.FleetTripId;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
