using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using Ninject.Activation;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
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

                var fleetDto = from x in fleets where x.Status == FleetJobCardStatus.Open.ToString()
                    select new FleetJobCardDto()
                    {
                        FleetJobCardId = x.FleetJobCardId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        Status = x.Status,
                        VehiclePartToFix = x.VehiclePartToFix,
                        FleetManagerId = x.FleetManagerId,
                        Amount = x.Amount,
                        VehicleNumber = x.VehicleNumber
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

                if (dto.IsAdmin)
                {
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

                fleetJobCards = await (from x in query 
                    where x.IsDeleted == false 
                          && x.FleetManagerId == dto.FleetManagerId 
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

        public async Task<List<FleetJobCardDto>> GetFleetJobCardsByFleetManagerAsync(string fleetManagerId)
        {
            try
            {
                var fleets = _context.FleetJobCard.Include("Fleet").Include("FleetOwner");

                var fleetDto = from x in fleets
                    where x.Status == FleetJobCardStatus.Open.ToString() && x.IsDeleted == false
                    select new FleetJobCardDto()
                    {
                        FleetJobCardId = x.FleetJobCardId,
                        DateCreated = x.DateCreated,
                        DateModified = x.DateModified,
                        Status = x.Status,
                        VehiclePartToFix = x.VehiclePartToFix,
                        FleetManagerId = x.FleetManagerId,
                        Amount = x.Amount,
                        VehicleNumber = x.VehicleNumber
                    };
                if(fleetManagerId == "Admin")
                {
                    return await Task.FromResult(fleetDto.OrderByDescending(x => x.DateCreated).ToList());
                }
                return await Task.FromResult(fleetDto.Where(x => x.FleetManagerId == fleetManagerId).OrderByDescending(x => x.DateCreated).ToList());
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

        public async Task<List<FleetJobCardByDateDto>> GetFleetJobCardsByFleetManagerInCurrentMonthAsync(GetFleetJobCardByDateRangeDto dto)
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

                if (dto.IsAdmin)
                {
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

                fleetJobCards = await (from x in query
                    where x.IsDeleted == false
                          && x.FleetManagerId == dto.FleetManagerId
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
