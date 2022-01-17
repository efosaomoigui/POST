using AutoMapper;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletTransactionRepository : Repository<WalletTransaction, GIGLSContext>, IWalletTransactionRepository
    {
        private GIGLSContext _context;

        public WalletTransactionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionAsync(int[] serviceCentreIds)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var walletTransactions = walletTransactionContext.Include(s => s.ServiceCentre).ToList();
            var walletTransactionDTO = Mapper.Map<IEnumerable<WalletTransactionDTO>>(walletTransactions);
            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionDateAsync(int[] serviceCentreIds, ShipmentCollectionFilterCriteria dateFilter)
        {
            //get startDate and endDate
            var queryDate = dateFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var walletTransactionContext = _context.WalletTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = walletTransactionContext.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            //var walletTransactions = walletTransactionContext.Include(s => s.ServiceCentre).ToList();        
            //var walletTransactionDTO = Mapper.Map<IEnumerable<WalletTransactionDTO>>(walletTransactions);

            List<WalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                               select new WalletTransactionDTO()
                                                               {
                                                                   WalletTransactionId = w.WalletTransactionId,
                                                                   DateOfEntry = w.DateOfEntry,
                                                                   Amount = w.Amount,
                                                                   CreditDebitType = w.CreditDebitType,
                                                                   Description = w.Description,
                                                                   IsDeferred = w.IsDeferred,
                                                                   PaymentType = w.PaymentType,
                                                                   UserId = w.UserId,
                                                                   ServiceCentreId = w.ServiceCentreId,
                                                                   ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                                   {
                                                                       Code = x.Code,
                                                                       Name = x.Name
                                                                   }).FirstOrDefault(),
                                                                   WalletId = w.WalletId,
                                                                   Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                                                   {
                                                                       Balance = x.Balance,
                                                                       CompanyType = x.CompanyType,
                                                                       CustomerCode = x.CustomerCode,
                                                                       CustomerId = x.CustomerId,
                                                                       CustomerType = x.CustomerType,
                                                                       WalletNumber = x.WalletNumber,
                                                                   }).FirstOrDefault()
                                                               }).OrderByDescending(s => s.DateOfEntry).ToList();


            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionCreditAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.Where(x => x.CreditDebitType == CreditDebitType.Credit).AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            walletTransactionContext = walletTransactionContext.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<WalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                               select new WalletTransactionDTO()
                                                               {
                                                                   WalletTransactionId = w.WalletTransactionId,
                                                                   DateOfEntry = w.DateOfEntry,
                                                                   Amount = w.Amount,
                                                                   CreditDebitType = w.CreditDebitType,
                                                                   Description = w.Description,
                                                                   IsDeferred = w.IsDeferred,
                                                                   PaymentType = w.PaymentType,
                                                                   PaymentTypeReference = w.PaymentTypeReference,
                                                                   UserId = w.UserId,
                                                                   ServiceCentreId = w.ServiceCentreId,
                                                                   ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                                   {
                                                                       Code = x.Code,
                                                                       Name = x.Name
                                                                   }).FirstOrDefault(),
                                                                   WalletId = w.WalletId,
                                                                   Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                                                   {
                                                                       Balance = x.Balance,
                                                                       CompanyType = x.CompanyType,
                                                                       CustomerCode = x.CustomerCode,
                                                                       CustomerId = x.CustomerId,
                                                                       CustomerType = x.CustomerType,
                                                                       WalletNumber = x.WalletNumber,
                                                                       //CustomerName = Context.Company.Where(s => s.CustomerCode == x.CustomerCode).FirstOrDefault().Name
                                                                   }).FirstOrDefault()
                                                               }).OrderByDescending(s => s.DateOfEntry).ToList();

            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionCreditOrDebitAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.Where(x => x.CreditDebitType == accountFilterCriteria.creditDebitType).AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var OneMonthAgo = DateTime.Now.AddMonths(0);  //One (1) Months ago
                startDate = new DateTime(OneMonthAgo.Year, OneMonthAgo.Month, 1);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                var queryDate = accountFilterCriteria.getStartDateAndEndDate();
                startDate = queryDate.Item1;
                endDate = queryDate.Item2;
            }



            walletTransactionContext = walletTransactionContext.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<WalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                               select new WalletTransactionDTO()
                                                               {
                                                                   WalletTransactionId = w.WalletTransactionId,
                                                                   DateOfEntry = w.DateOfEntry,
                                                                   Amount = w.Amount,
                                                                   CreditDebitType = w.CreditDebitType,
                                                                   Description = w.Description,
                                                                   IsDeferred = w.IsDeferred,
                                                                   PaymentType = w.PaymentType,
                                                                   PaymentTypeReference = w.PaymentTypeReference,
                                                                   UserId = w.UserId,
                                                                   ServiceCentreId = w.ServiceCentreId,
                                                                   ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                                   {
                                                                       Code = x.Code,
                                                                       Name = x.Name
                                                                   }).FirstOrDefault(),
                                                                   WalletId = w.WalletId,
                                                                   Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                                                   {
                                                                       Balance = x.Balance,
                                                                       CompanyType = x.CompanyType,
                                                                       CustomerCode = x.CustomerCode,
                                                                       CustomerId = x.CustomerId,
                                                                       CustomerType = x.CustomerType,
                                                                       WalletNumber = x.WalletNumber,
                                                                       //CustomerName = Context.Company.Where(s => s.CustomerCode == x.CustomerCode).FirstOrDefault().Name
                                                                   }).FirstOrDefault()
                                                               }).OrderByDescending(s => s.DateOfEntry).ToList();

            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<ModifiedWalletTransactionDTO>> GetWalletTransactionMobile(int walletId, ShipmentCollectionFilterCriteria filterCriteria)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.Where(x => x.WalletId == walletId).AsQueryable();

            if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
            {
                walletTransactionContext = walletTransactionContext.OrderByDescending(x => x.DateCreated).Take(20);
            }
            else
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                walletTransactionContext = walletTransactionContext.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);
            }

            List<ModifiedWalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                                       select new ModifiedWalletTransactionDTO()
                                                                       {
                                                                           WalletTransactionId = w.WalletTransactionId,
                                                                           Waybill = w.Waybill,
                                                                           DateOfEntry = w.DateOfEntry,
                                                                           Amount = w.Amount,
                                                                           CreditDebitType = w.CreditDebitType,
                                                                           Description = w.Description,
                                                                           IsDeferred = w.IsDeferred,
                                                                           PaymentType = w.PaymentType,
                                                                           WalletId = w.WalletId,
                                                                           TransactionCountryId = w.TransactionCountryId
                                                                       }).ToList();

            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public async Task<WalletTransactionSummary> GetWalletTransactionSummary(DashboardFilterCriteria dashboardFilterCriteria)
        {
            try
            {
                var result = new WalletTransactionSummary
                {
                    CreditAmount = 0,
                    DebitAmount = 0
                };

                var StartDate = DateTime.Now;
                var EndDate = DateTime.Now;

                //get startDate and endDate
                var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                StartDate = queryDate.Item1;
                EndDate = queryDate.Item2;

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
                SqlParameter endDate = new SqlParameter("@EndDate", EndDate);
                SqlParameter countryId = new SqlParameter("@CountryId", dashboardFilterCriteria.ActiveCountryId);

                SqlParameter[] param = new SqlParameter[]
                {
                    startDate,
                    endDate,
                    countryId
                };

                var summary = await _context.Database.SqlQuery<WalletTransactionSummary>("WalletTransactionSummary " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                if (summary != null)
                {
                    result.CreditAmount = summary.CreditAmount;
                    result.DebitAmount = summary.DebitAmount;
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WalletPaymentLogSummary> GetWalletPaymentSummary(DashboardFilterCriteria dashboardFilterCriteria)
        {
            try
            {
                var result = new WalletPaymentLogSummary
                {
                    Paystack = 0,
                    TheTeller = 0,
                    Flutterwave = 0,
                    USSD = 0
                };

                var StartDate = DateTime.Now;
                var EndDate = DateTime.Now;
                
                //get startDate and endDate
                var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                StartDate = queryDate.Item1;
                EndDate = queryDate.Item2;


                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
                SqlParameter endDate = new SqlParameter("@EndDate", EndDate);
                SqlParameter countryId = new SqlParameter("@CountryId", dashboardFilterCriteria.ActiveCountryId);

                SqlParameter[] param = new SqlParameter[]
                {
                    startDate,
                    endDate,
                    countryId
                };

                var summary = await _context.Database.SqlQuery<WalletPaymentLogSummary>("WalletPaymentLogSummary " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                if (summary != null)
                {
                    result.Paystack = summary.Paystack;
                    result.TheTeller = summary.TheTeller;
                    result.USSD = summary.USSD;
                    result.Flutterwave = summary.Flutterwave;
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionHistoryAsync(ShipmentCollectionFilterCriteria dateFilter)
        {
            //get startDate and endDate
            var queryDate = dateFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var walletTransactionContext = _context.WalletTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.PaymentTypeReference.Contains("2012GIGL")).AsQueryable();

            List<WalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                               select new WalletTransactionDTO()
                                                               {
                                                                   WalletTransactionId = w.WalletTransactionId,
                                                                   DateOfEntry = w.DateOfEntry,
                                                                   Amount = w.Amount,
                                                                   CreditDebitType = w.CreditDebitType,
                                                                   Description = w.Description,
                                                                   IsDeferred = w.IsDeferred,
                                                                   PaymentType = w.PaymentType,
                                                                   UserId = w.UserId,
                                                                   PaymentTypeReference = w.PaymentTypeReference,
                                                                   ServiceCentreId = w.ServiceCentreId,
                                                                   ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                                   {
                                                                       Code = x.Code,
                                                                       Name = x.Name
                                                                   }).FirstOrDefault(),
                                                                   WalletId = w.WalletId,
                                                                   Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                                                   {
                                                                       Balance = x.Balance,
                                                                       CompanyType = x.CompanyType,
                                                                       CustomerCode = x.CustomerCode,
                                                                       CustomerId = x.CustomerId,
                                                                       CustomerType = x.CustomerType,
                                                                       WalletNumber = x.WalletNumber,
                                                                   }).FirstOrDefault()
                                                               }).OrderByDescending(s => s.DateOfEntry).ToList();

            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<WalletCreditTransactionSummaryDTO> GetWalletCreditTransactionHistoryAsync(ShipmentCollectionFilterCriteria dateFilter)
        {
            //get startDate and endDate
            var queryDate = dateFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var walletTransactionContext = _context.WalletTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.CreditDebitType == CreditDebitType.Credit).AsQueryable();

            WalletCreditTransactionSummaryDTO summary = new WalletCreditTransactionSummaryDTO
            {
                WalletCreditTransactions = (from w in walletTransactionContext
                                            join u in Context.Users on w.UserId equals u.Id
                                            join c in Context.Country on u.UserActiveCountryId equals c.CountryId
                                            select new WalletCreditTransactionDTO()
                                            {
                                                DateOfEntry = w.DateOfEntry,
                                                Amount = w.Amount,
                                                Description = w.Description,
                                                Reference = w.PaymentTypeReference,
                                                User =  new UserDetailForCreditTransactionDTO
                                                {
                                                    CustomerCode = u.UserChannelCode,
                                                    FirstName = u.FirstName,
                                                    LastName = u.LastName
                                                },
                                                CurrencyCode = c.CurrencyCode,
                                                CurrencySymbol = c.CurrencySymbol
                                            }).OrderByDescending(s => s.DateOfEntry).ToList(),
                
            };

            summary.NairaAmount  = summary.WalletCreditTransactions.Any() ? summary.WalletCreditTransactions.Where(x => x.CurrencyCode == "NGN").Select(t => t.Amount).Sum() : 0.00m;
            summary.DollarAmount  = summary.WalletCreditTransactions.Any() ? summary.WalletCreditTransactions.Where(x => x.CurrencyCode == "USD").Select(t => t.Amount).Sum() : 0.00m;
            summary.CedisAmount  = summary.WalletCreditTransactions.Any() ? summary.WalletCreditTransactions.Where(x => x.CurrencyCode == "GHS").Select(t => t.Amount).Sum() : 0.00m;
            summary.PoundsAmount  = summary.WalletCreditTransactions.Any() ? summary.WalletCreditTransactions.Where(x => x.CurrencyCode == "GBP").Select(t => t.Amount).Sum() : 0.00m;

            return Task.FromResult(summary);
        }

        public Task<List<WalletCreditTransactionConvertedDTO>> GetWalletConversionTransactionHistoryAsync(ShipmentCollectionFilterCriteria dateFilter)
        {
            //get startDate and endDate
            var queryDate = dateFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var paymentLogs = _context.WalletPaymentLog.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.isConverted == true).AsQueryable();

           List<WalletCreditTransactionConvertedDTO> results = (from p in paymentLogs
                                                                join w in Context.WalletTransactions on p.Reference equals w.PaymentTypeReference
                                                                join u in Context.Users on w.UserId equals u.Id
                                                                join c in Context.Country on u.UserActiveCountryId equals c.CountryId
                                                                  select new WalletCreditTransactionConvertedDTO()
                                                                  {
                                                                      DateOfEntry = p.DateCreated,
                                                                      FundedAmount = p.Amount,
                                                                      CardType = p.CardType,
                                                                      EquivalentAmount = w.Amount,
                                                                      Description = w.Description,
                                                                      Reference = w.PaymentTypeReference,
                                                                      ProcessingPartner = p.OnlinePaymentType,
                                                                      User = new UserDetailForCreditTransactionDTO
                                                                      {
                                                                          CustomerCode = u.UserChannelCode,
                                                                          FirstName = u.FirstName,
                                                                          LastName = u.LastName
                                                                      },
                                                                      EquivalentCurrencyCode = c.CurrencyCode,
                                                                      EquivalentCurrencySymbol = c.CurrencySymbol
                                                                  }).OrderByDescending(s => s.DateOfEntry).ToList();
           
            return Task.FromResult(results);
        }
    }
}