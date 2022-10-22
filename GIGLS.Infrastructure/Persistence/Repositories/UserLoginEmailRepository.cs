using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
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
