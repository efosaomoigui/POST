using GIGLS.Core.Domain.Partnership;
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
                                 PartnerName = partner.PartnerName,
                                 Email = partner.Email,
                                 Address = partner.Address,
                                 PartnerCode = partner.FleetPartnerCode,
                                 PhoneNumber = partner.PhoneNumber,
                                 OptionalPhoneNumber = partner.OptionalPhoneNumber,
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
                                 PartnerPhoneNumber = partner.PhoneNumber
                             };

            return Task.FromResult(partnerDto.ToList());
        }

        //Get total count of fleet
        public async Task<int> FleetCount(string fleetCode)
        {
            var fleetCount = _context.Partners.Where(x => x.FleetPartnerCode == fleetCode).Count();
            return fleetCount;
        }
    }
}
