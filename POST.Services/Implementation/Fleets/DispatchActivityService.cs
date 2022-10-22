using POST.Core.IServices.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Fleets;
using POST.Core;
using System.Linq;
using POST.Core.Domain;
using AutoMapper;
using POST.Infrastructure;

namespace POST.Services.Implementation.Fleets
{
    public class DispatchActivityService : IDispatchActivityService
    {
        private readonly IUnitOfWork _uow;

        public DispatchActivityService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddDispatchActivity(DispatchActivityDTO dispatchActivity)
        {
            try
            {
                var newDispatchActivity = Mapper.Map<DispatchActivity>(dispatchActivity);
                _uow.DispatchActivity.Add(newDispatchActivity);
                await _uow.CompleteAsync();
                return new { Id = newDispatchActivity.DispatchActivityId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteDispatchActivity(int dispatchActivityId)
        {
            try
            {
                var dispatchActivity = await _uow.DispatchActivity.GetAsync(dispatchActivityId);
                if (dispatchActivity == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                _uow.DispatchActivity.Remove(dispatchActivity);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DispatchActivityDTO>> GetDispatchActivities()
        {
            return await _uow.DispatchActivity.GetDispatchActivitiesAsync();
        }

        public async Task<DispatchActivityDTO> GetDispatchActivityById(int dispatchActivityId)
        {
            try
            {
                var dispatchActivity = await _uow.DispatchActivity.GetAsync(dispatchActivityId);
                if (dispatchActivity == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                return Mapper.Map<DispatchActivityDTO>(dispatchActivity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateDispatchActivity(int dispatchActivityId, DispatchActivityDTO dispatchActivityDTO)
        {
            try
            {
                var dispatchActivity = await _uow.DispatchActivity.GetAsync(dispatchActivityId);
                if (dispatchActivity == null || dispatchActivityDTO.DispatchActivityId != dispatchActivityId)
                {
                    throw new GenericException("Information does not Exist");
                }

                dispatchActivity.DispatchActivityId = dispatchActivityDTO.DispatchActivityId;
                dispatchActivity.DispatchId = dispatchActivityDTO.DispatchId;
                dispatchActivity.Description = dispatchActivityDTO.Description;
                dispatchActivity.Location = dispatchActivityDTO.Location;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
