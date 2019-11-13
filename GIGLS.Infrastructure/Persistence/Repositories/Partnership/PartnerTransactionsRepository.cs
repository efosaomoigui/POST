using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IRepositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
{
    public class PartnerTransactionsRepository : Repository<PartnerTransactions, GIGLSContext>, IPartnerTransactionsRepository
    {

        public PartnerTransactionsRepository(GIGLSContext context) : base(context)
        {
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
            var partnersTrans = Context.PartnerTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            List<PartnerTransactionsDTO> partnerTransDTO = (from r in partnersTrans
                                                            select new PartnerTransactionsDTO()
                                                            {
                                                                Waybill = r.Waybill,
                                                                AmountReceived = r.AmountReceived,

                                                            }).ToList();

            return await Task.FromResult(partnerTransDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
