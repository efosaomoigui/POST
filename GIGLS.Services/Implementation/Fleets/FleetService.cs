using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core;
using GIGLS.Core.IServices.Fleets;
using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using AutoMapper;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetService : IFleetService
    {
        private readonly IUnitOfWork _uow;

        public FleetService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<FleetDTO>> GetFleets()
        {
            try
            {
                return await _uow.Fleet.GetFleets();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddFleet(FleetDTO fleet)
        {
            try
            {
                if (!await _uow.Partner.ExistAsync(c => c.PartnerId == fleet.PartnerId))
                {
                    throw new GenericException("PARTNER_INFORMATION_DOES_NOT_EXIST");
                }

                if (!await _uow.FleetModel.ExistAsync(c => c.ModelId == fleet.ModelId))
                {
                    throw new GenericException("MODEL_INFORMATION_DOES_NOT_EXIST");
                }

                if (await _uow.Fleet.ExistAsync(c => c.RegistrationNumber.ToLower() == fleet.RegistrationNumber.Trim().ToLower()))
                {
                    throw new GenericException($"FLEET WITH REGISTRATION NUMBER {fleet.RegistrationNumber} ALREADY EXIST");
                }

                var newFleet = Mapper.Map<Fleet>(fleet);

                _uow.Fleet.Add(newFleet);
                await _uow.CompleteAsync();
                return new { Id = newFleet.FleetId};
            }
            catch (Exception)
            {
                throw;
            }
        }
              
        public async Task DeleteFleet(int vehicleId)
        {
            try
            {
                var vehicle = await _uow.Fleet.GetAsync(vehicleId);
                if (vehicle == null)
                {
                    throw new GenericException("FLEET_INFROMATION_DOES_NOT_EXIST");
                }
                _uow.Fleet.Remove(vehicle);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetDTO> GetFleetById(int fleetId)
        {
            try
            {
                var fleet = await _uow.Fleet.GetAsync(x => x.FleetId == fleetId, "FleetModel,Partner");
                if (fleet == null)
                {
                    throw new GenericException("FLEET_INFORMATION_DOES_NOT_EXIST");
                }
                var fleetDto = Mapper.Map<FleetDTO>(fleet);
                fleetDto.ModelName = fleet.FleetModel.ModelName;
                fleetDto.PartnerName = fleet.Partner.PartnerName;
                return fleetDto;                
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task UpdateFleet(int fleetId, FleetDTO fleetDto)
        {
            try
            {
                if (!await _uow.Partner.ExistAsync(c => c.PartnerId == fleetDto.PartnerId))
                {
                    throw new GenericException("PARTNER _INFORMATION_DOES_NOT_EXIST");
                }

                if (!await _uow.FleetModel.ExistAsync(c => c.ModelId == fleetDto.ModelId))
                {
                    throw new GenericException("MODEL_INFORMATION_DOES_NOT_EXIST");
                }

                var fleet = await _uow.Fleet.GetAsync(fleetId);
                if (fleet == null || fleetDto.FleetId != fleetId)
                {
                    throw new GenericException("FLEET_INFORMATION_DOES_NOT_EXIST");
                }
                
                fleet.ChassisNumber = fleetDto.ChassisNumber;
                fleet.EngineNumber = fleetDto.EngineNumber;
                fleet.RegistrationNumber = fleetDto.RegistrationNumber;
                fleet.Status = fleetDto.Status;
                fleet.Description = fleetDto.Description;
                fleet.FleetType = fleetDto.FleetType;
                fleet.ModelId = fleetDto.ModelId;
                fleet.PartnerId = fleetDto.PartnerId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<int> GetFleetCapacity(int fleetId)
        {
            try
            {
                var fleet = await _uow.Fleet.GetAsync(x => x.FleetId == fleetId);
                if (fleet == null)
                {
                    throw new GenericException("FLEET_INFORMATION_DOES_NOT_EXIST");
                }
                return fleet.Capacity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetFleetCapacity(int fleetId, int capacity)
        {
            try
            {
                var fleet = await _uow.Fleet.GetAsync(fleetId);
                if (fleet == null)
                {
                    throw new GenericException("FLEET_INFORMATION_DOES_NOT_EXIST");
                }
                fleet.Capacity = capacity;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateFleetStatus(int fleetId, bool status)
        {
            try
            {
                var fleet = await _uow.Fleet.GetAsync(fleetId);
                if (fleet == null)
                {
                    throw new GenericException("FLEET_INFORMATION_DOES_NOT_EXIST");
                }
                fleet.Status = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
