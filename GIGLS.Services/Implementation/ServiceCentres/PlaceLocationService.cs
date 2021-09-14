using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.ServiceCentres
{
    public class PlaceLocationService : IPlaceLocationService
    {
        private readonly IUnitOfWork _uow;

        public PlaceLocationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
        public async Task<object> AddLocation(PlaceLocationDTO locationDto)
        {
            try
            {
                var state = await GetState(locationDto.StateId);
                var serviceCentre = await GetServiceCentre(locationDto.BaseStationId);

                var lga = await _uow.PlaceLocation.GetAsync(x => x.LocationName.ToLower() == locationDto.LocationName.ToLower() && x.StateId == locationDto.StateId);

                if (lga != null)
                {
                    throw new GenericException("Location Information already exists");
                }
                locationDto.StateName = state.StateName;
                locationDto.BaseStation = serviceCentre.Name;
                var newlocation = Mapper.Map<PlaceLocation>(locationDto);
                _uow.PlaceLocation.Add(newlocation);
                await _uow.CompleteAsync();
                return new { Id = newlocation.LocationId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteLocation(int locationId)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location information does not exist");
                }
                _uow.PlaceLocation.Remove(location);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PlaceLocationDTO> GetLocationById(int locationId)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetLocationById(locationId);
                if (location == null)
                {
                    throw new GenericException("Location information does not exist");
                }

                return location;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PlaceLocationDTO>> GetLocations()
        {
            return await _uow.PlaceLocation.GetLocations();
        }

        public async Task UpdateExpressHomeDeliveryLocation(int locationId, bool status)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.IsExpressHomeDelivery = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateExtraMileDeliveryLocation(int locationId, bool status)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.IsExtraMileDelivery = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGIGGOLocation(int locationId, bool status)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.IsGIGGO = status;
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
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.IsHomeDelivery = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateLocation(int locationId, PlaceLocationDTO locationDto)
        {
            try
            {
                var state = await GetState(locationDto.StateId);
                var centre = await GetServiceCentre(locationDto.BaseStationId);
                var location = await _uow.PlaceLocation.GetAsync(locationId);

                //To check if the update already exists
                var locations = await _uow.PlaceLocation.ExistAsync(c => c.LocationName.ToLower() == locationDto.LocationName.ToLower() && c.StateId == locationDto.StateId);
                if (location == null || locationDto.LocationId != locationId)
                {
                    throw new GenericException("Location Information does not exist");
                }
                else if (locations == true)
                {
                    throw new GenericException("Location Information already exists");
                }
                location.LocationName = locationDto.LocationName;
                location.StateName = state.StateName;
                location.StateId = state.StateId;
                location.BaseStation = centre.Name;
                location.BaseStationId = centre.ServiceCentreId;

                _uow.Complete();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateNormalHomeDeliveryLocation(int locationId, bool status)
        {
            try
            {
                var location = await _uow.PlaceLocation.GetAsync(locationId);
                if (location == null)
                {
                    throw new GenericException("Location Information does not exist");
                }
                location.IsNormalHomeDelivery = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<State> GetState(int stateId)
        {
            try
            {
                var state = await _uow.State.GetAsync(stateId);
                if (state == null)
                {
                    throw new GenericException("State does not exist");
                }

                return state;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ServiceCentre> GetServiceCentre(int centreId)
        {
            try
            {
                var serviceCentre = await _uow.ServiceCentre.GetAsync(centreId);
                if (serviceCentre == null)
                {
                    throw new GenericException("Service centre does not exist");
                }

                return serviceCentre;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
