using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class TransitManifestService : ITransitManifestService
    {
        private readonly IUnitOfWork _uow;

        public TransitManifestService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddTransitManifest(TransitManifestDTO transitManifestDTO)
        {
            try
            {
                if (await _uow.TransitManifest.ExistAsync(c => c.ManifestCode == transitManifestDTO.ManifestCode))
                {
                    throw new GenericException("TransitManifest already exists");
                }
                var newTransitManifest = Mapper.Map<TransitManifest>(transitManifestDTO);
                _uow.TransitManifest.Add(newTransitManifest);
                await _uow.CompleteAsync();
                return new { Id = newTransitManifest.TransitManifestId };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
