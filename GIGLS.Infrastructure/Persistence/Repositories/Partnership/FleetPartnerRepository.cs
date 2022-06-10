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
                               NumberOfTrips = _context.FleetTrip.Where(x => x.FleetId == fleet.FleetId).Count(),
                           };
            var assetDemoDto = new List<AssetDTO>
            {
                new AssetDTO
                {
                               Id = 1,
                               Name = "Ford",
                               RegistrationNumber = "XYZ-001-ABC",
                               NumberOfTrips = 10,
                },
                new AssetDTO
                {
                               Id = 2,
                               Name = "Truck",
                               RegistrationNumber = "XCB-101-APQ",
                               NumberOfTrips = 20,
                },
                new AssetDTO
                {
                               Id = 3,
                               Name = "Toyota",
                               RegistrationNumber = "NAV-221-GHA",
                               NumberOfTrips = 30,
                },
                new AssetDTO
                {
                               Id = 4,
                               Name = "Nissan",
                               RegistrationNumber = "MAB-333-OPC",
                               NumberOfTrips = 40,
                },
            };
            return Task.FromResult(assetDto.ToList());
        }

        //To be completed
        public Task<AssetDetailsDTO> GetFleetAttachedToEnterprisePartnerById(int fleetId)
        {
            var fleets = _context.Fleet.Where(x => x.FleetId == fleetId);

            var assetDto = from fleet in fleets
                           join fleetTrips in _context.FleetTrip on fleet.FleetId equals fleetTrips.FleetId
                           select new AssetDetailsDTO
                           {
                               Id = fleet.FleetId,
                               Name = fleet.FleetName,
                               RegistrationNumber = fleet.RegistrationNumber,
                               Status = fleet.Status ? "Active" : "Idle",
                               NumberOfTrips = _context.FleetTrip.Where(x => x.FleetId == fleet.FleetId).Count(),
                               Captain = _context.Users.Where(x => x.Id == fleet.PartnerId.ToString()).Select(x => new CaptainDTO
                               {
                                   Code = x.UserChannelCode,
                                   Name = $"{x.FirstName} {x.LastName}"
                               }).FirstOrDefault()
                               //Current location of the vehicle
                               //Captain
                               //Fleet Manager assigned to the vehicle
                           };
            var listAssetDetailsDemo = new List<AssetDetailsDTO>
            {
                new AssetDetailsDTO
                {
                               Id = 1,
                               Name = "Ford",
                               RegistrationNumber = "XYZ-001-ABC",
                               NumberOfTrips =10,
                               Status = "Active",
                               Captain =  new CaptainDTO
                               {
                                   Code = "EP0005",
                                   Name = "Isaac Gbade"
                               },
                               Location = "Lagos",
                               FleetManager = "Dele Dada",
                               TotalRevenue = 200000m
                },
                new AssetDetailsDTO
                {
                               Id = 2,
                               Name = "Truck",
                               RegistrationNumber = "XCB-101-APQ",
                               NumberOfTrips = 20,
                               Status = "Idle",
                               Captain =  new CaptainDTO
                               {
                                   Code = "EP0004",
                                   Name = "Gabriel Lamba"
                               },
                               Location = "Oyo",
                               FleetManager = "Tunde Oshomo",
                               TotalRevenue = 100000m
                },
                new AssetDetailsDTO
                {
                               Id = 3,
                               Name = "Toyota",
                               RegistrationNumber = "NAV-221-GHA",
                               NumberOfTrips = 30,
                               Status = "Active",
                               Captain =  new CaptainDTO
                               {
                                   Code = "EP0003",
                                   Name = "Daniel Akpata"
                               },
                               Location = "Edo",
                               FleetManager = "Tunde Oshomo",
                               TotalRevenue = 50000m
                },
                new AssetDetailsDTO
                {
                               Id = 4,
                               Name = "Nissan",
                               RegistrationNumber = "MAB-333-OPC",
                               NumberOfTrips = 40,
                               Status = "Idle",
                               Captain =  new CaptainDTO
                               {
                                   Code = "EP0003",
                                   Name = "Olaide Jamiu"
                               },
                               Location = "Ogun",
                               FleetManager = "Tunde Oshomo",
                               TotalRevenue = 20000m
                },
            };
            var assetDemoDto = listAssetDetailsDemo.Where(x => x.Id == fleetId).FirstOrDefault();
            //return Task.FromResult(assetDemoDto);
            return Task.FromResult(assetDemoDto);
        }

        //Get fleet trips by fleet id
        public Task<List<FleetTripDTO>> GetFleetTrips(int fleetId)
        {
            //To be completed
            //var fleetTrips = _context.FleetTrip.Where(x => x.FleetId == fleetId);

            //var assetTripsDto = from fleetTrip in fleetTrips
            //                    select new FleetTripDTO
            //                    {
            //                        DepartureDestination = fleetTrip.DepartureDestination,
            //                        ActualDestination = fleetTrip.ActualDestination,
            //                        DateCreated = fleetTrip.DateCreated,
            //                        //Get Total amount of trip
            //                        //Get trip status
            //                    };

            var listAssetTripssDemo = new List<FleetTripDTO>
            {
                new FleetTripDTO
                {
                               DepartureDestination = "LOSHUB",
                                    ActualDestination = "Victoria Islandd",
                                    DateCreated = System.DateTime.Now,
                                    
                },
                new FleetTripDTO
                {
                               DepartureDestination = "Iyana Ipaja",
                                    ActualDestination = "Surulere",
                                    DateCreated = System.DateTime.Now.AddDays(-1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Wuse",
                                    ActualDestination = "Gwaripa",
                                    DateCreated = System.DateTime.Now.AddDays(1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Ado Ekiti",
                                    ActualDestination = "Ikole Ekiti",
                                    DateCreated = System.DateTime.Now.AddDays(2),

                },
            };
            return Task.FromResult(listAssetTripssDemo.ToList());
            //return Task.FromResult(assetTripsDto.ToList());
        }

        //Get fleet trips by fleet id
        public Task<List<FleetTripDTO>> GetFleetTripsByPartner(string partnercode)
        {
            //To be completed
            //var fleetTrips = _context.FleetTrip.Where(x => x.FleetId == fleetId);

            //var assetTripsDto = from fleetTrip in fleetTrips
            //                    select new FleetTripDTO
            //                    {
            //                        DepartureDestination = fleetTrip.DepartureDestination,
            //                        ActualDestination = fleetTrip.ActualDestination,
            //                        DateCreated = fleetTrip.DateCreated,
            //                        //Get Total amount of trip
            //                        //Get trip status
            //                    };

            var listAssetTripssDemo = new List<FleetTripDTO>
            {
                new FleetTripDTO
                {
                               DepartureDestination = "LOSHUB",
                                    ActualDestination = "Victoria Islandd",
                                    DateCreated = System.DateTime.Now,

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Iyana Ipaja",
                                    ActualDestination = "Surulere",
                                    DateCreated = System.DateTime.Now.AddDays(-1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Wuse",
                                    ActualDestination = "Gwaripa",
                                    DateCreated = System.DateTime.Now.AddDays(1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Ado Ekiti",
                                    ActualDestination = "Ikole Ekiti",
                                    DateCreated = System.DateTime.Now.AddDays(2),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Abuja",
                                    ActualDestination = "Victoria Islandd",
                                    DateCreated = System.DateTime.Now,

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Lokoja",
                                    ActualDestination = "Surulere",
                                    DateCreated = System.DateTime.Now.AddDays(-1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Wuse",
                                    ActualDestination = "Gwaripa",
                                    DateCreated = System.DateTime.Now.AddDays(1),

                },
                new FleetTripDTO
                {
                               DepartureDestination = "Ado Ekiti",
                                    ActualDestination = "Ikole Ekiti",
                                    DateCreated = System.DateTime.Now.AddDays(2),

                },
            };
            return Task.FromResult(listAssetTripssDemo.ToList());
            //return Task.FromResult(assetTripsDto.ToList());
        }
    }
}
