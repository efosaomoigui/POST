using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.Entity;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Report;
using System.Data.SqlClient;

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

        public Task<List<WalletTransactionDTO>> GetWalletTransactionDateAsync (int[] serviceCentreIds, ShipmentCollectionFilterCriteria dateFilter)
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
                                                                 UserId = w.UserId,
                                                                 ServiceCentreId = w.ServiceCentreId,
                                                                 ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO {
                                                                     Code = x.Code,
                                                                     Name = x.Name
                                                                 }).FirstOrDefault(),
                                                                 WalletId = w.WalletId,
                                                                 Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO {
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

                //get startDate and endDate
                var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                var StartDate = queryDate.Item1;
                var EndDate = queryDate.Item2;

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

                if(summary != null)
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
    }
}