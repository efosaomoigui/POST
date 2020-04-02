using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class MobileGroupCodeWaybillMappingRepository : Repository<MobileGroupCodeWaybillMapping, GIGLSContext>, IMobileGroupCodeWaybillMappingRepository
    {
        private GIGLSContext _context;

        public MobileGroupCodeWaybillMappingRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetGroupCode(string waybill)
        {
            string groupCode =  null;
            var groupwaybillMapping = _context.MobileGroupCodeWaybillMapping.Where(x => x.WaybillNumber == waybill).FirstOrDefault();
            if(groupwaybillMapping != null)
            {
                groupCode = groupwaybillMapping.GroupCodeNumber;
            }
            return await Task.FromResult(groupCode);
        }
    }
}
