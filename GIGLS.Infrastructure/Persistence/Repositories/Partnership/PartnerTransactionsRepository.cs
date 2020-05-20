using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
            //Excluding It Test
            string excludeUserList = ConfigurationManager.AppSettings["excludeUserList"];
            string[] testUserId = excludeUserList.Split(',').ToArray();

            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            if (filterCriteria.StartDate == null & filterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }
           
            var partnersTrans = Context.PartnerTransactions.AsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate
                                && !testUserId.Contains(s.UserId));

            List<PartnerTransactionsDTO> partnerTransDTO = (from r in partnersTrans
                                                            select new PartnerTransactionsDTO()
                                                            {
                                                                Waybill = r.Waybill,
                                                                AmountReceived = r.AmountReceived,

                                                            }).ToList();

            return await Task.FromResult(partnerTransDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<List<PartnerTransactionsDTO>> GetPartnerTransactionByUser(string userId)
        {
            var partnersTrans = Context.PartnerTransactions.AsQueryable().Where(s => s.UserId == userId);

            List<PartnerTransactionsDTO> partnerTransDTO = (from r in partnersTrans
                                                            select new PartnerTransactionsDTO()
                                                            {
                                                                Waybill = r.Waybill,
                                                                AmountReceived = r.AmountReceived,
                                                                Destination = r.Destination,
                                                                Departure = r.Departure,
                                                                DateCreated = r.DateCreated,
                                                                DateModified = r.DateModified,
                                                                MobileGroupCode = _context.MobileGroupCodeWaybillMapping.Where(x => x.WaybillNumber == r.Waybill).
                                                                                    Select(d => d.GroupCodeNumber).FirstOrDefault()
                                                            }).ToList();

            return await Task.FromResult(partnerTransDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        //Five Recent Transactions
        public async Task<List<FleetPartnerTransactionsDTO>> GetRecentFivePartnerTransactionsForFleet(string fleetPartnerCode)
        {
            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var partnerDto = (from partner in partners
                              join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                              join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                              select new FleetPartnerTransactionsDTO
                              {
                                  AmountReceived = transaction.AmountReceived,
                                  Departure = transaction.Departure,
                                  Destination = transaction.Destination,
                                  Waybill = transaction.Waybill,
                                  DateCreated = transaction.DateCreated,
                                  FirstName = partner.FirstName,
                                  LastName = partner.LastName,
                                  PhoneNumber = partner.PhoneNumber,
                                  CurrencySymbol = country.CurrencySymbol,
                                  PreShipment = _context.PresShipmentMobile.Where(c => c.Waybill == transaction.Waybill)
                                  .Select(x => new PreShipmentMobileDTO
                                  {
                                      SenderStationName = _context.Station.Where(y => y.StationId == x.SenderStationId).Select(y => y.StationName).FirstOrDefault(),
                                      ReceiverStationName = _context.Station.Where(y => y.StationId == x.ReceiverStationId).Select(y => y.StationName).FirstOrDefault(),
                                      PreShipmentItems = _context.PresShipmentItemMobile.Where(d => d.PreShipmentMobileId == x.PreShipmentMobileId)
                                      .Select(y => new PreShipmentItemMobileDTO
                                      {
                                          ItemName = y.ItemName
                                      }).ToList()
                                  }).FirstOrDefault()
                              }).OrderByDescending(s => s.DateCreated).Take(5);
                             
            return await Task.FromResult(partnerDto.ToList());
        }

        //All Transactions, query by date
        public async Task<List<FleetPartnerTransactionsDTO>> GetPartnerTransactionsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            var partnerDto = from partner in partners
                              join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                              join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                              where transaction.DateCreated >= startDate && transaction.DateCreated < endDate
                              select new FleetPartnerTransactionsDTO
                              {
                                  AmountReceived = transaction.AmountReceived,
                                  Departure = transaction.Departure,
                                  Destination = transaction.Destination,
                                  Waybill = transaction.Waybill,
                                  DateCreated = transaction.DateCreated,
                                  FirstName = partner.FirstName,
                                  LastName = partner.LastName,
                                  PhoneNumber = partner.PhoneNumber,
                                  CurrencySymbol = country.CurrencySymbol,
                                  PreShipment = _context.PresShipmentMobile.Where(c => c.Waybill == transaction.Waybill)
                                  .Select(x => new PreShipmentMobileDTO
                                  {
                                      SenderStationName = _context.Station.Where(y => y.StationId == x.SenderStationId).Select(y => y.StationName).FirstOrDefault(),
                                      ReceiverStationName = _context.Station.Where(y => y.StationId == x.ReceiverStationId).Select(y => y.StationName).FirstOrDefault(),
                                      PreShipmentItems = _context.PresShipmentItemMobile.Where(y => y.PreShipmentMobileId == x.PreShipmentMobileId)
                                                            .Select(d => new PreShipmentItemMobileDTO
                                                            {
                                                                ItemName = d.ItemName
                                                            }).ToList()
                                  }).FirstOrDefault()
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

        //Earnings, query by date
        public async Task<List<object>> GetPartnerEarningsForFleet(ShipmentCollectionFilterCriteria filterCriteria, string fleetPartnerCode)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var partners = _context.Partners.Where(s => s.FleetPartnerCode == fleetPartnerCode);

            IEnumerable<object> earningsDto = (from partner in partners
                                                join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                                                join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                                                where transaction.DateCreated >= startDate && transaction.DateCreated < endDate
                                                group transaction by partner.PartnerName into g
                                                select new 
                                                {
                                                    PartnerName = g.Key,
                                                    Earnings = g.Sum(a => a.AmountReceived),
                                                    Trips = g.Count(),
                                                }).OrderByDescending(i => i.Earnings).ToList();

            var earnings = earningsDto.ToList();
            return await Task.FromResult(earnings);
        }
        
        private Task<IQueryable<ExternalPartnerTransactionsPaymentCacheDTO>> GetExternalPartnerTransactions(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var partners = _context.Partners.AsQueryable().Where(s => s.PartnerType == Core.Enums.PartnerType.DeliveryPartner && s.IsActivated == true);

            var partnerDto = from partner in partners
                             join transaction in _context.PartnerTransactions on partner.UserId equals transaction.UserId
                             join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                             where transaction.DateCreated >= startDate && transaction.DateCreated < endDate && transaction.IsProcessed == false
                             select new ExternalPartnerTransactionsPaymentCacheDTO
                             {
                                 FirstName = partner.FirstName,
                                 LastName = partner.LastName,
                                 PartnerCode = partner.PartnerCode,
                                 EnterprisePartner = _context.FleetPartner.Where(x => x.FleetPartnerCode == partner.FleetPartnerCode)
                                                    .Select(d => d.FirstName + " " + d.LastName).FirstOrDefault(),
                                 CurrencySymbol = country.CurrencySymbol,
                                 Email = partner.Email,
                                 AmountReceived = transaction.AmountReceived,
                                 Trips = transaction.PartnerTransactionsID,
                                 BankName = partner.BankName,
                                 AccountName = partner.AccountName,
                                 AccountNumber = partner.AccountNumber
                             };

            return Task.FromResult(partnerDto.AsQueryable().AsNoTracking());
        }

        public async Task<List<ExternalPartnerTransactionsPaymentDTO>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var partners = await GetExternalPartnerTransactions(filterCriteria);

            var partnerDto = from partner in partners
                             group partner by partner.PartnerCode into p
                             select new ExternalPartnerTransactionsPaymentDTO
                             {
                                 Code = p.Key,
                                 CurrencySymbol = p.FirstOrDefault().CurrencySymbol,
                                 Email = p.FirstOrDefault().Email,
                                 Amount = p.Sum(a => a.AmountReceived),
                                 Trips = p.Count(),
                                 EnterprisePartner = p.FirstOrDefault().EnterprisePartner,
                                 FirstName = p.FirstOrDefault().FirstName,
                                 LastName = p.FirstOrDefault().LastName,
                                 BankName = p.FirstOrDefault().BankName,
                                 AccountName = p.FirstOrDefault().AccountName,
                                 AccountNumber = p.FirstOrDefault().AccountNumber
                             };            
            return await partnerDto.OrderByDescending(s => s.Amount).AsNoTracking().ToListAsync();
        }

        
    }
}
