using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using System.Collections.Generic;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using AutoMapper;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetMakeService : IFleetMakeService
    {
        private readonly IUnitOfWork _uow;
        public FleetMakeService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddFleetMake(FleetMakeDTO make)
        {
            try
            {
                if (await _uow.FleetMake.ExistAsync(c => c.MakeName.ToLower() == make.MakeName.Trim().ToLower()))
                {
                    throw new GenericException($"{make.MakeName} Already Exist");
                }

                //clear the collections
                make.FleetModels.Clear();
                make.Fleets.Clear();

                var newMaker =  Mapper.Map<FleetMake>(make);    
                _uow.FleetMake.Add(newMaker);
                await _uow.CompleteAsync();
                return new { Id = newMaker.MakeId};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFleetMake(int makeId)
        {
            try
            {
                var maker = await _uow.FleetMake.GetAsync(makeId);
                if (maker == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                _uow.FleetMake.Remove(maker);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetMakeDTO> GetFleetMakeById(int makeId)
        {
            try
            {
                var maker = await _uow.FleetMake.GetAsync(makeId);
                if (maker == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                return Mapper.Map<FleetMakeDTO>(maker);
                //return new FleetMakeDTO
                //{
                //    MakeId = maker.MakeId,
                //    MakeName = maker.MakeName,
                //    DateCreated = maker.DateCreated,
                //    DateModified = maker.DateModified
                //    //VehicleModel = maker.VehicleModels.Select(m => new VehicleModelDTO
                //    //{
                //    //    VehicleModelId = m.VehicleModelId,
                //    //    ModelName = m.ModelName
                //    //}).ToList(),
                //    //Vehicles = maker.Vehicles.Select(v => new VehicleDTO
                //    //{
                //    //    VehicleChassisNumber = v.VehicleChassisNumber,
                //    //    VehicleEngineNumber = v.VehicleEngineNumber,
                //    //    VehicleId = v.VehicleId,
                //    //    VehicleRegistrationNumber = v.VehicleRegistrationNumber,
                //    //    VehicleStatus = v.VehicleStatus.ToString(),
                //    //    DateCreated = v.DateCreated,
                //    //    DateModified = v.DateModified
                //    //}).ToList()               
                //};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FleetMakeDTO>> GetFleetMakes()
        {
            return await _uow.FleetMake.GetFleetMakers();
        }

        //public Task<List<VehicleMakeDTO>> GetVehicleMakes()
        //{
        //    try
        //    {

        //        List<VehicleMakeDTO> option = _uow.VehicleMake.GetAll().Select(s => new VehicleMakeDTO
        //        {
        //            VehicleMakeId = s.VehicleMakeId,
        //            MakeName = s.MakeName,
        //            //VehicleModel = s.VehicleModels.Select(v => new VehicleModelDTO
        //            //{
        //            //    VehicleModelId = v.VehicleModelId,
        //            //    ModelName = v.ModelName,
        //            //    VehicleMake = s.MakeName,
        //            //    Vehicles = v.Vehicles.Select(vehicle => new VehicleDTO
        //            //    {
        //            //        VehicleId = vehicle.VehicleId,
        //            //        VehicleChassisNumber = vehicle.VehicleChassisNumber,
        //            //        VehicleEngineNumber = vehicle.VehicleEngineNumber,
        //            //        VehicleRegistrationNumber = vehicle.VehicleRegistrationNumber
        //            //    }).ToList()
        //            //}).ToList(),
        //        }).ToList();
        //        return Task.FromResult(option);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task UpdateFleetMake(int makeId, FleetMakeDTO makeDto)
        {
            try
            {
                var maker = await _uow.FleetMake.GetAsync(makeId);
                if (maker == null || makeDto.MakeId != makeId)
                {
                    throw new GenericException("Information does not Exist");
                }
                maker.MakeName = makeDto.MakeName;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
