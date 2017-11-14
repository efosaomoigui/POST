using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Linq;
using System.Collections.Generic;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetTripService : IFleetTripService
    {
        private readonly IUnitOfWork _uow;
        public FleetTripService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddFleetTrip(FleetTripDTO trip)
        {
            try
            {
                var newTrip = new FleetTrip
                {
                    ActualDestination = trip.ActualDestination,
                    DepartureDestination = trip.DepartureDestination,
                    ExpectedDestination = trip.ExpectedDestination,
                    ArrivalTime = trip.ArrivalTime,
                    CaptainId = trip.CaptainId, //captain detail later
                    DepartureTime = trip.DepartureTime,
                    DistanceTravelled = trip.DistanceTravelled,
                    FuelCosts = trip.FuelCosts,
                    FuelUsed = trip.FuelUsed,
                    FleetId = trip.FleetId
                };
                _uow.FleetTrip.Add(newTrip);
                await _uow.CompleteAsync();
                return new { Id = newTrip.FleetTripId};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFleetTrip(int tripId)
        {
            try
            {
                var trip = await _uow.FleetTrip.GetAsync(tripId);
                if (trip == null)
                {
                    throw new GenericException("TRIP_INFORMATION_DOES_NOT_EXIST");
                }
                _uow.FleetTrip.Remove(trip);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetTripDTO> GetFleetTripById(int tripId)
        {
            try
            {
                var trip = await _uow.FleetTrip.GetAsync(tripId);
                if (trip == null)
                {
                    throw new GenericException("TRIP_INFORMATION_DOES_NOT_EXIST");
                }
                return new FleetTripDTO
                {
                    ActualDestination = trip.ActualDestination,
                    DepartureDestination = trip.DepartureDestination,
                    ExpectedDestination = trip.ExpectedDestination,
                    ArrivalTime = trip.ArrivalTime,
                    CaptainName = trip.Captain.FirstName + " " + trip.Captain.LastName,
                    CaptainId = trip.CaptainId,
                    DepartureTime = trip.DepartureTime,
                    DistanceTravelled = trip.DistanceTravelled,
                    FuelCosts = trip.FuelCosts,
                    FuelUsed = trip.FuelUsed,
                    FleetId = trip.FleetId,
                    FleetRegistrationNumber = trip.Fleet.RegistrationNumber                    
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<FleetTripDTO>> GetFleetTrips()
        {
            try
            {
                List<FleetTripDTO> trips = _uow.FleetTrip.GetAll().Select(trip => new FleetTripDTO
                {
                    ActualDestination = trip.ActualDestination,
                    DepartureDestination = trip.DepartureDestination,
                    ExpectedDestination = trip.ExpectedDestination,
                    ArrivalTime = trip.ArrivalTime,
                    CaptainName = trip.Captain.FirstName + " " + trip.Captain.LastName,
                    CaptainId = trip.CaptainId,
                    DepartureTime = trip.DepartureTime,
                    DistanceTravelled = trip.DistanceTravelled,
                    FuelCosts = trip.FuelCosts,
                    FuelUsed = trip.FuelUsed,
                    FleetId = trip.FleetId,
                    FleetRegistrationNumber = trip.Fleet.RegistrationNumber
                }).ToList();
                return Task.FromResult(trips);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateFleetTrip(int tripId, FleetTripDTO tripDto)
        {
            try
            {
                var trip = await _uow.FleetTrip.GetAsync(tripId);
                if (trip == null || tripDto.FleetTripId != tripId)
                {
                    throw new GenericException("TRIP_INFORMATION_DOES_NOT_EXIST");
                }

                trip.ActualDestination = tripDto.ActualDestination;
                trip.DepartureDestination = tripDto.DepartureDestination;
                trip.ExpectedDestination = tripDto.ExpectedDestination;
                trip.ArrivalTime = tripDto.ArrivalTime;
                trip.CaptainId = tripDto.CaptainId;
                trip.DepartureTime = tripDto.DepartureTime;
                trip.DistanceTravelled = tripDto.DistanceTravelled;
                trip.FuelCosts = tripDto.FuelCosts;
                trip.FuelUsed = tripDto.FuelUsed;
                trip.FleetId = tripDto.FleetId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
