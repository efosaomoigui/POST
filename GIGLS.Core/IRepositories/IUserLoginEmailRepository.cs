using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IUserLoginEmailRepository : IRepository<UserLoginEmail>
    {
        Task<UserLoginEmail> GetUserLoginEmailByEmail(string email);
    }
    
}
