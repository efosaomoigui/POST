using POST.Core.Domain.Partnership;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.Partnership;
using POST.Core.IRepositories.Partnership;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.Partnership
{
    public class FleetPartnerRepository : Repository<FleetPartner, GIGLSContext>, IFleetPartnerRepository
    {
        private GIGLSContext _context;
        public FleetPartnerRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetPartnerDTO>> GetFleetPartnersAsync()
        {
            var partners = _context.FleetPartner;

            var partnerDto = from partner in partners
                             select new FleetPartnerDTO
                             {
                                 FleetPartnerId = partner.FleetPartnerId,
                                 Email = partner.Email,
                                 Address = partner.Address,
                                 FleetPartnerCode = partner.FleetPartnerCode,
                                 PhoneNumber = partner.PhoneNumber,
                                 FirstName = partner.FirstName,
                                 LastName = partner.LastName,
                                 UserId = partner.UserId,
                             };

            return Task.FromResult(partnerDto.ToList());
        }

        //Get List of Vehicles
        public Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner(string fleetPartnerCode)
        {
            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var partnerDto = from partner in partners
                             join vehicle in _context.VehicleType on partner.PartnerCode equals vehicle.Partnercode
                             select new VehicleTypeDTO
                             {
                                 Vehicletype = vehicle.Vehicletype,
                                 VehiclePlateNumber = vehicle.VehiclePlateNumber,
                                 Partnercode = vehicle.Partnercode,
                                 IsVerified = vehicle.IsVerified,
                                 PartnerName = partner.FirstName + " " + partner.LastName,
                                 PartnerPhoneNumber = partner.PhoneNumber,
                                 PartnerFirstName = partner.FirstName,
                                 PartnerLastName = partner.LastName,
                                 EnterprisePartner = _context.FleetPartner.Where(c => c.FleetPartnerCode == fleetPartnerCode).Select(x => new FleetPartnerDTO
                                 {
                                     FirstName = x.FirstName,
                                     LastName = x.LastName
                                 }).FirstOrDefault()

                             };

            return Task.FromResult(partnerDto.ToList());
        }

        //Get total count of fleet
        public async Task<int> FleetCount(string fleetCode)
        {
            var fleetCount = _context.Partners.Where(x => x.FleetPartnerCode == fleetCode).Count();
            return await Task.FromResult(fleetCount);
        }


        public Task<List<PartnerDTO>> GetExternalPartnersNotAttachedToAnyFleetPartner()
        {
            var partners = _context.Partners.AsQueryable().Where(s => s.FleetPartnerCode == null && s.PartnerType == Core.Enums.PartnerType.DeliveryPartner && s.IsActivated == true);

            var partnerDto = from partner in partners
                             select new PartnerDTO
                             {
                                 PartnerName = partner.PartnerName,
                                 FirstName = partner.FirstName,
                                 LastName = partner.LastName,
                                 PartnerCode = partner.PartnerCode
                             };

            return Task.FromResult(partnerDto.ToList());
        }

        //Get List of Fleet 
        public Task<List<AssetDTO>> GetFleetAttachedToEnterprisePartner(string fleetPartnerCode)
        {
            var partners = _context.Users.Where(s => s.UserChannelCode == fleetPartnerCode);

            var assetDto = from partner in partners
                           join fleet in _context.Fleet on partner.Id equals fleet.EnterprisePartnerId
                           select new AssetDTO
                           {
                               Id = fleet.FleetId,
                               Name = fleet.FleetName,
                               RegistrationNumber = fleet.RegistrationNumber,
                               NumberOfTrips = _context.FleetTrip.Where(x => x.FleetRegistrationNumber.ToLower() == fleet.RegistrationNumber.ToLower()).Count(),
                           };
            return Task.FromResult(assetDto.ToList());
        }

        //To be completed
        public Task<AssetDetailsDTO> GetFleetAttachedToEnterprisePartnerById(int  fleetId)
        {
            var fleets = _context.Fleet.Where(x => x.FleetId == fleetId);

            var assetDto = from fleet in fleets
                           select new AssetDetailsDTO
                           {
                               Id = fleet.FleetId,
                               Name = fleet.FleetName,
                               RegistrationNumber = fleet.RegistrationNumber,
                               Status = fleet.Status ? "Active" : "Idle",
                               NumberOfTrips = _context.FleetTrip.Where(x => x.FleetRegistrationNumber == fleet.RegistrationNumber).Count(),
                               Captain = _context.Partners.Where(x => x.PartnerId == fleet.PartnerId).Select(x => new CaptainDTO
                               {
                                   Code = x.PartnerCode,
                                   FirstName = x.FirstName,
                                   LastName = x.LastName
                               }).FirstOrDefault()
                               //Current location of the vehicle
                               //Captain
                               //Fleet Manager assigned to the vehicle
                           };
            return Task.FromResult(assetDto.FirstOrDefault());
        }

        //Get fleet trips by fleet id
        public Task<List<AssetTripDTO>> GetFleetTrips(int fleetId)
        {
            //To be completed
            var fleetTrips = _context.FleetTrip;

            var assetTripsDto = from fleetTrip in fleetTrips
                                join departStation in _context.Station on fleetTrip.DepartureStationId equals departStation.StationId
                                join destStation in _context.Station on fleetTrip.DestinationServiceCenterId equals destStation.StationId
                                where fleetTrip.FleetId == fleetId
                                select new AssetTripDTO
                                {
                                    DateCreated = fleetTrip.DateCreated,
                                    TripAmount = fleetTrip.TripAmount,
                                    Status = fleetTrip.Status,
                                    DepartureCity = departStation.StationName,
                                    DestinationCity = destStation.StationName
                                };

            return Task.FromResult(assetTripsDto.ToList());
        }

        //Get fleet trips by fleet id
        public Task<List<AssetTripDTO>> GetFleetTripsByPartner(string partnercode)
        {
            //To be completed
            var assetTripsDto = from fleetTrip in _context.FleetTrip
                                join departStation in _context.Station on fleetTrip.DepartureStationId equals departStation.StationId
                                join destStation in _context.Station on fleetTrip.DestinationServiceCenterId equals destStation.StationId
                                join user in _context.Users on partnercode equals user.UserChannelCode
                                join fleet in _context.Fleet on user.Id equals fleet.EnterprisePartnerId
                                where fleet.FleetId == fleetTrip.FleetId
                                select new AssetTripDTO
                                {
                                    DateCreated = fleetTrip.DateCreated,
                                    TripAmount = fleetTrip.TripAmount,
                                    Status = fleetTrip.Status,
                                    DepartureCity = departStation.StationName,
                                    DestinationCity = destStation.StationName
                                };

            return Task.FromResult(assetTripsDto.ToList());
        }
    }
}
