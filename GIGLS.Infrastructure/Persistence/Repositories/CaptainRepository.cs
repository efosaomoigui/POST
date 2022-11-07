using POST.Core.Domain.Partnership;
using POST.Core.DTO.Captains;
using POST.Core.Enums;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.CORE.DTO.Report;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class CaptainRepository : Repository<Partner, GIGLSContext>, ICaptainRepository
    {
        private GIGLSContext _context;
        private GIGLSContextForView _viewcontext;
        public CaptainRepository(GIGLSContext context, GIGLSContextForView viewcontext) : base(context)
        {
            _context = context;
            _viewcontext = viewcontext;
        }

        public async Task<IReadOnlyList<ViewCaptainsDTO>> GetAllCaptainsByDateAsync(DateTime? date)
        {
            try
            {
                var currentMonth = GetStartAndEndDayOfMonth();

                var captains = _context.Partners.Where(x => 
                    (x.PartnerType == PartnerType.Captain 
                    || x.PartnerType == PartnerType.InternalDeliveryPartner) 
                    && x.IsDeleted == false).AsQueryable();
                var allCaptains = new List<ViewCaptainsDTO>();

                if(date == null)
                {
                    captains = captains.Where(x => x.DateCreated >= currentMonth.FirstDay && x.DateCreated <= currentMonth.LastDay).AsQueryable();
                }
                else
                {
                    captains = captains.Where(x => DbFunctions.DiffDays(x.DateCreated, date) == 0).AsQueryable();
                }

                allCaptains = captains.Select(x => new ViewCaptainsDTO()
                {
                    PartnerId =  x.PartnerId,
                    Status = x.ActivityStatus.ToString(),
                    EmploymentDate = x.DateCreated,
                    CaptainCode = x.PartnerCode,
                    Email = x.Email,
                    Name = x.FirstName + " " + x.LastName,
                    VehicleAssigned = string.IsNullOrEmpty(x.VehicleType + " " + x.VehicleLicenseNumber) ? "No vehicle assigned yet" : x.VehicleType + " " + x.VehicleLicenseNumber
                }).ToList();

                return await Task.FromResult(allCaptains);
            }
            catch (Exception)
            {

                throw;
            }            
        }

        public async Task<Partner> GetCaptainByIdAsync(int partnerId)
        {
            try
            {
                var captain = await _context.Partners.FirstOrDefaultAsync(x => 
                    (x.PartnerType == PartnerType.Captain 
                    || x.PartnerType == PartnerType.InternalDeliveryPartner)
                    && x.PartnerId == partnerId 
                    && x.IsDeleted == false);
                return await Task.FromResult(captain);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<Partner>> GetAllCaptainsAsync()
        {
            try
            {
                var captain = await _context.Partners.Where(x => 
                        (x.PartnerType == PartnerType.Captain 
                        || x.PartnerType == PartnerType.InternalDeliveryPartner)
                        && x.IsDeleted == false)
                    .OrderByDescending(x => x.DateCreated)
                    .ToListAsync();
                return await Task.FromResult(captain);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<VehicleDTO>> GetAllVehiclesAsync()
        {
            try
            {
                var vehicles = _context.Fleet.Where(x => x.IsDeleted == false).Select(x => new VehicleDTO()
                {
                    FleetId = x.FleetId,
                    Status = x.Status == true ? "Active" : "Inactive",
                    AssignedCaptain = _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).FirstName.ToString() + " " + _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).LastName.ToString(),
                    FleetName = x.FleetName,
                    RegistrationNumber = x.RegistrationNumber,
                    VehicleOwner = _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).FirstName.ToString() + " " + _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).LastName.ToString(),
                    VehicleOwnerId = x.EnterprisePartnerId,
                    VehicleAge = (int)DbFunctions.DiffDays(x.DateCreated, DateTime.Now),
                    IsFixed = x.IsFixed.ToString()
                }).OrderByDescending(x => x.FleetId).ToList();

                return await Task.FromResult(vehicles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<VehicleDTO>> GetAllVehiclesByDateAsync(DateTime? date)
        {
            try
            {
                var currentMonth = GetStartAndEndDayOfMonth();

                var vehicles = _context.Fleet.Where(x => x.IsDeleted == false).AsQueryable();
                var allVehicles = new List<VehicleDTO>();

                if (date == null)
                {
                    vehicles = vehicles.Where(x => x.DateCreated >= currentMonth.FirstDay && x.DateCreated <= currentMonth.LastDay).AsQueryable();
                }
                else
                {
                    vehicles = vehicles.Where(x => DbFunctions.DiffDays(x.DateCreated, date) == 0).AsQueryable();
                }

                allVehicles = vehicles.Select(x => new VehicleDTO()
                {
                    FleetId = x.FleetId,
                    Status = x.Status == true ? "Active" : "Inactive",
                    AssignedCaptain = _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).FirstName.ToString() + " " + _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).LastName.ToString(),
                     //x.PartnerId.ToString(),
                    FleetName = x.FleetName,
                    RegistrationNumber = x.RegistrationNumber,
                    VehicleOwner = _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).FirstName.ToString() + " " + _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).LastName.ToString() ,
                    VehicleOwnerId = x.EnterprisePartnerId,
                    VehicleAge = (int)DbFunctions.DiffDays(x.DateCreated, DateTime.Now),
                    IsFixed = x.IsFixed.ToString()
                }).ToList();

                return await Task.FromResult(allVehicles);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<VehicleDetailsDTO> GetVehicleByRegistrationNumberAsync(string regNum)
        {
            try
            {
                var currentMonth = GetStartAndEndDayOfMonth();

                var vehicle = await _context.Fleet.Where(x => x.IsDeleted == false && x.RegistrationNumber == regNum).FirstOrDefaultAsync();
                if (vehicle == null)
                {
                    throw new GenericException($"Vehicle with registration number: {regNum} does not exist");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == vehicle.EnterprisePartnerId);
                var partner = await _context.Partners.FirstOrDefaultAsync(p => p.PartnerId == vehicle.PartnerId);

                var vehicleDto = new VehicleDetailsDTO()
                {
                    FleetId = vehicle.FleetId,
                    Status = vehicle.Status == true ? "Active" : "Inactive",
                    AssignedCaptain = partner.PartnerName,
                    FleetName = vehicle.FleetName,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    VehicleOwner = user != null ? $"{user.FirstName} {user.LastName}" : "Null",
                    VehicleOwnerId = vehicle.EnterprisePartnerId,
                    VehicleAge = (int)( DateTime.Now - vehicle.DateCreated ).TotalDays,
                    Capacity = vehicle.Capacity,
                    VehicleType = vehicle.FleetType.ToString(),
                    PartnerId = vehicle.PartnerId,
                    IsFixed = vehicle.IsFixed.ToString(),
                };

                return vehicleDto;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<List<VehicleDTO>> GetAllVehiclesByDateRangeAsync(DateFilterCriteria filter)
        {
            //get startDate and endDate
            var startDate = new DateTime();
            if (filter.StartDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            var queryDate = filter.getStartDateAndEndDate();
            var endDate = queryDate.Item2;

            // filter by cancelled shipments
            var vehicles = _context.Fleet.AsQueryable().Where(x => x.IsDeleted == false);
            vehicles = vehicles.Where(x => x.DateCreated >= startDate && x.DateCreated <= endDate)
                .OrderByDescending(x => x.DateCreated);

            var resultDto = vehicles.Select(x => new VehicleDTO()
            {
                FleetId = x.FleetId,
                Status = x.Status == true ? "Active" : "Inactive",
                AssignedCaptain = _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).FirstName.ToString() + " " + _context.Partners.FirstOrDefault(p => p.PartnerId == x.PartnerId).LastName.ToString(),
                FleetName = x.FleetName,
                RegistrationNumber = x.RegistrationNumber,
                VehicleOwner = _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).FirstName.ToString() + " " + _context.Users.FirstOrDefault(user => user.Id == x.EnterprisePartnerId).LastName.ToString(),
                VehicleOwnerId = x.EnterprisePartnerId,
                VehicleAge = (int)DbFunctions.DiffDays(x.DateCreated, DateTime.Now),
                IsFixed = x.IsFixed.ToString()
            }).ToList();

            return Task.FromResult(resultDto);
        }

        public Task<List<ViewCaptainsDTO>> GetAllCaptainsByDateRangeAsync(DateFilterCriteria filter)
        {
            //get startDate and endDate
            var startDate = new DateTime();
            DateTime endDate;
            var queryDate = filter.getStartDateAndEndDate();
            if (filter.StartDate == null && filter.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate = queryDate.Item2;
            }
            else
            {
                startDate = queryDate.Item1;
                endDate = queryDate.Item2;
            }
            
            
            var allCaptains = new List<ViewCaptainsDTO>();

            // filter by cancelled shipments
            var captains = _context.Partners.AsQueryable().Where(x => x.IsDeleted == false);
            captains = captains.Where(x => x.DateCreated >= startDate && x.DateCreated <= endDate)
                .OrderByDescending(x => x.DateCreated);

            allCaptains = captains.Select(x => new ViewCaptainsDTO()
            {
                PartnerId = x.PartnerId,
                Status = x.ActivityStatus.ToString(),
                EmploymentDate = x.DateCreated,
                CaptainCode = x.PartnerCode,
                Email = x.Email,
                Name = x.FirstName + " " + x.LastName,
                VehicleAssigned = string.IsNullOrEmpty(x.VehicleType + " " + x.VehicleLicenseNumber) ? "No vehicle assigned yet" : x.VehicleType + " " + x.VehicleLicenseNumber
            }).ToList();

            return Task.FromResult(allCaptains);
        }

        private CurrentMonthDetailsDTO GetStartAndEndDayOfMonth()
        {
            DateTime today = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return new CurrentMonthDetailsDTO { FirstDay = firstDayOfMonth, LastDay = lastDayOfMonth};
        }
    }
}
