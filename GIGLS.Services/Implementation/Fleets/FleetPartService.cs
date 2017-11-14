using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using AutoMapper;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetPartService : IFleetPartService
    {
        private readonly IUnitOfWork _uow;
        public FleetPartService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddFleetPart(int modelId, FleetPartDTO part)
        {
            try
            {
                if (!await _uow.FleetModel.ExistAsync(c => c.ModelId == modelId))
                {
                    throw new GenericException("Model informtion does not exist");
                }

                if (await _uow.FleetPart.ExistAsync(c => c.PartName.ToLower() == part.PartName.Trim().ToLower()))
                {
                    throw new GenericException($"{part.PartName} Already Exist");
                }

                var newPart = Mapper.Map<FleetPart>(part);                
                _uow.FleetPart.Add(newPart);
                await _uow.CompleteAsync();
                return new { Id = newPart.PartId};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFleetPart(int partId)
        {
            try
            {
                var part = await _uow.FleetPart.GetAsync(partId);
                if (part == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                _uow.FleetPart.Remove(part);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetPartDTO> GetFleetPartById(int partId)
        {
            try
            {
                var part = await _uow.FleetPart.GetAsync(partId);
                if (part == null)
                {
                    throw new GenericException("Information does not Exist");
                }

                var partDto = Mapper.Map<FleetPartDTO>(part);
                partDto.ModelName = part.Model.ModelName;
                return partDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FleetPartDTO>> GetFleetParts()
        {
            try
            {
                return await _uow.FleetPart.GetFleetParts();                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateFleetPart(int partId, FleetPartDTO partDto)
        {
            try
            {
                if (await _uow.FleetModel.ExistAsync(c => c.ModelId == partDto.ModelId))
                {
                    throw new GenericException("Fleet Model Information does not Exist");
                }

                var part = await _uow.FleetPart.GetAsync(partId);

                if (part == null || partDto.PartId != partId)
                {
                    throw new GenericException("Information does not Exist");
                }

                part.PartName = partDto.PartName;
                part.ModelId = partDto.ModelId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
