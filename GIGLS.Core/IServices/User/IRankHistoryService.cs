using POST.Core.DTO.Stores;
using POST.Core.DTO.User;
using System.Threading.Tasks;

namespace POST.Core.IServices.User
{
    public interface IRankHistoryService
    {
        Task AddRankHistory(RankHistoryDTO history);
    }
}
