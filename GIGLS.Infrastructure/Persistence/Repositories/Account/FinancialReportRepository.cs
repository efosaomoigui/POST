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
using System.Data.SqlClient;
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
            var earningsBreakdownDTO = new EarningsBreakdownDTO();
            var results = await GetFinancialBreakdownSummary(dashboardFilter);

            earningsBreakdownDTO.GIGGO = results.GIGGo;
            earningsBreakdownDTO.Agility = results.Agility;
            earningsBreakdownDTO.IntlShipments = results.Intl;

            earningsBreakdownDTO.Demurrage = await GetTotalFinancialReportDemurrage(dashboardFilter);

            return earningsBreakdownDTO;
        }

        public async Task<List<FinancialReportDTO>> GetFinancialReportBreakdown(AccountFilterCriteria accountFilterCriteria)
        {
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


            //declare parameters for the stored procedure
            SqlParameter startDates = new SqlParameter("@StartDate", startDate);
            SqlParameter endDates = new SqlParameter("@EndDate", endDate);
            SqlParameter countryId = new SqlParameter("@CountryId", accountFilterCriteria.CountryId);
            SqlParameter source = new SqlParameter("@Source", accountFilterCriteria.ServiceCenterId);


            SqlParameter[] param = new SqlParameter[]
            {
                    startDates,
                    endDates,
                    countryId,
                    source
            };

            var summary = await _context.Database.SqlQuery<FinancialReportDTO>("FinancialReports " +
               "@StartDate, @EndDate, @Source, @CountryId",
               param).ToListAsync();

            return await Task.FromResult(summary);
        }

        public Task<List<FinancialReportDTO>> GetFinancialReportBreakdownForDemurrage(AccountFilterCriteria accountFilterCriteria)
        {
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

            var transactionContext = _context.FinancialReport.Where(x => x.CountryId == accountFilterCriteria.CountryId &&
            x.Demurrage > 0 &&
            x.DateCreated >= startDate && x.DateCreated < endDate).AsQueryable();

            List<FinancialReportDTO> reportDTO = (from w in transactionContext
                                                  select new FinancialReportDTO()
                                                  {

                                                      Waybill = w.Waybill,
                                                      Earnings = w.Earnings,
                                                      GrandTotal = w.GrandTotal,
                                                      PartnerEarnings = w.PartnerEarnings,
                                                      Demurrage = w.Demurrage,
                                                      Source = w.Source,
                                                      CurrencySymbol = _context.Country.Where(s => s.CountryId == w.CountryId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                                  }).ToList();

            return Task.FromResult(reportDTO.OrderByDescending(s => s.DateCreated).ToList());

        }

        public async Task<decimal> GetTotalFinancialReportEarnings(DashboardFilterCriteria dashboardFilterCriteria)
        {
            try
            {
                var StartDate = DateTime.Now;
                var EndDate = DateTime.Now;

                //If No Date Supplied
                if (!dashboardFilterCriteria.StartDate.HasValue && !dashboardFilterCriteria.EndDate.HasValue)
                {
                    var threeMonthsAgo = DateTime.Now.AddMonths(-2);  
                    StartDate = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
                    EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }
                else
                {
                    //get startDate and endDate
                    var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                    StartDate = queryDate.Item1;
                    EndDate = queryDate.Item2;
                }

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

                var summaryResult = await _context.Database.SqlQuery<decimal?>("FinancialReportsEarnings " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                decimal summary = 0.00M;

                if (summaryResult != null)
                {
                    summary = (decimal)summaryResult;
                }

                return await Task.FromResult(summary);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetTotalFinancialReportDemurrage(DashboardFilterCriteria dashboardFilterCriteria)
        {
            try
            {
                var StartDate = DateTime.Now;
                var EndDate = DateTime.Now;

                //If No Date Supplied
                if (!dashboardFilterCriteria.StartDate.HasValue && !dashboardFilterCriteria.EndDate.HasValue)
                {
                    var threeMonthsAgo = DateTime.Now.AddMonths(-2);  
                    StartDate = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
                    EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }
                else
                {
                    //get startDate and endDate
                    var queryDate = dashboardFilterCriteria.getStartDateAndEndDate();
                    StartDate = queryDate.Item1;
                    EndDate = queryDate.Item2;
                }

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

                var summaryResult = await _context.Database.SqlQuery<decimal?>("FinancialReportsDemurrage " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                decimal summary = 0.00M;

                if (summaryResult != null)
                {
                    summary = (decimal)summaryResult;
                }

                return await Task.FromResult(summary);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FinancialBreakdownSummaryDTO> GetFinancialBreakdownSummary(DashboardFilterCriteria dashboardFilterCriteria)
        {
            try
            {
                var result = new FinancialBreakdownSummaryDTO
                {
                    GIGGo = 0,
                    Agility = 0,
                    Intl = 0
                };

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

                var summary = await _context.Database.SqlQuery<FinancialBreakdownSummaryDTO>("FinancialBreakdownSummary " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                if (summary != null)
                {
                    result.GIGGo = summary.GIGGo;
                    result.Agility = summary.Agility;
                    result.Intl = summary.Intl;
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
