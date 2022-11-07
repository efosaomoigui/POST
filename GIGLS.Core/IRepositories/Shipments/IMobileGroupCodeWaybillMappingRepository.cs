using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IMobileGroupCodeWaybillMappingRepository : IRepository<MobileGroupCodeWaybillMapping>
    {
        Task<string> GetGroupCode(string waybill);
    }
}
