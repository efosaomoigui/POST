using POST.Core.Domain.Partnership;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using POST.Core.IRepositories.Partnership;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.Partnership
{
    public class PartnerPayoutRepository : Repository<PartnerPayout, GIGLSContext>, IPartnerPayoutRepository
    {
        private GIGLSContext _context;
        public PartnerPayoutRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PartnerPayoutDTO>> GetPartnerPayoutByDate(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var partnerPayout = _context.PartnerPayout.AsQueryable().Where(s => s.DateProcessed >= startDate && s.DateProcessed < endDate);

            List<PartnerPayoutDTO> partnerTransDTO = (from r in partnerPayout
                                                            select new PartnerPayoutDTO()
                                                            {
                                                                Amount = r.Amount,
                                                                DateProcessed = r.DateProcessed,
                                                                PartnerName = r.PartnerName,
                                                                ProcessedBy = r.ProcessedBy,
                                                                StartDate = r.StartDate,
                                                                EndDate = r.EndDate
                                                            }).ToList();

            return await Task.FromResult(partnerTransDTO.OrderByDescending(x => x.DateProcessed).ToList());
        }
    }
   
}

