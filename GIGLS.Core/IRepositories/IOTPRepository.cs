using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IOTPRepository : IRepository<Domain.OTP>
    {
         Task<OTP> IsOTPValid(int OTP);
    }
}