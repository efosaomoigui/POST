using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
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
