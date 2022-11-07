using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using POST.Core.Enums;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence.Repository;
using Ninject.Activation;

namespace POST.Infrastructure.Persistence.Repositories.Fleets
{
    public class FleetJobCardRepository : Repository<FleetJobCard, GIGLSContext>, IFleetJobCardRepository
    {
        private GIGLSContext _context;

        public FleetJobCardRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FleetJobCardDto>> GetFleetJobCardsAsync()
        {
            try
            {
                var fleets = _context.FleetJobCard.Include("Fleet").Include("FleetOwner");

                var fleetDto = from x in fleets
                    select new FleetJobCardDto()
                    {
                        FleetJobCardId = x.FleetJobCardId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        Status = x.Status,
                        VehiclePartToFix = x.VehiclePartToFix,
                        FleetManagerId = x.FleetManagerId,
                        Amount = x.Amount,
                        VehicleNumber = x.VehicleNumber,
                        PaymentReceiptUrl = x.PaymentReceiptUrl
                    };
                return await Task.FromResult(fleetDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto)
        {
            try
            {
                //Get start date and end date for database query
                var filterCriteria = new DashboardFilterCriteria { EndDate = dto.EndDate, StartDate = dto.StartDate };
                var filter = filterCriteria.getStartDateAndEndDate();
                dto.StartDate = filter.Item1;
                dto.EndDate = filter.Item2;

                var query = _context.FleetJobCard.AsQueryable();
                var fleetJobCards = new List<FleetJobCardByDateDto>();

                fleetJobCards = await (from x in query 
                    where x.IsDeleted == false 
                          && x.VehicleNumber == dto.VehicleNumber 
                          && x.DateCreated >= dto.StartDate 
                          && x.DateCreated <= dto.EndDate
                          select new FleetJobCardByDateDto()
                          {
                              DateCreated = x.DateCreated,
                              Status = x.Status,
                              FleetJobCardId = x.FleetJobCardId,
                              VehicleNumber = x.VehicleNumber,
                              Amount = x.Amount,
                              VehiclePartToFix = x.VehiclePartToFix,
                          }).OrderByDescending(x => x.DateCreated).ToListAsync();

                return fleetJobCards;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FleetJobCard> GetFleetJobCardByIdAsync(int jobCardId)
        {
            try
            {
                var jobCard = await _context.FleetJobCard.Where(x => x.FleetJobCardId == jobCardId).Include("Fleet").Include("FleetOwner").FirstOrDefaultAsync();
                
                return await Task.FromResult(jobCard);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FleetJobCardByDateDto>> GetFleetJobCardsInCurrentMonthAsync(GetFleetJobCardByDateRangeDto dto)
        {
            try
            {
                var fleetJobCards = new List<FleetJobCardByDateDto>();

                //Get start date and end date for database query
                var filterCriteria = new DashboardFilterCriteria { EndDate = dto.EndDate, StartDate = dto.StartDate };
                var filter = filterCriteria.getStartDateAndEndDate();
                dto.StartDate = filter.Item1;
                dto.EndDate = filter.Item2;

                var query = _context.FleetJobCard.AsQueryable();

                fleetJobCards = await (from x in query
                    where x.IsDeleted == false
                          && x.DateCreated >= dto.StartDate
                          && x.DateCreated <= dto.EndDate
                    select new FleetJobCardByDateDto()
                    {
                        DateCreated = x.DateCreated,
                        Status = x.Status,
                        FleetJobCardId = x.FleetJobCardId,
                        VehicleNumber = x.VehicleNumber,
                        Amount = x.Amount,
                        VehiclePartToFix = x.VehiclePartToFix,
                    }).OrderByDescending(x => x.DateCreated).ToListAsync();
                return fleetJobCards;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
