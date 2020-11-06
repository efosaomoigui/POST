using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
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

        public Task<List<FinancialReportDTO>> GetFinancialReportBreakdown(AccountFilterCriteria accountFilterCriteria)
        {
            //filter by service center
            var transactionContext = _context.FinancialReport.Where(x => x.Source.ToString() == accountFilterCriteria.CompanyType && x.CountryId == accountFilterCriteria.CountryId).AsQueryable();


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
                                                           Demurrage = w.Demurrage
                                                          
                                                       }).OrderByDescending(s => s.DateCreated).ToList();

            return Task.FromResult(transactionDTO.OrderByDescending(s => s.DateCreated).ToList());
        }

    }
}
