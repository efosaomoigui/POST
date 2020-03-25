using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IMobileGroupCodeWaybillMappingRepository : IRepository<MobileGroupCodeWaybillMapping>
    {
        Task<string> GetGroupCode(string waybill);
    }
}
