using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class HomeDeliveryLocationService: IHomeDeliveryLocationService
    {
        private readonly IUnitOfWork _uow;

        public HomeDeliveryLocationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddHomeDeliveryLocation(HomeDeliveryLocationDTO homeDeliveryLocationDTO)
        {
            try
            {
                var state = await _uow.State.GetAsync(homeDeliveryLocationDTO.StateId);
                if(state == null)
                {
                    throw new GenericException("State does not exist");
                }
                var location = await _uow.HomeDeliveryLocation.GetAsync(x => x.LGAName.ToLower() == homeDeliveryLocationDTO.LGAName.ToLower() && x.StateId == state.StateId);

                if (location != null)
                {
                    throw new GenericException("Location Information already exists");
                }
                homeDeliveryLocationDTO.LGAState = state.StateName;
                var newlocation = Mapper.Map<HomeDeliveryLocation>(homeDeliveryLocationDTO);
                _uow.HomeDeliveryLocation.Add(newlocation);
                await _uow.CompleteAsync();
                return new { Id = newlocation.HomeDeliveryLocationId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HomeDeliveryLocationDTO> GetHomeDeliveryLocationById(int locationId)
        {
            try
            {
                var location = await _uow.HomeDeliveryLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location information does not exist");
                }

                var locationDto = Mapper.Map<HomeDeliveryLocationDTO>(location);
                return locationDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<HomeDeliveryLocationDTO>> GetHomeDeliveryLocations()
        {
            var locations = _uow.HomeDeliveryLocation.GetAll().OrderBy(x => x.LGAName);
            return Task.FromResult(Mapper.Map<IEnumerable<HomeDeliveryLocationDTO>>(locations));
        }

        public async Task UpdateHomeDeliveryLocation(int locationId, HomeDeliveryLocationDTO homeDeliveryLocationDTO)
        {
            try
            {
                var location = await _uow.HomeDeliveryLocation.GetAsync(locationId);

                var state = await _uow.State.GetAsync(homeDeliveryLocationDTO.StateId);
                if (state == null)
                {
                    throw new GenericException("State does not exist");
                }

                //To check if the update already exists
                var locationExist = await _uow.HomeDeliveryLocation.ExistAsync(c => c.LGAName.ToLower() == homeDeliveryLocationDTO.LGAName.ToLower() && c.StateId == state.StateId);
                if (location == null || homeDeliveryLocationDTO.HomeDeliveryLocationId != locationId)
                {
                    throw new GenericException("Location Information does not exist");
                }
                else if (locationExist == true)
                {
                    throw new GenericException("Location Information already exists");
                }
                location.LGAName = homeDeliveryLocationDTO.LGAName;
                location.LGAState = state.StateName;
                location.StateId = state.StateId;
                location.Status = homeDeliveryLocationDTO.Status;
                _uow.Complete();

            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task UpdateHomeDeliveryLocation(int locationId, bool status)
        {
            try
            {
                var location = await _uow.HomeDeliveryLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.Status = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteHomeDeliveryLocation(int locationId)
        {
            try
            {
                var location = await _uow.HomeDeliveryLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location information does not exist");
                }
                _uow.HomeDeliveryLocation.Remove(location);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HomeDeliveryLocationDTO>> GetActiveLocations()
        {
            try
            {
                return await _uow.HomeDeliveryLocation.GetActiveHomeDeliveryLocations();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
