using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IUserLoginEmailRepository : IRepository<UserLoginEmail>
    {
        Task<UserLoginEmail> GetUserLoginEmailByEmail(string email);
    }
    
}
