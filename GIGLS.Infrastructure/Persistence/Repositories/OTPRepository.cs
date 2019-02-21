using System;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class OTPRepository : Repository<OTP, GIGLSContext>, IOTPRepository
    {
        public OTPRepository(GIGLSContext context) : base(context)
        {
        }

       
        public async Task<OTP> IsOTPValid(int OTP)
        {
            var message = new OTP();
            DateTime LatestTime = DateTime.Now;
            try
            {
                message = Context.OTP.Where(x => x.Otp == OTP).FirstOrDefault();
                if (message != null)
                {
                    TimeSpan span = LatestTime.Subtract(message.DateCreated);
                    int difference = Convert.ToInt32(span.TotalMinutes);
                    if (difference < 5)
                        message.IsValid = true;
                    return await Task.FromResult(message);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return await Task.FromResult(message);
        }
        
    }
}
