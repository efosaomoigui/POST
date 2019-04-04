using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class UserLoginEmailRepository : Repository<UserLoginEmail, GIGLSContext>, IUserLoginEmailRepository
    {
        public UserLoginEmailRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<UserLoginEmail> GetUserLoginEmailByEmail(string email)
        {
            var userLoginEmail = Context.UserLoginEmail.Where(x => x.Email.Equals(email)).FirstOrDefault();
            return Task.FromResult(userLoginEmail);
        }
    }
    
}
