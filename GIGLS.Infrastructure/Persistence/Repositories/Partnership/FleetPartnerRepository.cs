using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
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
                                 PartnerFirstName =  partner.FirstName,
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
            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var assetDto = from partner in partners
                             join fleet in _context.Fleet on partner.PartnerId equals fleet.PartnerId
                             select new AssetDTO
                             {
                                 Id = fleet.FleetId,
                                 Name = fleet.FleetName,
                                 RegistrationNumber = fleet.RegistrationNumber,
                                 NumberOfTrips = _context.FleetTrip.Where(x => x.FleetId == fleet.FleetId).Count(),
                             };

            return Task.FromResult(assetDto.ToList());
        }

        //Get List of Fleet 
        public Task<AssetDetailsDTO> GetFleetAttachedToEnterprisePartnerByI(int fleetId)
        {
            var fleets = _context.Fleet.Where(x => x.FleetId == fleetId);

            var assetDto = from fleet in fleets
                           join fleetTrips in _context.FleetTrip on fleet.FleetId equals fleetTrips.FleetId
                           select new AssetDetailsDTO
                           {
                               Id = fleet.FleetId,
                               Name = fleet.FleetName,
                               RegistrationNumber = fleet.RegistrationNumber,
                               Status = fleet.Status ? "Active": "Idle",
                               NumberOfTrips = _context.FleetTrip.Where(x => x.FleetId == fleet.FleetId).Count(),
                               Captain = _context.Users.Where(x => x.Id == fleetTrips.CaptainId)
                           };

            return Task.FromResult(assetDto.FirstOrDefault());
        }
    }
}
