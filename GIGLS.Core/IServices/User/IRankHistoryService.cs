using GIGLS.Core.DTO.Stores;
using GIGLS.Core.DTO.User;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.User
{
    public interface IRankHistoryService
    {
        Task AddRankHistory(RankHistoryDTO history);
    }
}
