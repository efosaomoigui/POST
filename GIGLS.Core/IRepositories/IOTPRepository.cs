using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IOTPRepository : IRepository<Domain.OTP>
    {
         Task<OTP> IsOTPValid(int OTP);
    }
}