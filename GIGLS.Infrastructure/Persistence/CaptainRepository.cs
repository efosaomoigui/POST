using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Captains;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence
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
                DateTime today = DateTime.Now.Date;
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var captains = _context.Partners.Where(x => x.PartnerType == PartnerType.Captain && x.IsDeleted == false).AsQueryable();
                var allCaptains = new List<ViewCaptainsDTO>();

                if(date == null)
                {
                    captains = captains.Where(x => x.DateCreated >= firstDayOfMonth && x.DateCreated <= lastDayOfMonth).AsQueryable();
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
                var captain = await _context.Partners.FirstOrDefaultAsync(x => x.PartnerType == PartnerType.Captain && x.PartnerId == partnerId && x.IsDeleted == false);
                return await Task.FromResult(captain);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
