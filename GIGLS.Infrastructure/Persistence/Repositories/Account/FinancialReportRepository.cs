using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Account;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Account
{
    public class FinancialReportRepository : Repository<FinancialReport, GIGLSContext>, IFinancialReportRepository
    {
        private GIGLSContext _context;

        public FinancialReportRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        //Get Earnings Breakdown
        public async Task<EarningsBreakdownDTO> GetEarningsBreakdown(DashboardFilterCriteria dashboardFilter)
        {
            //get startDate and endDate
            var queryDate = dashboardFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var earnings = _context.FinancialReport.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.CountryId == dashboardFilter.ActiveCountryId);

            var earningsBreakdownDTO = new EarningsBreakdownDTO();

            earningsBreakdownDTO.GIGGO = earnings.Where(x => x.Source == ReportSource.GIGGo).Select(x => x.Earnings).DefaultIfEmpty(0).Sum();
            earningsBreakdownDTO.Agility = earnings.Where(x => x.Source == ReportSource.Agility).Select(x => x.Earnings).DefaultIfEmpty(0).Sum();
            earningsBreakdownDTO.Demurrage = earnings.Select(x => x.Demurrage).DefaultIfEmpty(0).Sum();

            var intlShipments = _context.Invoice.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.PaymentStatus == PaymentStatus.Paid &&
              s.PaymentMethod == PaymentType.Cash.ToString() && s.IsInternational == true).Select(x => x.Amount).DefaultIfEmpty(0).Sum();

            earningsBreakdownDTO.IntlShipments = intlShipments;

            return earningsBreakdownDTO;

        }


        public Task<List<FinancialReportDTO>> GetFinancialReportBreakdown(AccountFilterCriteria accountFilterCriteria)
        {
            var transactionContext = _context.FinancialReport.Where(x => x.CountryId == accountFilterCriteria.CountryId).AsQueryable();

            if(accountFilterCriteria.CompanyType == "Demurrage")
            {
                transactionContext = transactionContext.Where(x => x.Demurrage > 0);
            }
            else
            {
                transactionContext = transactionContext.Where(x => x.Source.ToString() == accountFilterCriteria.CompanyType);
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

            transactionContext = transactionContext.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<FinancialReportDTO> transactionDTO = (from w in transactionContext
                                                       select new FinancialReportDTO()
                                                       {
                                                           Waybill = w.Waybill,
                                                           Source = w.Source,
                                                           Earnings = w.Earnings,
                                                           PartnerEarnings = w.PartnerEarnings,
                                                           GrandTotal = w.GrandTotal,
                                                           Demurrage = w.Demurrage,
                                                           DateCreated = w.DateCreated,
                                                           CurrencySymbol = _context.Country.Where(x => x.CountryId == w.CountryId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                                          
                                                       }).ToList();

            return Task.FromResult(transactionDTO.OrderByDescending(s => s.DateCreated).ToList());
        }

        public Task<List<FinancialReportDTO>> GetIntlReportBreakdown(AccountFilterCriteria accountFilterCriteria)
        {
            //filter by service center
            var transactionContext = _context.Invoice.Where(x => x.IsInternational == true && x.PaymentStatus == PaymentStatus.Paid 
            && x.PaymentMethod == PaymentType.Cash.ToString()).AsQueryable();


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

            transactionContext = transactionContext.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<FinancialReportDTO> transactionDTO = (from w in transactionContext
                                                       select new FinancialReportDTO()
                                                       {
                                                           Waybill = w.Waybill,
                                                           Source = ReportSource.Intl,
                                                           Earnings = w.Amount,
                                                           PartnerEarnings = 0,
                                                           GrandTotal = w.Amount,
                                                           Demurrage = 0,
                                                           DateCreated = w.DateCreated,
                                                           CurrencySymbol = _context.Country.Where(x => x.CountryId == w.CountryId).Select(x => x.CurrencySymbol).FirstOrDefault()

                                                       }).ToList();

            return Task.FromResult(transactionDTO.OrderByDescending(s => s.DateCreated).ToList());
        }

    }
}
