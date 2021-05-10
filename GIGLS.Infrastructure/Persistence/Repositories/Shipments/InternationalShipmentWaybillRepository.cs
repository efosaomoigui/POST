using GIGLS.Core.Domain.DHL;
using GIGLS.Core.DTO.DHL;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class InternationalShipmentWaybillRepository : Repository<InternationalShipmentWaybill, GIGLSContext>, IInternationalShipmentWaybillRepository
    {
        private GIGLSContext _context;

        public InternationalShipmentWaybillRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var shipments = _context.InternationalShipmentWaybill.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            var shipmentDto = (from s in shipments
                               select new InternationalShipmentWaybillDTO
                               {
                                   Id = s.Id,
                                   DateCreated = s.DateCreated,
                                   DateModified = s.DateModified,
                                   Waybill= s.Waybill,
                                   ShipmentIdentificationNumber = s.ShipmentIdentificationNumber,
                                   PackageResult = s.PackageResult,
                                   InternationalShipmentStatus = s.InternationalShipmentStatus,
                                   OutBoundChannel = s.OutBoundChannel
                               }).ToList();
            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
