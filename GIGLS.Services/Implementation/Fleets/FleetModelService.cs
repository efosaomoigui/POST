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
    public class FleetModelService : IFleetModelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFleetMakeService _makeService;

        public FleetModelService(IUnitOfWork uow, IFleetMakeService makeService)
        {
            _uow = uow;
            _makeService = makeService;
            MapperConfig.Initialize();
        }
        public async Task<object> AddFleetModel(FleetModelDTO model)
        {
            try
            {
                if (!await _uow.FleetMake.ExistAsync(c => c.MakeId == model.MakeId))
                {
                    throw new GenericException("Fleet Make Information does not Exist");
                }

                if (await _uow.FleetModel.ExistAsync(c => c.ModelName.ToLower() == model.ModelName.Trim().ToLower()))
                {
                    throw new GenericException($"{model.ModelName} Already Exist");
                }

                //clear list
                model.Fleets.Clear();

                var newModel = Mapper.Map<FleetModel>(model);
                _uow.FleetModel.Add(newModel);
                await _uow.CompleteAsync();
                return new { Id = newModel.ModelId};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFleetModel(int modelId)
        {
            try
            {
                var model = await _uow.FleetModel.GetAsync(modelId);
                if (model == null)
                {
                    throw new GenericException("Fleet Model Information does not Exist");
                }
                _uow.FleetModel.Remove(model);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetModelDTO> GetFleetModelById(int modelId)
        {
            try
            {
                var model = await _uow.FleetModel.GetAsync(x => x.ModelId == modelId, "FleetMake");
                if (model == null)
                {
                    throw new GenericException("Fleet Model Information does not Exist");
                }
                var modelDto =  Mapper.Map<FleetModelDTO>(model);
                modelDto.MakeName = model.FleetMake.MakeName;
                return modelDto;

                //t _uow.Station.GetAsync(s => s.StationId == stationId, "State");

                //return new FleetModelDTO
                //{
                //    ModelId = model.ModelId,
                //    ModelName = model.ModelName,
                //    DateCreated = model.DateCreated,
                //    DateModified = model.DateModified,
                //    MakeName = model.FleetMake.MakeName,
                //    Fleets = model.Fleets.Select(v => new FleetDTO
                //    {
                //        ChassisNumber = v.ChassisNumber,
                //        EngineNumber = v.EngineNumber,
                //        FleetId = v.FleetId,
                //        RegistrationNumber = v.RegistrationNumber,
                //        FleetStatusName = v.FleetStatus.ToString(),
                //        FleetStatus = v.FleetStatus,
                //        FleetType = v.FleetType,
                //        FleetTypeName = v.FleetType.ToString(),
                //        DateCreated = v.DateCreated,
                //        DateModified = v.DateModified
                //    }).ToList()
                //};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FleetModelDTO>> GetFleetModels()
        {
            return await _uow.FleetModel.GetFleetModels();
        }

        //public Task<List<VehicleModelDTO>> GetVehicleModels()
        //{
        //    try
        //    {
        //        List<VehicleModelDTO> model = _uow.VehicleModel.GetAll().Select(s => new VehicleModelDTO
        //        {
        //            VehicleModelId = s.VehicleModelId,
        //            ModelName = s.ModelName,
        //            DateCreated = s.DateCreated,
        //            DateModified = s.DateModified,
        //            VehicleMake = s.VehicleMake.MakeName,
        //            Vehicles = s.Vehicles.Select(v => new VehicleDTO
        //            {
        //                VehicleChassisNumber = v.VehicleChassisNumber,
        //                VehicleEngineNumber = v.VehicleEngineNumber,
        //                VehicleId = v.VehicleId,
        //                VehicleRegistrationNumber = v.VehicleRegistrationNumber,
        //                VehicleStatus = v.VehicleStatus.ToString(),
        //                DateCreated = v.DateCreated,
        //                DateModified = v.DateModified
        //            }).ToList()
        //        }).ToList();
        //        return Task.FromResult(model);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task UpdateFleetModel(int modelId, FleetModelDTO modelDto)
        {
            try
            {
                await _makeService.GetFleetMakeById(modelDto.MakeId);

                var model = await _uow.FleetModel.GetAsync(modelId);
                if (model == null || modelDto.ModelId != modelId)
                {
                    throw new GenericException("Fleet Model Information does not Exist");
                }
                model.ModelName = modelDto.ModelName;
                model.MakeId = modelDto.MakeId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
