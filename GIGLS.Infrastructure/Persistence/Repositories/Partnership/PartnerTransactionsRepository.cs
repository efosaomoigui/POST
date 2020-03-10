using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IRepositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
{
    public class PartnerTransactionsRepository : Repository<PartnerTransactions, GIGLSContext>, IPartnerTransactionsRepository
    {
        private GIGLSContext _context;
        public PartnerTransactionsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByDate(BaseFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            if (filterCriteria.StartDate == null & filterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            //Excluding It Test
            string[] testUserId = { "2932eb15-aa30-462c-89f0-7247670f504b", "ab3722d7-57f3-4e6e-a32d-1580315b7da6", "e67d50c2-953a-44b2-bbcd-c38fadef237f" };
           
            var partnersTrans = Context.PartnerTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate
                                && !testUserId.Contains(s.UserId)).AsQueryable();

            List<PartnerTransactionsDTO> partnerTransDTO = (from r in partnersTrans
                                                            select new PartnerTransactionsDTO()
                                                            {
                                                                Waybill = r.Waybill,
                                                                AmountReceived = r.AmountReceived,

                                                            }).ToList();

            return await Task.FromResult(partnerTransDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
        
        //Five Recent Transactions
        public async Task<List<PartnerTransactionsDTO>> GetRecentFivePartnerTransactionsForFleet(string fleetPartnerCode)
        {
            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var partnerDto = (from partner in partners
                              join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                              join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                              select new PartnerTransactionsDTO
                              {
                                  AmountReceived = transaction.AmountReceived,
                                  Departure = transaction.Departure,
                                  Destination = transaction.Destination,
                                  Waybill = transaction.Waybill,
                                  DateCreated = transaction.DateCreated,
                                  FirstName = partner.FirstName,
                                  LastName = partner.LastName,
                                  PhoneNumber = partner.PhoneNumber,
                                  CurrencySymbol = country.CurrencySymbol
                              }).OrderByDescending(s => s.DateCreated).Take(5);
                             
            return await Task.FromResult(partnerDto.ToList());
        }

        //All Transactions, query by date
        public async Task<List<PartnerTransactionsDTO>> GetPartnerTransactionsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var partnerDto = from partner in partners
                              join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                              join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                              where transaction.DateCreated >= startDate && transaction.DateCreated < endDate
                              select new PartnerTransactionsDTO
                              {
                                  AmountReceived = transaction.AmountReceived,
                                  Departure = transaction.Departure,
                                  Destination = transaction.Destination,
                                  Waybill = transaction.Waybill,
                                  DateCreated = transaction.DateCreated,
                                  FirstName = partner.FirstName,
                                  LastName = partner.LastName,
                                  PhoneNumber = partner.PhoneNumber,
                                  CurrencySymbol = country.CurrencySymbol
                              };


            return await Task.FromResult(partnerDto.OrderByDescending(s => s.DateCreated).ToList());
        }

        //Get Number of Total Transactions of Fleet
        public async Task<int> GetTotalTransactionsOfFleet(string fleetPartnerCode)
        {                     
            var partnersAttached = Context.Partners.Where(x => x.FleetPartnerCode == fleetPartnerCode).Select(x => x.UserId);

            var partnersTrans = Context.PartnerTransactions.Where(s => partnersAttached.Contains(s.UserId)).Count();
                        
            return await Task.FromResult(partnersTrans);
        }

    }
}
